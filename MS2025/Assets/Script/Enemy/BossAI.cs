using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class BossAI : NetworkBehaviour
{
    public BossActionSequence[] actionSequence;
    private int currentActionIndex = 0;
    private BossActionData currentAction;
    private bool isActionInitialized = false;
    private Animator animator;
    private bool isOnce = false;

    [SerializeField, Header("ノックバックのアニメーション名")]
    private string animName;

    [Tooltip("ダウン時エフェクト")]

    public ParticleSystem Dawnparticle;

    // プレイヤーターゲット用
    private List<Transform> players;
    [Networked] private int currentPlayerIndex { get; set; }
    [Networked] private int currentSequenceIndex { get; set; }
    [Networked, SerializeField] private int maxPlayerIndex { get; set; }
    [Networked, SerializeField] public bool isInterrupted { get; set; }
    [Networked, SerializeField] public bool isDown { get; set; }
    [Networked, SerializeField] public bool isAir { get; set; }

    public BossActionData downAction;

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
    }

    private void HandleInterruption()
    {
        // ノックバック処理
        networkedAnimationName = animName;

        // アニメーション再生中なら、まだ中断状態を解除しない
        Debug.Log(networkedAnimationName + "ノックバック");

        // アニメーションが再生されている間は中断状態を維持
        if (animator.GetCurrentAnimatorStateInfo(0).IsName(animName))
        {
            Debug.Log(networkedAnimationName + "再生中");
            return;
        }

        // アニメーションが終了したらフラグをリセットし、次のアクションを開始
        StartCoroutine(WaitAndStartNextAction(10f)); // 1秒待ってから次のアクションへ
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

            //パーティクル生成
            // パーティクルシステムのインスタンスを生成
            ParticleSystem newParticle = Instantiate(Dawnparticle);
            //最も近い場所にパーティクルを生成
            newParticle.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z);
            // パーティクルを発生させる
            newParticle.Play();
            // インスタンス化したパーティクルシステムのGameObjectを1秒後に削除
            Destroy(newParticle.gameObject, 1.0f);


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
    }

    [Rpc(RpcSources.All, RpcTargets.All)]
    public void RPC_AnimName()
    {
        isInterrupted = true;
    }
}
