using Fusion;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;

public class ChangeBossAction : NetworkBehaviour
{
    [SerializeField, Header("ボス")]
    private GameObject boss;

    private GameObject Player;

    private int BossPattern;

    private int Count;//開幕nBossHPを参照するとSpawn前でエラーが出るので、少し待たせるためのカウント

    [SerializeField, Header("変更する行動")]
    private BossActionSequence bossAction;
    [SerializeField]
    private BossActionSequence jumpAction;

    [SerializeField, Header("切り替えテキスト")]
       private GameObject InstructionText;
    [SerializeField] private GameObject TextSprite;
    [Networked] public int TextNo { get; set; }

    public Sprite[] numberSprites; // スプライト

    BossActionSequence data;

    [Networked] public int combo { get; set; }

    public override void Spawned()
    {
        TextNo = 0;

        data = boss.GetComponent<BossAI>().actionSequence[0];
    }

 

    public override void FixedUpdateNetwork()
    {

    }

    public override void Render()
    {


        switch (TextNo)
        {

            case 1:
                TextSprite.GetComponent<Image>().sprite = numberSprites[TextNo];
                break;
            case 2:
                TextSprite.GetComponent<Image>().sprite = numberSprites[TextNo];
                boss.GetComponent<BossAI>().actionSequence[0] = jumpAction;
                break;
            case 3:
                TextSprite.GetComponent<Image>().sprite = numberSprites[TextNo];
                boss.GetComponent<BossAI>().actionSequence[0] = bossAction;
                break;
            case 4:
                TextSprite.GetComponent<Image>().sprite = numberSprites[TextNo];
                break;
            case 5:
                TextSprite.GetComponent<Image>().sprite = numberSprites[TextNo];
                boss.GetComponent<BossAI>().actionSequence[0] = data;
                break;
            case 6:
                TextSprite.GetComponent<Image>().sprite = numberSprites[TextNo];
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
//    //行動入れ替え

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
//            TextBox.GetComponent<TextMesh>().text = "協力ジャンプ";
//            break;

//        case 1:

//            ParryInstrucionText.SetActive(false);
//            JumpInstrucionText.SetActive(true);
//            InstructionText.SetActive(false);
//            BossPattern = 2;
//            TextBox.GetComponent<TextMesh>().text = "基本操作説明";
//            break;

//        case 2:

//            ParryInstrucionText.SetActive(false);
//            JumpInstrucionText.SetActive(false);
//            InstructionText.SetActive(true);
//            BossPattern = 0;
//            TextBox.GetComponent<TextMesh>().text = "パリィ説明";
//            break;
//    }

//    Tach = false;

//}