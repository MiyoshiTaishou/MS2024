using Fusion;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeJumpAction : NetworkBehaviour
{
    [SerializeField, Header("�{�X")]
    private GameObject boss;
    
    private int BossPattern;

    private int Count;//�J��nBossHP���Q�Ƃ����Spawn�O�ŃG���[���o��̂ŁA�����҂����邽�߂̃J�E���g

    [SerializeField, Header("�ύX����s��")]
    private BossActionSequence jumpAction;
  

    [SerializeField, Header("�؂�ւ��e�L�X�g")]
    private GameObject TextPanel1;
    [SerializeField] private GameObject TextPanel2;
    [SerializeField] private GameObject TextPanel3;

    [SerializeField, Header("�����e�L�X�g�{�b�N�X")]
    private GameObject TextBox;

    
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            //�s������ւ�
            BossActionSequence data = boss.GetComponent<BossAI>().actionSequence[0];

            boss.GetComponent<BossAI>().actionSequence[0] = jumpAction;

            jumpAction = data;

            //
            if(boss.GetComponent<BossAI>().actionSequence[0].name== "BASTutorial1")
            {
                TextPanel1.SetActive(false);
                TextPanel2.SetActive(false);
                TextPanel3.SetActive(true);
                TextBox.GetComponent<TextMesh>().text = "�p���B����";
            }
            else if(boss.GetComponent<BossAI>().actionSequence[0].name == "BASTutorial2")
            {
                TextPanel1.SetActive(false);
                TextPanel2.SetActive(true);
                TextPanel3.SetActive(false);
                TextBox.GetComponent<TextMesh>().text = "��{�������";
            }
            else if(boss.GetComponent<BossAI>().actionSequence[0].name == "K_WaveAttack")
            {
                TextPanel1.SetActive(true);
                TextPanel2.SetActive(false);
                TextPanel3.SetActive(false);
                TextBox.GetComponent<TextMesh>().text = "��{�������";
            }

            //switch(BossPattern)
            //{
            //    case 0:
            //        TextPanel1.SetActive(false);
            //        TextPanel2.SetActive(true);
            //        BossPattern = 1;
            //        TextBox.GetComponent<TextMesh>().text = "��{�������";
            //        break;

            //    case 1:
            //        TextPanel2.SetActive(false);
            //        TextPanel1.SetActive(true);
            //        BossPattern = 0;
            //        TextBox.GetComponent<TextMesh>().text = "���̓W�����v";
            //        break;
            //}


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
