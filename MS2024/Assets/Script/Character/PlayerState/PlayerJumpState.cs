using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ジャンプ関連の処理
/// </summary>
public class PlayerJumpState : IState
{
    private PlayerState character;
    private Rigidbody rb;
    private Vector2 moveInput;

    // 地面との距離を判定するRayの長さ
    private float rayDistance = 1.5f; // 足元から地面までの距離
    // 地面のタグ名
    private string groundTag = "Ground"; // 地面のタグを"Ground"とする

    public PlayerJumpState(PlayerState character)
    {
        this.character = character;
    }

    public void Enter()
    {
        rb = character.GetComponent<Rigidbody>();
        rb.velocity = new Vector3(rb.velocity.x, character.jumpForce, rb.velocity.z);
        character.SetAnimation("APlayerJump");
    }

    public void Exit()
    {

    }

    public void Update()
    {
        // 入力から移動ベクトルを取得
        moveInput = character.input.actions["Move"].ReadValue<Vector2>();
        Vector3 move = new Vector3(moveInput.x, 0, moveInput.y).normalized;

        // 加速度を加味した速度の更新
        character.currentSpeed += character.moveSpeedAcc * Time.deltaTime;

        // 速度を最大速度で制限
        character.currentSpeed = Mathf.Min(character.currentSpeed, character.maxSpeed);

        // Rigidbody による移動
        Vector3 velocity = move * character.currentSpeed;
        velocity.y = rb.velocity.y;  // Y軸の速度は変更しない
        rb.velocity = velocity;      // Rigidbody の速度を設定

        // 落下処理
        if (rb.velocity.y < 0)
        {
            rb.velocity += Vector3.up * Physics.gravity.y * (character.fallMultiplier - 1) * Time.deltaTime;

            // 足元から下方向にRayを飛ばして地面との距離を測る
            RaycastHit hit;
            if (Physics.Raycast(character.transform.position, Vector3.down, out hit, rayDistance))
            {
                // ヒットしたオブジェクトが"Ground"タグを持っている場合、着地アニメーションを再生
                if (hit.collider.CompareTag(groundTag))
                {
                    // 着地アニメーションが再生されていない場合、再生する
                    AnimatorStateInfo animStateInfo = character.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0);
                    if (!animStateInfo.IsName("APlayerLand"))
                    {
                        character.SetAnimation("APlayerLand");
                    }
                }
            }
        }            

        // 着地アニメーションが完了したらIdleステートに移行
        AnimatorStateInfo landAnimStateInfo = character.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0);
        if (landAnimStateInfo.IsName("APlayerLand") && landAnimStateInfo.normalizedTime >= 1.0f)
        {
            // アニメーションが終了してからIdleステートに遷移
            character.ChangeState(new PlayerIdleState(character));
        }
    }
}
