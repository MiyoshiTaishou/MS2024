using Fusion;
using UnityEngine;

[CreateAssetMenu(fileName = "MoveRoundAction", menuName = "Boss/Actions/MoveRound")]
public class BossMoveRoundAction : BossActionData
{
    public float moveSpeed; // 円周を回る速度
    public float radius; // 円の半径
    public float angularSpeed; // 角速度 (ラジアン毎秒)
    
    public Vector3 centerPoint; // 円の中心
    private float currentAngle; // 現在の角度
    private float elapsedTime; // 経過時間

    public float moveTime = 10.0f;

    public override void InitializeAction(GameObject boss, Transform player)
    {      
        // 現在の角度と経過時間をリセット
        currentAngle = 0f;
        elapsedTime = 0f;
    }

    public override bool ExecuteAction(GameObject boss, Transform player)
    {
        // 経過時間を更新
        elapsedTime += Time.deltaTime;

        // 時間経過に応じて角度を進める
        currentAngle += angularSpeed * Time.deltaTime;

        // 円周上の次の位置を計算
        float x = centerPoint.x + radius * Mathf.Cos(currentAngle);
        float z = centerPoint.z + radius * Mathf.Sin(currentAngle);

        Vector3 targetPosition = new Vector3(x, boss.transform.position.y, z);

        // ボスの位置を直接設定してきれいな円を描く
        boss.transform.position = Vector3.Lerp(boss.transform.position, targetPosition, moveSpeed * Time.deltaTime);

        // 次の行動に移るかどうかの判定（例: 5秒後に次の行動）
        if (elapsedTime >= moveTime)
        {
            return true; // アクション完了
        }

        return false; // 実行中
    }
}
