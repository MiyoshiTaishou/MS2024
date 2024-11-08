using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeBossAction : NetworkBehaviour
{
    [SerializeField, Header("ボス")]
    private GameObject boss;

    [SerializeField, Header("変更する行動")]
    private BossActionSequence bossAction;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            //行動入れ替え
            BossActionSequence data = boss.GetComponent<BossAI>().actionSequence[0];

            boss.GetComponent<BossAI>().actionSequence[0] = bossAction;

            bossAction = data;
        }
    }
}
