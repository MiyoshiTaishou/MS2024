using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToBossObject : NetworkBehaviour
{
    [SerializeField, Header("対象のオブジェクト")]
    private GameObject target;

    [SerializeField, Header("どれぐらい離れるか")]
    private Vector3 distance;

    //追従処理を実行するか
    private bool isToMove = true;

    public void SetToMove(bool _isToMove) { isToMove = _isToMove; }

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
}
