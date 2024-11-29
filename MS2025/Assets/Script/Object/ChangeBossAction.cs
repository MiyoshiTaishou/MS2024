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
    [SerializeField]
    private BossActionSequence jumpAction;

    [SerializeField, Header("�؂�ւ��e�L�X�g")]
    private GameObject ParryInstrucionText;
    [SerializeField] private GameObject InstructionText;
    [SerializeField] private GameObject JumpInstrucionText;
    [SerializeField, Header("�����e�L�X�g�{�b�N�X")]
    private GameObject TextBox;
    private int ActionNo=0;
    
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            //�s������ւ�
       
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
                    TextBox.GetComponent<TextMesh>().text = "���̓W�����v";
                    break;

                case 1:
               
                    ParryInstrucionText.SetActive(false);
                    JumpInstrucionText.SetActive(true);
                    InstructionText.SetActive(false);
                    BossPattern = 2;
                    TextBox.GetComponent<TextMesh>().text = "��{�������";
                    break;

                case 2:
                   
                    ParryInstrucionText.SetActive(false);
                    JumpInstrucionText.SetActive(false);
                    InstructionText.SetActive(true);
                    BossPattern = 0;
                    TextBox.GetComponent<TextMesh>().text = "�p���B����";
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
