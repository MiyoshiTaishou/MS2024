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

    [SerializeField, Header("ゲームマネージャー")]
    private GameManager gameManager;

    [Header("次のシーン名"),SerializeField]
    private string nextSceneName; // 遷移先のシーン名

    [SerializeField]
    private TransitionManager transitionManager;

    private NetworkRunner networkRunner;

    private bool isOnce = false;

    // シーン遷移が一度だけ実行されるようにするためのフラグ
    private bool hasTransitioned = false;

    [SerializeField] private CrushingGame crushingGame;

    public Sprite[] numberSprites; // スプライト

    BossActionSequence data;

    [Networked] public int combo { get; set; }

    public override void Spawned()
    {
        networkRunner = FindObjectOfType<NetworkRunner>();
        data = boss.GetComponent<BossAI>().actionSequence[0];
    }

 

    public override void FixedUpdateNetwork()
    {
        switch (TextNo)
        {

            case 5://終わり
                // シーン遷移の処理
                if (!isOnce)
                {
                    //if (crushingGame.IsAnimation()) return;
                    //transitionManager.TransitionStart();
                    //// クライアントに先にシーン遷移を指示
                    //gameManager.RPC_EndBattle(10, 5);
                    //StartCoroutine(Load());

                    //isOnce = true;
                }

                break;
        }
    }

    private void EndTutorial()
    {
        // シーン遷移が一度だけ行われるようにチェック
        if (hasTransitioned) return;
        if (crushingGame.IsAnimation()) return;

        transitionManager.TransitionStart();
        hasTransitioned = true; // シーン遷移フラグを設定
        StartCoroutine(Load());

        //if (Object.HasStateAuthority)
        //{
        //    RPC_ClientSceneTransition();
        //}
    }

    public override void Render()
    {


        switch (TextNo)
        {

            case 1://必殺技を出してもらう
                TextSprite.GetComponent<Image>().sprite = numberSprites[TextNo];
                break;
            case 2://かちあげ攻撃を練習してもらう
                TextSprite.GetComponent<Image>().sprite = numberSprites[TextNo];
                boss.GetComponent<BossAI>().actionSequence[0] = jumpAction;//ボスにひたすらジャンプさせる
                break;
            case 3://パリィを練習
                TextSprite.GetComponent<Image>().sprite = numberSprites[TextNo];//目の前に攻撃
                boss.GetComponent<BossAI>().actionSequence[0] = bossAction;
                break;
            case 4://溜め攻撃
                TextSprite.GetComponent<Image>().sprite = numberSprites[TextNo];
                boss.GetComponent<BossAI>().actionSequence[0] = data;//何もしなくなる
                break;
            case 5://終わり
                TextSprite.GetComponent<Image>().sprite = numberSprites[TextNo];
                crushingGame.StartAnimation();
                EndTutorial();
                // クライアントに先にシーン遷移を指示
                gameManager.RPC_EndBattle(10, 5);
                break;
        }

    }

    [Rpc(RpcSources.All, RpcTargets.All)]
    public void RPC_Cange()
    {
       
        if(combo>=1&&TextNo<1)
        {
            TextNo = 5;
        }
        
    }

    private IEnumerator Load()
    {
        yield return new WaitForSeconds(2f);
        networkRunner.LoadScene(nextSceneName);
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