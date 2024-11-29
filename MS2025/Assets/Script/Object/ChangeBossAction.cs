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

    private int Count;//�J��nBossHP���Q�Ƃ����Spawn�O�ŃG���[���o��̂ŁA�����҂����邽�߂̃J�E���g

    [SerializeField, Header("�ύX����s��")]
    private BossActionSequence bossAction;

    [SerializeField, Header("�؂�ւ��e�L�X�g")]
    private GameObject TextPanel1;
    [SerializeField] private GameObject TextPanel2;

    [SerializeField, Header("�摜�؂�ւ�")]
    private Sprite imageBase;
    [SerializeField]private Sprite imageBoss;
    
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
                    GameObject.Find("BossIcon").GetComponent<SpriteRenderer>().sprite = imageBase;
                    BossPattern = 1;
                    break;

                case 1:
                    TextPanel2.SetActive(false);
                    TextPanel1.SetActive(true);
                    GameObject.Find("BossIcon").GetComponent<SpriteRenderer>().sprite = imageBoss;
                    BossPattern = 0;
                    break;
            }
        }
    }

    private void HPCheck()
    {
        if (boss.GetComponent<BossStatus>().nBossHP < boss.GetComponent<BossStatus>().InitHP / 2 && Count > 1)
        {
            TextPanel1.SetActive(false);
            TextPanel2.SetActive(true);
            BossPattern = 0;
        }
    }

    private void Update()
    {
        if(Count>=10)
        {
            HPCheck();
        }
        else
        {
            Count++;
        }

    }
}
