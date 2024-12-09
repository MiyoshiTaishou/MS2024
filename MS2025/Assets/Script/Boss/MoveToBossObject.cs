using Fusion;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst.Intrinsics;
using UnityEngine;

public class MoveToBossObject : NetworkBehaviour
{
    [SerializeField, Header("対象のオブジェクト")]
    private GameObject target;

    [SerializeField,Networked]
    private Vector3 distance { get; set; }

    //追従処理を実行するか
    [Networked] private bool isToMove { get; set; }

    public void SetToMove(bool _isToMove) { isToMove = _isToMove; }

    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void RPC_SetToMove(bool _tomove)
    {
        SetToMove(_tomove);
    }

    private Animator animator;

    public override void Spawned()
    {
        animator = GetComponent<Animator>();
        isToMove = true;
    }

    public override void FixedUpdateNetwork()
    {       
        if(target.GetComponent<BossAI>().isInterrupted || target.GetComponent<BossAI>().isDown)
        {
            isToMove = true;
        }
    }

    public override void Render()
    {
        if(isToMove)
        {
            animator.SetTrigger("Hit");
        }
        else
        {
            animator.SetTrigger("Attack");
        }

        if (isToMove)
        {
            transform.position = target.transform.position + distance;
        }
    }
}
