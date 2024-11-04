using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeBossAction : NetworkBehaviour
{
    [SerializeField, Header("�{�X")]
    private GameObject boss;

    [SerializeField, Header("�ύX����s��")]
    private BossActionSequence bossAction;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            //�s������ւ�
            BossActionSequence data = boss.GetComponent<BossAI>().actionSequence[0];

            boss.GetComponent<BossAI>().actionSequence[0] = bossAction;

            bossAction = data;
        }
    }
}
