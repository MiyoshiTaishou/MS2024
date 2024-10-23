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

    // プレイヤーターゲット用
    private List<Transform> players;
    [Networked] private int currentPlayerIndex { get; set; }
    [Networked] private int currentSequenceIndex { get; set; }
    [Networked,SerializeField] private int maxPlayerIndex { get; set; }

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

        if (currentAction == null) return;

        if (!isActionInitialized)
        {
            RPC_InitAction();
        }

        if (currentAction.ExecuteAction(gameObject))
        {
            StartNextAction(); // アクション完了後に次のアクションに進む
        }
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
        // リストが空かどうかだけチェックして、必要なら更新する
        if (players == null || players.Count < maxPlayerIndex)
        {
            Debug.LogError("Not enough players available!");
            return;
        }

        if (currentActionIndex >= actionSequence[currentSequenceIndex].actions.Length)
        {
            Debug.Log("All actions completed");
            currentActionIndex = 0;
            currentSequenceIndex = Random.Range(0, actionSequence.Length);
        }

        // 次のアクションを設定
        currentAction = actionSequence[currentSequenceIndex].actions[currentActionIndex];
        isActionInitialized = false;
        currentActionIndex++;

        Debug.Log($"Starting Action: {currentAction.name}");

        // ターゲットを次のプレイヤーに変更
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
}
