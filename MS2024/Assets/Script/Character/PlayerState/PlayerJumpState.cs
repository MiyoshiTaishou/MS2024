using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �W�����v�֘A�̏���
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
        // ���͂���ړ��x�N�g�����擾
        moveInput = character.input.actions["Move"].ReadValue<Vector2>();
        Vector3 move = new Vector3(moveInput.x, 0, moveInput.y).normalized;

        // �����x�������������x�̍X�V
        character.currentSpeed += character.moveSpeedAcc * Time.deltaTime;

        // ���x���ő呬�x�Ő���
        character.currentSpeed = Mathf.Min(character.currentSpeed, character.maxSpeed);

        // Rigidbody �ɂ��ړ�
        Vector3 velocity = move * character.currentSpeed;
        velocity.y = rb.velocity.y;  // Y���̑��x�͕ύX���Ȃ�
        rb.velocity = velocity;      // Rigidbody �̑��x��ݒ�

        //��������
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
