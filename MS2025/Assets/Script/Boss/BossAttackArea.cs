using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttackArea : NetworkBehaviour
{
    GameObject box;

    public override void Spawned()
    {
        box = GameObject.Find("Networkbox");
    }

    private void OnTriggerEnter(Collider other)
    {       
        if (other.CompareTag("Player"))
        {
            Debug.Log("çUåÇÉqÉbÉg");
            box.GetComponent<ShareNumbers>().RPC_Damage();
        }        
    }
}
