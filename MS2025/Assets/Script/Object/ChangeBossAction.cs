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
    [SerializeField]
    private BossActionSequence jumpAction;

    [SerializeField, Header("切り替えテキスト")]
    private GameObject ParryInstrucionText;
    [SerializeField] private GameObject InstructionText;
    [SerializeField] private GameObject JumpInstrucionText;
    [SerializeField, Header("説明テキストボックス")]
    private GameObject TextBox;
    private int ActionNo=0;
    
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            //行動入れ替え
       
            BossActionSequence data = boss.GetComponent<BossAI>().actionSequence[0];

            boss.GetComponent<BossAI>().actionSequence[0] = bossAction;

            bossAction = jumpAction;

            jumpAction = data;

            switch(BossPattern)
            {
                case 0:
                    ParryInstrucionText.SetActive(true);
                    JumpInstrucionText.SetActive(false);
                    InstructionText.SetActive(false);
                    BossPattern = 1;
                    TextBox.GetComponent<TextMesh>().text = "協力ジャンプ";
                    break;

                case 1:
               
                    ParryInstrucionText.SetActive(false);
                    JumpInstrucionText.SetActive(true);
                    InstructionText.SetActive(false);
                    BossPattern = 2;
                    TextBox.GetComponent<TextMesh>().text = "基本操作説明";
                    break;

                case 2:
                   
                    ParryInstrucionText.SetActive(false);
                    JumpInstrucionText.SetActive(false);
                    InstructionText.SetActive(true);
                    BossPattern = 0;
                    TextBox.GetComponent<TextMesh>().text = "パリィ説明";
                    break;
            }
        }
    }

    private void HPCheck()
    {
        //if (boss.GetComponent<BossStatus>().nBossHP < boss.GetComponent<BossStatus>().InitHP / 2 && Count > 1)
        //{
        //    ParryInstrucionText.SetActive(false);
        //    InstructionText.SetActive(true);
        //    BossPattern = 0;
        //}
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
