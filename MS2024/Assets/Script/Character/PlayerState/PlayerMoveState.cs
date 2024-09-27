using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveState : IState
{
    private PlayerState character;

    private Vector2 moveInput;

    private float currentSpeed;  // ���݂̈ړ����x


    public PlayerMoveState(PlayerState character)
    {
        this.character = character;
    }

    public void Enter()
    {
        // �L�����N�^�[���ړ���Ԃɓ���Ƃ��̏���
        //character.SetAnimation("Idle");
        Debug.Log("�ړ������ɓ���܂�");        
    }

    public void Update()
    {
        // ���͂���ړ��x�N�g�����擾
        moveInput = character.input.actions["Move"].ReadValue<Vector2>();
        Vector3 move = new Vector3(moveInput.x, 0, moveInput.y).normalized;

        // �����x�������������x�̍X�V
        currentSpeed += character.moveSpeedAcc * Time.deltaTime;

        // ���x���ő呬�x�Ő���
        currentSpeed = Mathf.Min(currentSpeed, character.maxSpeed);

        // �ړ���K�p
        character.transform.Translate(move * currentSpeed * Time.deltaTime, Space.World);     
    }

    public void Exit()
    {
        // Idle��Ԃ𔲂���Ƃ��̏���
    }
}
