using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttackArea : NetworkBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (Object.HasStateAuthority)
        {
            if (other.GetComponent<PlayerHP>())
            {
                other.GetComponent<PlayerHP>().RPC_Damage(10);
            }
        }
    }
}
