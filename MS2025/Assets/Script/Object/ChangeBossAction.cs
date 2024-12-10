using Fusion;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;

public class ChangeBossAction : NetworkBehaviour
{
    [SerializeField, Header("�{�X")]
    private GameObject boss;

    private GameObject Player;

    private int BossPattern;

    private int Count;//�J��nBossHP���Q�Ƃ����Spawn�O�ŃG���[���o��̂ŁA�����҂����邽�߂̃J�E���g

    [SerializeField, Header("�ύX����s��")]
    private BossActionSequence bossAction;
    [SerializeField]
    private BossActionSequence jumpAction;

    [SerializeField, Header("�؂�ւ��e�L�X�g")]
       private GameObject InstructionText;
    [SerializeField] private GameObject TextSprite;
    [Networked] public int TextNo { get; set; }

    public Sprite[] numberSprites; // �X�v���C�g

    BossActionSequence data;

    [Networked] public int combo { get; set; }

    public override void Spawned()
    {

        data = boss.GetComponent<BossAI>().actionSequence[0];
    }

 

    public override void FixedUpdateNetwork()
    {

    }

    public override void Render()
    {


        switch (TextNo)
        {

            case 1://�K�E�Z���o���Ă��炤
                TextSprite.GetComponent<Image>().sprite = numberSprites[TextNo];
                break;
            case 2://���������U������K���Ă��炤
                TextSprite.GetComponent<Image>().sprite = numberSprites[TextNo];
                boss.GetComponent<BossAI>().actionSequence[0] = jumpAction;//�{�X�ɂЂ�����W�����v������
                break;
            case 3://�p���B����K
                TextSprite.GetComponent<Image>().sprite = numberSprites[TextNo];//�ڂ̑O�ɍU��
                boss.GetComponent<BossAI>().actionSequence[0] = bossAction;
                break;
            case 4://���ߍU��
                TextSprite.GetComponent<Image>().sprite = numberSprites[TextNo];
                boss.GetComponent<BossAI>().actionSequence[0] = data;//�������Ȃ��Ȃ�
                break;
            case 5://�I���
                TextSprite.GetComponent<Image>().sprite = numberSprites[TextNo];
                //���ɃV�[���J�ڏ���

                break;
        }

    }

    [Rpc(RpcSources.All, RpcTargets.All)]
    public void RPC_Cange()
    {
       
        if(combo>=1&&TextNo<1)
        {
            TextNo = 1;
        }
        
    }

   

    private void Update()
    {
     
    }
}


//Tach = Tach;

//if (Tach)
//{
//    //�s������ւ�

//BossActionSequence data = boss.GetComponent<BossAI>().actionSequence[0];

//boss.GetComponent<BossAI>().actionSequence[0] = bossAction;

//bossAction = jumpAction;

//jumpAction = data;

//    switch (BossPattern)
//    {
//        case 0:
//            ParryInstrucionText.SetActive(true);
//            JumpInstrucionText.SetActive(false);
//            InstructionText.SetActive(false);
//            BossPattern = 1;
//            TextBox.GetComponent<TextMesh>().text = "���̓W�����v";
//            break;

//        case 1:

//            ParryInstrucionText.SetActive(false);
//            JumpInstrucionText.SetActive(true);
//            InstructionText.SetActive(false);
//            BossPattern = 2;
//            TextBox.GetComponent<TextMesh>().text = "��{�������";
//            break;

//        case 2:

//            ParryInstrucionText.SetActive(false);
//            JumpInstrucionText.SetActive(false);
//            InstructionText.SetActive(true);
//            BossPattern = 0;
//            TextBox.GetComponent<TextMesh>().text = "�p���B����";
//            break;
//    }

//    Tach = false;

//}