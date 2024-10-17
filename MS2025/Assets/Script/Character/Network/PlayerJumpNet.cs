using Fusion;
using Fusion.Addons.Physics;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerJumpNet : NetworkBehaviour
{
    Animator animator;
    AudioSource audioSource;

    [Header("ジャンプSE"), SerializeField] private AudioClip jumpSE;

    [Networked] public NetworkButtons ButtonsPrevious { get; set; }

    [Networked] private bool isGround { get; set; }

    [SerializeField, Header("ジャンプの力")] private float jumpPower = 10.0f;
    [SerializeField, Header("重力")] private float gravity = 9.8f;

    private Vector3 velocity;  // プレイヤーの速度
    private bool isJumping;    // ジャンプ中かどうか

    public override void Spawned()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();

        // Unityの自動重力はオフにしておく
        GetComponent<NetworkRigidbody3D>().Rigidbody.useGravity = false;
    }

    public override void FixedUpdateNetwork()
    {
        if (Object.HasStateAuthority && GetInput(out NetworkInputData data))
        {
            var pressed = data.Buttons.GetPressed(ButtonsPrevious);
            ButtonsPrevious = data.Buttons;

            Debug.Log("地面に着いているか: " + isGround);

            // ジャンプボタンが押され、かつ地面にいるときジャンプする
            if (pressed.IsSet(NetworkInputButtons.Jump) && isGround && !isJumping)
            {
                RPC_Jump();
                isJumping = true;  // ジャンプ中に設定
                Debug.Log("ジャンプします");
            }

            // 自作の重力計算を適用
            ApplyGravity();
        }
    }

    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void RPC_Jump()
    {
        audioSource.PlayOneShot(jumpSE);

        // ジャンプの初速度を設定
        velocity.y = jumpPower;
        isGround = false;  // ジャンプしたので地面にいない状態に設定
    }

    // 重力を手動で計算して適用するメソッド
    private void ApplyGravity()
    {
        if (!isGround)  // 空中にいる場合にのみ重力を適用
        {
            // 重力加速度を速度に加える
            velocity.y -= gravity * Runner.DeltaTime;

            // 計算した速度をRigidbodyに適用
            Vector3 currentVelocity = GetComponent<NetworkRigidbody3D>().Rigidbody.velocity;
            GetComponent<NetworkRigidbody3D>().Rigidbody.velocity = new Vector3(currentVelocity.x, velocity.y, currentVelocity.z);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // 地面に着地した場合の処理
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGround = true;
            isJumping = false;  // ジャンプ終了
            velocity.y = 0;     // 垂直速度をリセット
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        // 地面から離れた場合の処理
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGround = false;
        }
    }
}
