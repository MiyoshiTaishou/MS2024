using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;


public class BossAI : NetworkBehaviour 
{
    public BossActionSequence actionSequence;
    private int currentActionIndex = 0;
    private BossActionData currentAction;
    private bool isActionInitialized = false;

    public override void Spawned()
    {
        StartNextAction();
    }

    public override void FixedUpdateNetwork()
    {
        if (currentAction == null) return;

        if (!isActionInitialized)
        {
            currentAction.InitializeAction(gameObject); // アクションの初期化
            isActionInitialized = true;
        }

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
            currentAction = null; // 全てのアクションが完了
            currentActionIndex = 0;           
        }

        currentAction = actionSequence.actions[currentActionIndex];
        isActionInitialized = false;
        currentActionIndex++;
        Debug.Log($"Starting Action: {currentAction.name}");
    }
}
