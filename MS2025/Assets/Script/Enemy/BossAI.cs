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

    private int currentActionIndex = 0;
    private BossActionData currentAction;
    private bool isActionInitialized = false;
    private Animator animator;
    private bool isOnce = false;
    private bool isHalf = false;
    private int isParticle = 1;
    private Vector3 scale;

    [SerializeField, Header("ノックバックのアニメーション名")]
    private string animName;



    // プレイヤーターゲット用
    private List<Transform> players;
    [Networked] private int currentPlayerIndex { get; set; }
    [Networked] private int currentSequenceIndex { get; set; }
    [Networked, SerializeField] private int maxPlayerIndex { get; set; }
    [Networked, SerializeField] public bool isInterrupted { get; set; }
    [Networked, SerializeField] public bool isDown { get; set; }
    [Networked, SerializeField] public bool isAir { get; set; }

    [SerializeField,Header("ダウン時の行動データ")]
    public BossActionData downAction;

    [SerializeField, Header("のけぞり時の行動データ")]
    public BossActionData parryction;
    [Tooltip("ダウン時エフェクト")]

    public ParticleSystem Dawnparticle;

    private ParticleSystem newParticle;

    // アニメーション名をネットワーク同期させる
    [Networked]
    private NetworkString<_16> networkedAnimationName { get; set; }

    public override void Spawned()
    {
        animator = GetComponent<Animator>(); // Animator コンポーネントを取得
        currentSequenceIndex = Random.Range(0, actionSequence.Length);

        // プレイヤーオブジェクトをすべて取得してリストに保存
        players = new List<Transform>();
        RefreshPlayerList();

        scale = transform.localScale;

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

        if (currentAction.ExecuteAction(gameObject))
        {
            StartNextAction(); // アクション完了後に次のアクションに進む
        }      

        //向き変更処理
        if(GetComponent<Rigidbody>().velocity.x < -0.5)
        {
            transform.localScale = scale;
        }
        else if (GetComponent<Rigidbody>().velocity.x > 0.5)
        {
            Vector3 temp = scale;
            temp.x = -scale.x;
            transform.localScale = temp;
        }
        
        //50%以下で行動変更
        if (!isHalf && GetComponent<BossStatus>().nBossHP < GetComponent<BossStatus>().InitHP / 2)
        {
            isHalf = true;
            actionSequence = actionSequenceHalf;
            currentActionIndex = 0;
            StartNextAction(); // プレイヤーが二人以上揃っていたらアクションを開始            
        }
    }

    private void HandleInterruption()
    {
        currentAction = parryction;
        currentActionIndex = 0;
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
        }

        isActionInitialized = true;
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
        
        if(isParticle==2||isParticle==3)
        {
            if(isParticle==2)
            {
                // パーティクルシステムのインスタンスを生成
                newParticle = Instantiate(Dawnparticle);

                //パーティクルを生成
                newParticle.transform.position = this.transform.position;
                // パーティクルを発生させる
                newParticle.Play();
                isParticle = 3;
            }
            
            if (isDown==false)
            {
                // インスタンス化したパーティクルシステムのGameObjectを削除
                Destroy(newParticle.gameObject, 0.01f);

                isParticle = 1;
            }
        }

    }

    [Rpc(RpcSources.All, RpcTargets.All)]
    public void RPC_AnimName()
    {
        isInterrupted = true;
    }
}
