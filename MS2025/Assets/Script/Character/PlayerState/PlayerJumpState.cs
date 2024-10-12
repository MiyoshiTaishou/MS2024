//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

///// <summary>
///// ジャンプ関連の処理
///// </summary>
//public class PlayerJumpState : IState
//{
//    private PlayerState character;    
//    private Rigidbody rb;
//    private Vector2 moveInput;

//    private float rayDistance = 1.5f;     
//    private string groundTag = "Ground";

//    public PlayerJumpState(PlayerState character)
//    {
//        this.character = character;
//    }

//    public void Enter()
//    {
//        rb = GetComponent<Rigidbody>();
//        rb.velocity = new Vector3(rb.velocity.x, jumpForce, rb.velocity.z);
//    }

//    public void Exit()
//    {
        
//    }

//    public void Update()
//    {
//        // 入力から移動ベクトルを取得
//        moveInput = input.actions["Move"].ReadValue<Vector2>();
//        Vector3 move = new Vector3(moveInput.x, 0, moveInput.y).normalized;

//        // 加速度を加味した速度の更新
//        currentSpeed += moveSpeedAcc * Time.deltaTime;

//        // 速度を最大速度で制限
//        currentSpeed = Mathf.Min(currentSpeed, maxSpeed);

//        // Rigidbody による移動
//        Vector3 velocity = move * currentSpeed;
//        velocity.y = rb.velocity.y;  // Y軸の速度は変更しない
//        rb.velocity = velocity;      // Rigidbody の速度を設定
     

//        //落下処理
//        if (rb.velocity.y < 0)
//        {
//            rb.velocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1) * Time.deltaTime;

//            RaycastHit hit;
//            if (Physics.Raycast(transform.position, Vector3.down, out hit, rayDistance))
//            {
//                if (hit.collider.CompareTag(groundTag))
//                {
//                    AnimatorStateInfo animStateInfo = GetComponent<Animator>().GetCurrentAnimatorStateInfo(0);
//                    if (!animStateInfo.IsName("APlayerLand"))
//                    {
//                        SetAnimation("APlayerLand");
//                    }
//                }
//            }
//        }

//        AnimatorStateInfo landAnimStateInfo = GetComponent<Animator>().GetCurrentAnimatorStateInfo(0);
//        if (landAnimStateInfo.IsName("APlayerLand") && landAnimStateInfo.normalizedTime >= 1.0f)
//            if (rb.velocity.y == 0)
//            {
//                ChangeState(new PlayerIdleState(character));
//            }
//    }    
//}   
