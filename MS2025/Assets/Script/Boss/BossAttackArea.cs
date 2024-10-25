using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttackArea : NetworkBehaviour
{
    GameObject box;
    GameObject parent;

    public override void Spawned()
    {
        box = GameObject.Find("Networkbox");
        parent = transform.parent.gameObject;
    }
  
    private void OnTriggerEnter(Collider other)
    {       
        if (other.CompareTag("Player"))
        {
            if (other.GetComponent<PlayerParryNet>().ParryCheck())
            {
                Debug.Log("パリィ成功");
                other.GetComponent<PlayerParryNet>().RPC_ParrySystem();
                gameObject.SetActive(false);

                return;
            }

            Debug.Log("攻撃ヒット");
            box.GetComponent<ShareNumbers>().RPC_Damage();
            other.GetComponent<PlayerHP>().RPC_DamageAnim();
            //parent.GetComponent<BossAI>().RPC_AnimName();

            gameObject.SetActive(false);
        }        
    }
}
