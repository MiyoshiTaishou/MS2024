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

    // �n�ʂƂ̋����𔻒肷��Ray�̒���
    private float rayDistance = 1.5f; // ��������n�ʂ܂ł̋���
    // �n�ʂ̃^�O��
    private string groundTag = "Ground"; // �n�ʂ̃^�O��"Ground"�Ƃ���

    public PlayerJumpState(PlayerState character)
    {
        this.character = character;
    }

    public void Enter()
    {
        rb = character.GetComponent<Rigidbody>();
        rb.velocity = new Vector3(rb.velocity.x, character.jumpForce, rb.velocity.z);
        character.SetAnimation("APlayerJump");
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

        // ��������
        if (rb.velocity.y < 0)
        {
            rb.velocity += Vector3.up * Physics.gravity.y * (character.fallMultiplier - 1) * Time.deltaTime;

            // �������牺������Ray���΂��Ēn�ʂƂ̋����𑪂�
            RaycastHit hit;
            if (Physics.Raycast(character.transform.position, Vector3.down, out hit, rayDistance))
            {
                // �q�b�g�����I�u�W�F�N�g��"Ground"�^�O�������Ă���ꍇ�A���n�A�j���[�V�������Đ�
                if (hit.collider.CompareTag(groundTag))
                {
                    // ���n�A�j���[�V�������Đ�����Ă��Ȃ��ꍇ�A�Đ�����
                    AnimatorStateInfo animStateInfo = character.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0);
                    if (!animStateInfo.IsName("APlayerLand"))
                    {
                        character.SetAnimation("APlayerLand");
                    }
                }
            }
        }            

        // ���n�A�j���[�V����������������Idle�X�e�[�g�Ɉڍs
        AnimatorStateInfo landAnimStateInfo = character.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0);
        if (landAnimStateInfo.IsName("APlayerLand") && landAnimStateInfo.normalizedTime >= 1.0f)
        {
            // �A�j���[�V�������I�����Ă���Idle�X�e�[�g�ɑJ��
            character.ChangeState(new PlayerIdleState(character));
        }
    }
}
