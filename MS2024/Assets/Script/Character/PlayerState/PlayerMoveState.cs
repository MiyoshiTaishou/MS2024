using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveState : IState
{
    private PlayerState character;

    private Vector2 moveInput;

    private float currentSpeed;  // 現在の移動速度


    public PlayerMoveState(PlayerState character)
    {
        this.character = character;
    }

    public void Enter()
    {
        // キャラクターが移動状態に入るときの処理
        //character.SetAnimation("Idle");
        Debug.Log("移動処理に入ります");        
    }

    public void Update()
    {
        // 入力から移動ベクトルを取得
        moveInput = character.input.actions["Move"].ReadValue<Vector2>();
        Vector3 move = new Vector3(moveInput.x, 0, moveInput.y).normalized;

        // 加速度を加味した速度の更新
        currentSpeed += character.moveSpeedAcc * Time.deltaTime;

        // 速度を最大速度で制限
        currentSpeed = Mathf.Min(currentSpeed, character.maxSpeed);

        // 移動を適用
        character.transform.Translate(move * currentSpeed * Time.deltaTime, Space.World);     
    }

    public void Exit()
    {
        // Idle状態を抜けるときの処理
    }
}
