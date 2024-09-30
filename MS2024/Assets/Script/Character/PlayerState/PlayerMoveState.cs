using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using Fusion.Addons.Physics;

public class PlayerMoveState : IState
{
    private PlayerState character;
    private Vector2 moveInput;
    private NetworkRigidbody3D networkRb;  // NetworkRigidbody �Q��

    public PlayerMoveState(PlayerState character)
    {
        this.character = character;
        // NetworkRigidbody ���擾
        networkRb = character.GetComponent<NetworkRigidbody3D>();
    }

    public void Enter()
    {
        // �L�����N�^�[���ړ���Ԃɓ���Ƃ��̏���
        Debug.Log("�ړ������ɓ���܂�");
        character.SetAnimation("APlayerWalk");
    }

    public void Update()
    {
        // ���[�J���v���C���[�̂ݓ��͂��擾����
        if (character.Object.HasInputAuthority)
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
            velocity.y = networkRb.Rigidbody.velocity.y;  // Y���̑��x�͕ύX���Ȃ�

            // NetworkRigidbody ���g�p���đ��x���X�V
            networkRb.Rigidbody.velocity = velocity;

            // �����̕ύX����
            if (moveInput.x < 0.0f)
            {
                character.gameObject.transform.localScale = new Vector3(character.initScale.x * -1, character.initScale.y, character.initScale.z);
            }
            else if (moveInput.x > 0.0f)
            {
                character.gameObject.transform.localScale = character.initScale;
            }
        }
    }

    public void Exit()
    {
        // �ړ���Ԃ𔲂���Ƃ��̏���
    }
}
