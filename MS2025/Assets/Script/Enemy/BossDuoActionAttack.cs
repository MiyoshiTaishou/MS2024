using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DuoAction", menuName = "Boss/Actions/DuoAttack")]
public class BossDuoActionAttack : BossActionData
{
    // 配列からリストに変更
    [SerializeField,Header("アクションリスト")]
    private List<BossActionData> actionDatas = new List<BossActionData>();

    public override void InitializeAction(GameObject boss, Transform player)
    {
        foreach (var actionData in actionDatas)
        {
            actionData.InitializeAction(boss, player);
        }
    }

    public override bool ExecuteAction(GameObject boss)
    {
        // 実行中に削除するため、一時リストに移動
        List<BossActionData> completedActions = new List<BossActionData>();

        foreach (var actionData in actionDatas)
        {
            if (actionData.ExecuteAction(boss))
            {
                Debug.Log(actionData + "アクション完了");
                // 完了したアクションを記録
                completedActions.Add(actionData);
            }
        }

        // 完了したアクションをリストから削除
        foreach (var completedAction in completedActions)
        {
            int num = 0;
            num++;

            Debug.Log(num + "アクション完了数");

            return num == completedActions.Count;
        }
       
        return false;
    }
}
