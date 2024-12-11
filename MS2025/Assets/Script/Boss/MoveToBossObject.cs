using Fusion;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst.Intrinsics;
using UnityEngine;
using UnityEngine.UIElements;

public class MoveToBossObject : NetworkBehaviour
{
    [SerializeField, Header("�Ώۂ̃I�u�W�F�N�g")]
    private GameObject target;

    [SerializeField, Networked]
    private Vector3 distance { get; set; }

    // �Ǐ]���������s���邩
    [Networked] private bool isToMove { get; set; }

    //������ς��鏈��
    [Networked] private bool isDir { get; set; }

    // �O��̏�Ԃ�ێ�
    private bool previousIsToMove;

    private Vector3 scale;

    public void SetToMove(bool _isToMove)
    {
        isToMove = _isToMove;
    }

    public void SetDir(bool _isDir)
    {
        isDir = _isDir;
    }

    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void RPC_SetToMove(bool _toMove)
    {
        SetToMove(_toMove);
    }

    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void RPC_SetDir(bool _isDir)
    {
        SetDir(_isDir);
    }

    private Animator animator;

    public override void Spawned()
    {
        animator = GetComponent<Animator>();
        isToMove = true;
        previousIsToMove = isToMove;
        scale = transform.localScale;
    }

    public override void FixedUpdateNetwork()
    {
        if (target.GetComponent<BossAI>().isInterrupted || target.GetComponent<BossAI>().isDown)
        {
            isToMove = true;
        }
    }

    public override void Render()
    {
        // isToMove �̏�Ԃ��ω������ꍇ�̂݃A�j���[�V�������g���K�[
        if (previousIsToMove != isToMove)
        {
            if (isToMove)
            {
                animator.SetTrigger("Hit");
            }
            else
            {
                animator.SetTrigger("Attack");
            }

            // ��Ԃ̍X�V
            previousIsToMove = isToMove;
        }

        // �Ǐ]����
        if (isToMove)
        {
            transform.position = target.transform.position + distance;
            transform.localScale = scale;
            GetComponent<BoxCollider>().enabled = false;
        }
        else
        {
            //�����ύX����
            if (isDir)
            {
                transform.localScale = scale;
            }
            else
            {
                Vector3 temp = scale;
                temp.x = -scale.x;
                transform.localScale = temp;
            }
        }   
    }
}
