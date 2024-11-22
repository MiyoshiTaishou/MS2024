using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DuoAction", menuName = "Boss/Actions/DuoAttack")]
public class BossDuoActionAttack : BossActionData
{
    [SerializeField, Header("アクションリスト")]
    private List<BossActionData> actionDatas = new List<BossActionData>();

    // アクションの完了状態を追跡するリスト
    private HashSet<BossActionData> completedActions = new HashSet<BossActionData>();

    public override void InitializeAction(GameObject boss, Transform player)
    {
        // アクションを初期化し、完了状態をリセット
        completedActions.Clear();
        foreach (var actionData in actionDatas)
        {
            actionData.InitializeAction(boss, player);
        }
    }

    public override bool ExecuteAction(GameObject boss, Transform player)
    {
        bool allCompleted = true;

        foreach (var actionData in actionDatas)
        {
            // 完了していないアクションだけを実行
            if (!completedActions.Contains(actionData))
            {
                if (actionData.ExecuteAction(boss,player))
                {
                    Debug.Log(actionData + " アクション完了");
                    completedActions.Add(actionData); // 完了状態を記録
                }
                else
                {
                    allCompleted = false; // まだ未完了のアクションがある
                }
            }
        }

        // 全てのアクションが完了していれば true を返す
        return allCompleted;
    }
}
