using Fusion;
using Fusion.Addons.Physics;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerJumpNet : NetworkBehaviour
{
    Animator animator;
    AudioSource audioSource;

    [Header("�W�����vSE"), SerializeField] private AudioClip jumpSE;

    [Networked] public NetworkButtons ButtonsPrevious { get; set; }

    [Networked] private bool isGround { get; set; }

    [SerializeField, Header("�W�����v�̗�")] private float jumpPower = 10.0f;
    [SerializeField, Header("�d��")] private float gravity = 9.8f;

    private Vector3 velocity;  // �v���C���[�̑��x
    private bool isJumping;    // �W�����v�����ǂ���

    public override void Spawned()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();

        // Unity�̎����d�͂̓I�t�ɂ��Ă���
        GetComponent<NetworkRigidbody3D>().Rigidbody.useGravity = false;
    }

    public override void FixedUpdateNetwork()
    {
        if (Object.HasStateAuthority && GetInput(out NetworkInputData data))
        {
            var pressed = data.Buttons.GetPressed(ButtonsPrevious);
            ButtonsPrevious = data.Buttons;

            Debug.Log("�n�ʂɒ����Ă��邩: " + isGround);

            // �W�����v�{�^����������A���n�ʂɂ���Ƃ��W�����v����
            if (pressed.IsSet(NetworkInputButtons.Jump) && isGround && !isJumping)
            {
                RPC_Jump();
                isJumping = true;  // �W�����v���ɐݒ�
                Debug.Log("�W�����v���܂�");
            }

            // ����̏d�͌v�Z��K�p
            ApplyGravity();
        }
    }

    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void RPC_Jump()
    {
        audioSource.PlayOneShot(jumpSE);

        // �W�����v�̏����x��ݒ�
        velocity.y = jumpPower;
        isGround = false;  // �W�����v�����̂Œn�ʂɂ��Ȃ���Ԃɐݒ�
    }

    // �d�͂��蓮�Ōv�Z���ēK�p���郁�\�b�h
    private void ApplyGravity()
    {
        if (!isGround)  // �󒆂ɂ���ꍇ�ɂ̂ݏd�͂�K�p
        {
            // �d�͉����x�𑬓x�ɉ�����
            velocity.y -= gravity * Runner.DeltaTime;

            // �v�Z�������x��Rigidbody�ɓK�p
            Vector3 currentVelocity = GetComponent<NetworkRigidbody3D>().Rigidbody.velocity;
            GetComponent<NetworkRigidbody3D>().Rigidbody.velocity = new Vector3(currentVelocity.x, velocity.y, currentVelocity.z);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // �n�ʂɒ��n�����ꍇ�̏���
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGround = true;
            isJumping = false;  // �W�����v�I��
            velocity.y = 0;     // �������x�����Z�b�g
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        // �n�ʂ��痣�ꂽ�ꍇ�̏���
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGround = false;
        }
    }
}
