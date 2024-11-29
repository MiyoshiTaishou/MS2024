using Fusion;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeBossAction : NetworkBehaviour
{
    [SerializeField, Header("�{�X")]
    private GameObject boss;
    
    private int BossPattern;

    [SerializeField, Header("�ύX����s��")]
    private BossActionSequence bossAction;

    [SerializeField, Header("�؂�ւ��e�L�X�g")]
    private GameObject TextPanel1;
    [SerializeField] private GameObject TextPanel2;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            //�s������ւ�
            BossActionSequence data = boss.GetComponent<BossAI>().actionSequence[0];

            boss.GetComponent<BossAI>().actionSequence[0] = bossAction;

            bossAction = data;

            switch(BossPattern)
            {
                case 0:
                    TextPanel1.SetActive(false);
                    TextPanel2.SetActive(true);
                    BossPattern = 1;
                    break;

                case 1:
                    TextPanel2.SetActive(false);
                    TextPanel1.SetActive(true);
                    BossPattern = 0;
                    break;
            }
        }
    }

    private void Update()
    {
        
        if(boss.GetComponent<BossStatus>().nBossHP<boss.GetComponent<BossStatus>().InitHP / 2)
        {
            TextPanel1.SetActive(false);
            TextPanel2.SetActive(true);
            BossPattern = 0;
        }

    }
}
