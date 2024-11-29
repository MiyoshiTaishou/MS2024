using Fusion;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeBossAction : NetworkBehaviour
{
    [SerializeField, Header("ボス")]
    private GameObject boss;
    
    private int BossPattern;

    private int Count;//開幕nBossHPを参照するとSpawn前でエラーが出るので、少し待たせるためのカウント

    [SerializeField, Header("変更する行動")]
    private BossActionSequence bossAction;

    [SerializeField, Header("切り替えテキスト")]
    private GameObject TextPanel1;
    [SerializeField] private GameObject TextPanel2;

    [SerializeField, Header("画像切り替え")]
    private Sprite imageBase;
    [SerializeField]private Sprite imageBoss;
    
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            //行動入れ替え
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
