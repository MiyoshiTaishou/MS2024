using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackAreaDamage : NetworkBehaviour
{
    GameObject player;
    PlayerAttack attack;
    public override void Spawned()
    {
        player = transform.parent.gameObject;
        attack = player.GetComponent<PlayerAttack>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (Object.HasStateAuthority)
        {
            if (other.GetComponent<BossStatus>())
            {
                other.GetComponent<BossStatus>().RPC_Damage(10);
                attack.AddCombo();
            }
        }      
    }
}
