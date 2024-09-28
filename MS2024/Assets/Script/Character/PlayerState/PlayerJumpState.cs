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

    public PlayerJumpState(PlayerState character)
    {
        this.character = character;
    }

    public void Enter()
    {
        rb = character.GetComponent<Rigidbody>();
        rb.velocity = new Vector3(rb.velocity.x, character.jumpForce, rb.velocity.z);
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

        //落下処理
        if (rb.velocity.y < 0)
        {
            rb.velocity += Vector3.up * Physics.gravity.y * (character.fallMultiplier - 1) * Time.deltaTime;
        }

        if(rb.velocity.y == 0)
        {
            character.ChangeState(new PlayerIdleState(character));
        }
    }   
}
