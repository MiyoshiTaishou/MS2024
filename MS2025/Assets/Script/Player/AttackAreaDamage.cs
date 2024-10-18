using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackAreaDamage : NetworkBehaviour
{
    GameObject player;
    [SerializeField] GameObject netobj;
    PlayerAttack attack;
    ShareNumbers sharenum;
    public override void Spawned()
    {
        player = transform.parent.gameObject;
        attack = player.GetComponent<PlayerAttack>();
        netobj = GameObject.Find("Networkbox");
        if (netobj == null)
        {
            Debug.LogError("‚â‚Á‚Î‚¢‚Ë");
        }
        sharenum = netobj.GetComponent<ShareNumbers>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (Object.HasStateAuthority)
        {
            if (other.GetComponent<BossStatus>())
            {
                other.GetComponent<BossStatus>().RPC_Damage(10);
                sharenum.AddCombo();
                RPCCombo();
            }
        }      
    }
    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void RPCCombo()
    {
        attack.currentCombo = sharenum.nCombo;
    }
}
