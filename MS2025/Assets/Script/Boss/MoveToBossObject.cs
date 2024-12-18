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
    [Networked] private bool isToSpecial { get; set; }

    //������ς��鏈��
    [Networked] private bool isDir { get; set; }

    // �O��̏�Ԃ�ێ�
    private bool previousIsToMove;

    [Networked, SerializeField]
    private bool initDir { get; set; }

    private Vector3 scale;

    [SerializeField,Networked]
    public PARRYTYPE TypeBoss { get; set; }

    [SerializeField, Networked]
    public bool Tanuki { get; set; }

    public void SetToMove(bool _isToMove)
    {
        isToMove = _isToMove;
    }

    public void SetToSpecial(bool _isToMove)
    {
        isToSpecial = _isToMove;
    }

    public void SetDir(bool _isDir)
    {
        isDir = _isDir;
    }

    [Rpc(RpcSources.All, RpcTargets.All)]
    public void RPC_SetToMove(bool _toMove)
    {
        SetToMove(_toMove);
    }

    [Rpc(RpcSources.All, RpcTargets.All)]
    public void RPC_SetToSpecial(bool _toMove)
    {
        SetToSpecial(_toMove);
    }

    [Rpc(RpcSources.All, RpcTargets.All)]
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
                isToSpecial = false;
            }
            else
            {
                if (isToSpecial)
                {
                    animator.SetTrigger("Special");
                }
                else
                {
                    animator.SetTrigger("Attack");
                }                
            }

            // ��Ԃ̍X�V
            previousIsToMove = isToMove;
        }

        // �Ǐ]����
        if (isToMove)
        {
            transform.position = target.transform.position + distance;
            transform.localScale = scale;
            transform.localRotation = Quaternion.identity;
            GetComponent<BoxCollider>().enabled = false;

            GetComponent<BossAttackArea2Boss>().Type = TypeBoss;            
            target.GetComponent<BossAI>().isDirCheck = true;
        }
        else
        {
            target.GetComponent<BossAI>().isDirCheck = false;

            if (initDir)
            {
                //�����ύX����
                if (!isDir)
                {
                    transform.localScale = scale;
                    Debug.Log("��������");
                }
                else
                {
                    Vector3 temp = scale;
                    temp.x = -scale.x;
                    transform.localScale = temp;
                    Debug.Log("��������");
                }
            }
            else
            {
                //�����ύX����
                if (!isDir)
                {
                    Vector3 temp = scale;
                    temp.x = -scale.x;
                    transform.localScale = temp;
                    Debug.Log("��������");
                }
                else
                {
                    transform.localScale = scale;
                    Debug.Log("��������");                  
                }
            }
        }   
    }
}
