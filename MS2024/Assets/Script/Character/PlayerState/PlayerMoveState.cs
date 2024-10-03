//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using Fusion;
//using Fusion.Addons.Physics;

//public class PlayerMoveState : IState
//{
//    private PlayerState character;
//    //private Vector2 moveInput;
//    private NetworkRigidbody3D networkRb;  // NetworkRigidbody �Q��

//    public PlayerMoveState(PlayerState character)
//    {
//        this.character = character;
//        // NetworkRigidbody ���擾
//        networkRb = GetComponent<NetworkRigidbody3D>();
//    }

//    public void Enter()
//    {
//        // �L�����N�^�[���ړ���Ԃɓ���Ƃ��̏���
//        Debug.Log("�ړ������ɓ���܂�");
//        SetAnimation("APlayerWalk");
//    }

//    public void Update()
//    {
//        // ���[�J���v���C���[�̂ݓ��͂��擾����
//        if (Object.HasInputAuthority)
//        {
//            // ���͂���ړ��x�N�g�����擾
//            moveInput = input.actions["Move"].ReadValue<Vector2>();
//            Vector3 move = new Vector3(moveInput.x, 0, moveInput.y).normalized;

//            // �����x�������������x�̍X�V
//            currentSpeed += moveSpeedAcc * Time.deltaTime;

//            // ���x���ő呬�x�Ő���
//            currentSpeed = Mathf.Min(currentSpeed, maxSpeed);

//            // Rigidbody �ɂ��ړ�
//            Vector3 velocity = move * currentSpeed;
//            velocity.y = networkRb.Rigidbody.velocity.y;  // Y���̑��x�͕ύX���Ȃ�

//            // NetworkRigidbody ���g�p���đ��x���X�V
//            networkRb.Rigidbody.velocity = velocity;

//            // �����̕ύX����
//            if (moveInput.x < 0.0f)
//            {
//                gameObject.transform.localScale = new Vector3(initScale.x * -1, initScale.y, initScale.z);
//            }
//            else if (moveInput.x > 0.0f)
//            {
//                gameObject.transform.localScale = initScale;
//            }
//        }
//    }

//    public void Exit()
//    {
//        // �ړ���Ԃ𔲂���Ƃ��̏���
//    }
//}
