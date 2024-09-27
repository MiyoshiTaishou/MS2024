using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// �v���C���[�X�e�[�g�Ǘ��N���X
/// </summary>
public class PlayerState : MonoBehaviour
{
    private IState currentState;
    [HideInInspector]public PlayerInput input;

    // �C���X�y�N�^�[�Œ����\�Ȉړ����x�Ɖ����x
    [HideInInspector] public float moveSpeed = 5.0f;
    [HideInInspector] public float moveSpeedAcc = 1.0f;
    [HideInInspector] public float maxSpeed = 10.0f;

    // Start is called before the first frame update
    void Start()
    {
        input = GetComponent<PlayerInput>();

        // ������Ԃ��ړ���ԂɃZ�b�g (���̏�Ԃɂ���ꍇ�͕ύX)
        currentState = new PlayerIdleState(this);
        currentState.Enter();        
    }

    // Update is called once per frame
    void Update()
    {
        //���݂̏�Ԃ�Update���������s
        currentState.Update();
        ChangeStateUpdate();
    }

    /// <summary>
    /// ��Ԃ̕ύX
    /// </summary>
    /// <param name="newState"></param>
    public void ChangeState(IState newState)
    {
        currentState.Exit();
        currentState = newState;
        currentState.Enter();
    }

    // �L�����N�^�[�̃A�j���[�V������ݒ肷�郁�\�b�h
    public void SetAnimation(string animationName)
    {
        // �A�j���[�V�����̃Z�b�g����
    }

    public void ChangeStateUpdate()
    {
        if(currentState is PlayerIdleState)
        {
            Vector2 moveInput = input.actions["Move"].ReadValue<Vector2>();
            if(moveInput != Vector2.zero)
            {
                ChangeState(new PlayerMoveState(this));
            }
        }

        if (currentState is PlayerMoveState)
        {
            Vector2 moveInput = input.actions["Move"].ReadValue<Vector2>();
            if (moveInput == Vector2.zero)
            {
                ChangeState(new PlayerIdleState(this));
            }
        }
    }
}
