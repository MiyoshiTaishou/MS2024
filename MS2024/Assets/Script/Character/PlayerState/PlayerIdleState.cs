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
        // �L�����N�^�[��Idle��Ԃɓ���Ƃ��̏���
        //character.SetAnimation("Idle");
        Debug.Log("�ҋ@�����ɓ���܂�");
    }

    public void Update()
    {
        
    }

    public void Exit()
    {
        // Idle��Ԃ𔲂���Ƃ��̏���
    }
}
