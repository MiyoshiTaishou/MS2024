using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Windows;
using UnityEngine.InputSystem;

public class LocalPlayerJump : MonoBehaviour
{
    private Rigidbody rb;
    private Vector2 moveInput;

    private float rayDistance = 1.5f;
    private string groundTag = "Ground";

    private Animator animator;

    private float currentSpeed;
    // ジャンプ関連
    public float jumpForce = 5.0f;
    public float fallMultiplier = 2.5f; // 落下速度の強化

    // インスペクターで調整可能な移動速度と加速度
    public float moveSpeed = 5.0f;
    public float moveSpeedAcc = 1.0f;
    public float maxSpeed = 10.0f;

    bool jump = false;
    public PlayerInput input;


    void Start()
    {
        animator = GetComponent<Animator>();
        input = GetComponent<PlayerInput>();
        rb = GetComponent<Rigidbody>();
        rb.velocity = new Vector3(rb.velocity.x, jumpForce, rb.velocity.z);
    }


    public void Update()
    {
        if(jump)
        {
            // 入力から移動ベクトルを取得

            Vector3 move = new Vector3(moveInput.x, jumpForce, moveInput.y).normalized;

            // 加速度を加味した速度の更新
            currentSpeed += moveSpeedAcc * Time.deltaTime;

            // 速度を最大速度で制限
            currentSpeed = Mathf.Min(currentSpeed, maxSpeed);

            // Rigidbody による移動
            Vector3 velocity = move * currentSpeed;
           // velocity.y = rb.velocity.y;  // Y軸の速度は変更しない
            rb.velocity = velocity;      // Rigidbody の速度を設定

            rb.velocity = new Vector3(rb.velocity.x, jumpForce, rb.velocity.z);


            Debug.Log(rb.velocity);

            //rb.AddForce(move);

            //落下処理
            if (rb.velocity.y < 0)
            {
                rb.velocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1) * Time.deltaTime;

                RaycastHit hit;
                if (Physics.Raycast(transform.position, Vector3.down, out hit, rayDistance))
                {
                    if (hit.collider.CompareTag(groundTag))
                    {
                        AnimatorStateInfo animStateInfo = GetComponent<Animator>().GetCurrentAnimatorStateInfo(0);
                        if (!animStateInfo.IsName("APlayerLand"))
                        {
                            animator.Play("APlayerLand");
                        }
                    }
                }
            }

            AnimatorStateInfo landAnimStateInfo = GetComponent<Animator>().GetCurrentAnimatorStateInfo(0);
            if (landAnimStateInfo.IsName("APlayerLand") && landAnimStateInfo.normalizedTime >= 1.0f)
                animator.Play("APlayerIdle");


            jump = false;
        }


    }

    /// <summary>
    /// コントローラー入力
    /// </summary>
    /// <param name="context"></param>
    public void Jump(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            jump = true;
            moveInput = input.actions["Move"].ReadValue<Vector2>();

        }

    }
}
