//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using Fusion;
//using Fusion.Addons.Physics;

//public class PlayerMoveState : IState
//{
//    private PlayerState character;
//    //private Vector2 moveInput;
//    private NetworkRigidbody3D networkRb;  // NetworkRigidbody 参照

//    public PlayerMoveState(PlayerState character)
//    {
//        this.character = character;
//        // NetworkRigidbody を取得
//        networkRb = GetComponent<NetworkRigidbody3D>();
//    }

//    public void Enter()
//    {
//        // キャラクターが移動状態に入るときの処理
//        Debug.Log("移動処理に入ります");
//        SetAnimation("APlayerWalk");
//    }

//    public void Update()
//    {
//        // ローカルプレイヤーのみ入力を取得する
//        if (Object.HasInputAuthority)
//        {
//            // 入力から移動ベクトルを取得
//            moveInput = input.actions["Move"].ReadValue<Vector2>();
//            Vector3 move = new Vector3(moveInput.x, 0, moveInput.y).normalized;

//            // 加速度を加味した速度の更新
//            currentSpeed += moveSpeedAcc * Time.deltaTime;

//            // 速度を最大速度で制限
//            currentSpeed = Mathf.Min(currentSpeed, maxSpeed);

//            // Rigidbody による移動
//            Vector3 velocity = move * currentSpeed;
//            velocity.y = networkRb.Rigidbody.velocity.y;  // Y軸の速度は変更しない

//            // NetworkRigidbody を使用して速度を更新
//            networkRb.Rigidbody.velocity = velocity;

//            // 向きの変更処理
//            if (moveInput.x < 0.0f)
//            {
//                gameObject.transform.localScale = new Vector3(initScale.x * -1, initScale.y, initScale.z);
//            }
//            else if (moveInput.x > 0.0f)
//            {
//                gameObject.transform.localScale = initScale;
//            }
//        }
//    }

//    public void Exit()
//    {
//        // 移動状態を抜けるときの処理
//    }
//}
