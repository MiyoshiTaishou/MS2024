using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToBossObject : NetworkBehaviour
{
    [SerializeField, Header("�Ώۂ̃I�u�W�F�N�g")]
    private GameObject target;

    [SerializeField, Header("�ǂꂮ�炢����邩")]
    private Vector3 distance;

    //�Ǐ]���������s���邩
    private bool isToMove = true;

    public void SetToMove(bool _isToMove) { isToMove = _isToMove; }

    private Animator animator;

    public override void Spawned()
    {
        animator = GetComponent<Animator>();
    }

    public override void FixedUpdateNetwork()
    {
        if (isToMove)
        {
            transform.position = target.transform.position + distance;
        }

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
    }
}
