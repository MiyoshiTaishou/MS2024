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
        
    }

    public void Exit()
    {
        // Idle状態を抜けるときの処理
    }
}
