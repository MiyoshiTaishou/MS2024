using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttack : NetworkBehaviour
{    
    GameObject attackArea;
  
    [Networked, SerializeField]
    private float attackTimeSpace { get; set; }

    [Networked, SerializeField]
    private bool isAttack { get; set; }

    public override void Spawned()
    {
        attackArea = gameObject.transform.Find("AttackArea").gameObject;
        attackArea.SetActive(false);
    }

    public override void FixedUpdateNetwork()
    {
       if(attackTimeSpace > 5.0f)
        {
            isAttack = !isAttack;           
            attackTimeSpace = 0.0f;
        }

        attackTimeSpace += Runner.DeltaTime;
    }

    public override void Render()
    {
        if (attackTimeSpace > 5.0f)
        {           
            attackArea.SetActive(isAttack);           
        }
    }
}
