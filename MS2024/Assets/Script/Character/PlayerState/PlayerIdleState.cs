using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class PlayerIdleState : IState
{
    private PlayerState character;

    public PlayerIdleState(PlayerState character)
    {
        this.character = character;
    }

    public void Enter()
    {
        // キャラクターがIdle状態に入るときの処理
        //character.SetAnimation("Idle");
        Debug.Log("待機処理に入ります");
    }

    public void Update()
    {
        Vector2 move = character.input.actions["Move"].ReadValue<Vector2>();

        if (move.x != 0.0f && move.y != 0.0f)
        {
            character.ChangeState(new PlayerMoveState(character));
        }
    }

    public void Exit()
    {
        // Idle状態を抜けるときの処理
    }
}
