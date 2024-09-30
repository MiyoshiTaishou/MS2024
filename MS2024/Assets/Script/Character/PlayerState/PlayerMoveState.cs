using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveState : IState
{
    private PlayerState character;
    private Vector2 moveInput;   
    private Rigidbody rb;        // Rigidbody参照

    public PlayerMoveState(PlayerState character)
    {
        this.character = character;
        // キャラクターの Rigidbody を取得
        rb = character.GetComponent<Rigidbody>();
    }

    public void Enter()
    {
        // キャラクターが移動状態に入るときの処理
        Debug.Log("移動処理に入ります");
        character.SetAnimation("APlayerWalk");
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

        if (moveInput.x < 0.0f)
        {
            character.gameObject.transform.localScale = new Vector3(character.initScale.x * -1, character.initScale.y, character.initScale.z);
        }
        else if (moveInput.x > 0.0f)
        {
            character.gameObject.transform.localScale = character.initScale;
        }
    }

    public void Exit()
    {
        // Idle状態を抜けるときの処理
    }
}
