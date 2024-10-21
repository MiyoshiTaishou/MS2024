using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using WebSocketSharp;

public class BossAI : NetworkBehaviour
{
    public BossActionSequence actionSequence;
    private int currentActionIndex = 0;
    private BossActionData currentAction;
    private bool isActionInitialized = false;
    private Animator animator;

    // アニメーション名をネットワーク同期させる
    [Networked]
    private NetworkString<_16> networkedAnimationName { get; set; }

    public override void Spawned()
    {
        animator = GetComponent<Animator>(); // Animator コンポーネントを取得
        StartNextAction();
    }

    public override void FixedUpdateNetwork()
    {
        if (currentAction == null) return;

        if (!isActionInitialized)
        {
            RPC_InitAction();
        }

        if (currentAction.ExecuteAction(gameObject))
        {
            StartNextAction(); // アクション完了後に次のアクションに進む
        }
        //RPC_ExecuteAction();
    }

    [Rpc(RpcSources.All, RpcTargets.All)]
    public void RPC_InitAction()
    {
        currentAction.InitializeAction(gameObject); // アクションの初期化

        // アクションに対応するアニメーションをホスト側で再生
        if (Object.HasStateAuthority && animator != null && !string.IsNullOrEmpty(currentAction.actionName))
        {
            Debug.Log($"Playing animation: {currentAction.actionName}");
            networkedAnimationName = currentAction.actionName; // ネットワーク変数にアニメーション名をセット
        }

        isActionInitialized = true;
    }

    [Rpc(RpcSources.All, RpcTargets.All)]
    public void RPC_ExecuteAction()
    {
        if (currentAction.ExecuteAction(gameObject))
        {
            StartNextAction(); // アクション完了後に次のアクションに進む
        }
    }

    void StartNextAction()
    {
        if (currentActionIndex >= actionSequence.actions.Length)
        {
            Debug.Log("All actions completed");
            currentActionIndex = 0;
        }

        currentAction = actionSequence.actions[currentActionIndex];
        isActionInitialized = false;
        currentActionIndex++;
        Debug.Log($"Starting Action: {currentAction.name}");
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
