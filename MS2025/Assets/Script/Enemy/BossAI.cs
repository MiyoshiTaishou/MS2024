using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using UnityEngine.UIElements;

public class BossAI : NetworkBehaviour
{
    [Header("通常行動")]
    public BossActionSequence[] actionSequence;

    [Header("50%以下の行動")]
    public BossActionSequence[] actionSequenceHalf;

    //ノックバックする状態か
    [Networked] public bool isKnockBack { get; set; }

    //パリィ可能な状態か
    [Networked] public bool isParry { get; set; }
    [Networked] public int Nokezori { get; set; }
    [SerializeField, Tooltip("パリィ連携攻撃でのけぞる回数")] public int NokezoriLimit;
    private int currentActionIndex = 0;
    private BossActionData currentAction;
    public BossActionData GetCurrentAction() { return currentAction; }
    private bool isActionInitialized = false;
    private Animator animator;
    private bool isOnce = false;
    private bool isHalf = false;
    private int isParticle = 1;//ダウンパーティクルを出したり消したりするためのint変数
    private int isAttack = 0;//攻撃予兆エフェクトを出すタイミングを計るためのint変数 
    private Vector3 scale;

    [SerializeField, Header("ノックバックのアニメーション名")]
    private string animName;

    // プレイヤーターゲット用
    public List<Transform> players;
    [Networked] public int currentPlayerIndex { get; set; }
    [Networked] private int currentSequenceIndex { get; set; }
    [Networked, SerializeField] private int maxPlayerIndex { get; set; }
    [Networked, SerializeField] public bool isInterrupted { get; set; }/*これを呼ぶ*/
    [Networked, SerializeField] public bool isDown { get; set; }
    [Networked, SerializeField] public bool isAir { get; set; }

    [SerializeField,Header("ダウン時の行動データ")]
    public BossActionData downAction;

    [SerializeField, Header("のけぞり時の行動データ")]
    public BossActionData parryction;
    [Tooltip("ダウン時エフェクト")]
    private ParticleSystem Dawnparticle;

    [SerializeField, Header("攻撃の予兆に関する項目")]
    [Tooltip("攻撃予兆エフェクト")]
     private ParticleSystem AttackOmenParticle;
    [Tooltip("攻撃予兆エフェクトを出すまでの時間(0.3fが丁度いい気がします)")]
    private float Omentime = 0.3f;
    [Tooltip("攻撃予兆エフェクトのX座標")]
    private float OmenPosX = 1.5f;
    [Tooltip("攻撃予兆エフェクトのY座標")]
    private float OmenPosY = 1.7f;

    private ParticleSystem newParticle;

    private GameManager gameManager;

    private ShareNumbers shareNumbers;

    // アニメーション名をネットワーク同期させる
    [Networked]
    private NetworkString<_16> networkedAnimationName { get; set; }

    public override void Spawned()
    {
        animator = GetComponent<Animator>(); // Animator コンポーネントを取得
        currentSequenceIndex = Random.Range(0, actionSequence.Length);

        Nokezori = 0;
        // プレイヤーオブジェクトをすべて取得してリストに保存
        players = new List<Transform>();
        RefreshPlayerList();

        scale = transform.localScale;

        //ゲームマネージャー検索
        gameManager = GameObject.FindObjectOfType<GameManager>();

        if(!gameManager)
        {
            Debug.LogWarning("見つかりませんでした");
        }

        //シェアナンバー検索
        shareNumbers = GameObject.FindObjectOfType<ShareNumbers>();

        if (!shareNumbers)
        {
            Debug.LogWarning("見つかりませんでした");
        }

        if (players.Count < maxPlayerIndex)
        {
            Debug.Log("Waiting for more players...");
        }
        else
        {
            StartNextAction(); // プレイヤーが二人以上揃っていたらアクションを開始
        }
    }

    public override void FixedUpdateNetwork()
    {
        // プレイヤーが二人以上いない場合、行動を開始せず探索を続ける
        if (players.Count < maxPlayerIndex)
        {
            SearchForPlayers(); // 探索中の動作をここに実装
            return;
        }

        //ゲーム開始してなかったら動かさない
        if(!gameManager.GetBattleActive())
        {
            return;
        }

        //必殺技中は動かない
        if(shareNumbers.isSpecial)
        {
            return;
        }

        if (isInterrupted)
        {
            HandleInterruption();
            return;
        }

        if (isDown && !isOnce)
        {
            Debug.Log("ダウン");
            currentAction = downAction;
            currentActionIndex = 0;
            isActionInitialized = false;
            isOnce = true;
            isParticle = 2;
            return;
        }

        if (currentAction == null) return;

        //押されても動かないようにする為の処理
        GetComponent<Rigidbody>().velocity = new Vector3(0, GetComponent<Rigidbody>().velocity.y, 0);

        if (!isActionInitialized)
        {
            RPC_InitAction();
        }

        if (currentAction.ExecuteAction(gameObject, players[currentPlayerIndex]))
        {
            StartNextAction(); // アクション完了後に次のアクションに進む
        }           
        
        //50%以下で行動変更
        if (!isHalf && GetComponent<BossStatus>().nBossHP < GetComponent<BossStatus>().InitHP / 2)
        {
            isHalf = true;
            actionSequence = actionSequenceHalf;
            currentActionIndex = 0;
            currentSequenceIndex = 0;
            StartNextAction(); // プレイヤーが二人以上揃っていたらアクションを開始            
        }
    }

    private void HandleInterruption()
    {
        Debug.Log("ぱられたあああああああ");
        currentAction = parryction;
        currentActionIndex = 0;
        currentSequenceIndex = 0;
        isActionInitialized = false;
        isInterrupted = false;
    }

