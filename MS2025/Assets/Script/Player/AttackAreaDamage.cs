using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackAreaDamage : NetworkBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (Object.HasStateAuthority)
        {
            if (other.GetComponent<BossStatus>())
            {
                other.GetComponent<BossStatus>().RPC_Damage(10);
            }
        }      
    }
}