    private IEnumerator WaitAndStartNextAction(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        isInterrupted = false;
        StartNextAction();
    }

    // プレイヤーが二人以上揃うまで探索を続けるためのメソッド
    private void SearchForPlayers()
    {
        // プレイヤーリストを再確認する
        RefreshPlayerList();

        if (players.Count >= maxPlayerIndex)
        {
            Debug.Log("Players are now available. Starting actions.");
            StartNextAction(); // プレイヤーが揃ったらアクションを開始
        }
        else
        {
            Debug.Log("Searching for players...");
        }
    }

    // プレイヤーリストを更新するメソッド
    private void RefreshPlayerList()
    {
        players.Clear();
        foreach (var playerObj in GameObject.FindGameObjectsWithTag("Player"))
        {
            players.Add(playerObj.transform);
        }
    }

    [Rpc(RpcSources.All, RpcTargets.All)]
    public void RPC_InitAction()
    {
        // 現在のターゲットプレイヤーの参照をアクションに設定
        currentAction.InitializeAction(gameObject, players[currentPlayerIndex]); // ターゲットプレイヤーを渡す

        // アクションに対応するアニメーションをホスト側で再生
        if (Object.HasStateAuthority && animator != null && !string.IsNullOrEmpty(currentAction.actionName))
        {
            Debug.Log($"Playing animation: {currentAction.actionName}");
            networkedAnimationName = currentAction.actionName; // ネットワーク変数にアニメーション名をセット
            if(networkedAnimationName!="Attack")
            {
                //Attack以外ならまた攻撃時パーティクルが出るように設定する
                isAttack = 0;
            }
        }

        //次のアニメーションが攻撃モーションならパーティクルを出す
        if(networkedAnimationName== "Attack")
        {
            Invoke("Omen", Omentime);
        }

        isActionInitialized = true;
    }

    void Omen()
    {
        isAttack = 1;
    }

    void StartNextAction()
    {
        if (players == null || players.Count < maxPlayerIndex)
        {
            Debug.LogError("Not enough players available!");
            return;
        }

        if (isDown)
        {
            Debug.Log("ダウン完了");
            currentActionIndex = 0;
            currentSequenceIndex = Random.Range(0, actionSequence.Length);
            currentAction = downAction; // ダウンアクションを設定
            isActionInitialized = false;

            isDown = false; // ダウン状態を解除
            isOnce = false; // フラグをリセット
        

            return;
        }

        // 通常のアクションシーケンスの処理
        if (currentActionIndex >= actionSequence[currentSequenceIndex].actions.Length)
        {
            Debug.Log("All actions completed");
            currentActionIndex = 0;
            currentSequenceIndex = Random.Range(0, actionSequence.Length);
        }

        currentAction = actionSequence[currentSequenceIndex].actions[currentActionIndex];
        isActionInitialized = false;
        currentActionIndex++;

        Debug.Log($"Starting Action: {currentAction.name}");
        currentPlayerIndex = (currentPlayerIndex + 1) % players.Count;
    }


    public override void Render()
    {
        // クライアント側でもアニメーションを再生（ネットワーク変数が変わったときに実行）
        if (animator != null && !string.IsNullOrEmpty((string)networkedAnimationName) && animator.GetCurrentAnimatorStateInfo(0).IsName((string)networkedAnimationName) == false)
        {
            Debug.Log($"Synchronizing animation: {networkedAnimationName}");
            animator.Play((string)networkedAnimationName);
               
        }

        //向き変更処理
        if (GetComponent<Rigidbody>().velocity.x < -0.5)
        {
            transform.localScale = scale;
        }
        else if (GetComponent<Rigidbody>().velocity.x > 0.5)
        {
            Vector3 temp = scale;
            temp.x = -scale.x;
            transform.localScale = temp;
        }

        if (isParticle==2||isParticle==3)
        {
            switch(isParticle)
            {
                case 2:
                    // パーティクルシステムのインスタンスを生成
                    newParticle = Instantiate(Dawnparticle);

                    //パーティクルを生成
                    newParticle.transform.position = this.transform.position;
                    // パーティクルを発生させる
                    newParticle.Play();
                    isParticle = 3;
                    break;
            }
            
        }

        //ダウン状態が解除されたらダウンパーティクルを削除する
        //if (!isDown)
        //{
        //    // インスタンス化したパーティクルシステムのGameObjectを削除
        //    Destroy(newParticle.gameObject, 0.01f);

        //    isParticle = 1;
        //}

        switch(isAttack)
        {
            case 1:
                // パーティクルシステムのインスタンスを生成
                ParticleSystem OmenParticle = Instantiate(AttackOmenParticle);

                if (this.transform.localScale.x > 0)
                {
                    //パーティクルを生成
                    OmenParticle.transform.position = new Vector3(this.transform.position.x + OmenPosX, this.transform.position.y + OmenPosY, this.transform.position.z - 0.8f);
                    // パーティクルを発生させる
                    OmenParticle.Play();

                    Destroy(OmenParticle.gameObject, 0.8f);
                }
                else
                {
                    //パーティクルを生成
                    OmenParticle.transform.position = new Vector3(this.transform.position.x - OmenPosX, this.transform.position.y + OmenPosY, this.transform.position.z - 0.8f);
                    // パーティクルを発生させる
                    OmenParticle.Play();

                    Destroy(OmenParticle.gameObject, 0.8f);
                }

                isAttack = 2;
                break;

                case 2:
                break;
        }
    }

    [Rpc(RpcSources.All, RpcTargets.All)]
    public void RPC_AnimName()
    {
        Nokezori = NokezoriLimit;
        isInterrupted = true;
    }
}
