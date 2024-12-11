using Fusion;
using UnityEngine;

[CreateAssetMenu(fileName = "MoveRoundAction", menuName = "Boss/Actions/MoveRound")]
public class BossMoveRoundAction : BossActionData
{
    public float moveSpeed; // 移動速度
    public float range; // ランダム移動範囲

    public Vector3 minBounds; // 移動範囲の最小座標
    public Vector3 maxBounds; // 移動範囲の最大座標

    private Vector3 targetPosition; // 次の移動先
    private float elapsedTime; // 経過時間
    public float moveTime = 10.0f; // 行動の実行時間

    public override void InitializeAction(GameObject boss, Transform player)
    {
        // 経過時間をリセット
        elapsedTime = 0f;

        // 最初のランダムな目標地点を設定
        targetPosition = GetRandomPosition(boss.transform.position);
    }

    public override bool ExecuteAction(GameObject boss, Transform player)
    {
        // 経過時間を更新
        elapsedTime += Time.deltaTime;

        // ボスを目標地点に移動させる
        boss.transform.position = Vector3.MoveTowards(boss.transform.position, targetPosition, moveSpeed * Time.deltaTime);

        // 範囲制限を適用
        boss.transform.position = ApplyBounds(boss.transform.position);

        // 目標地点に到達した場合、新しい目標地点を設定
        if (Vector3.Distance(boss.transform.position, targetPosition) < 0.1f)
        {
            targetPosition = GetRandomPosition(boss.transform.position);
        }

        // 次の行動に移るかどうかの判定
        if (elapsedTime >= moveTime)
        {
            return true; // アクション完了
        }

        return false; // 実行中
    }

    private Vector3 GetRandomPosition(Vector3 currentPosition)
    {
        // ランダムなオフセットを計算
        float randomX = Random.Range(-range, range);
        float randomZ = Random.Range(-range, range);

        // 新しい目標地点を設定し、範囲内に収める
        Vector3 newPosition = new Vector3(currentPosition.x + randomX, currentPosition.y, currentPosition.z + randomZ);
        return ApplyBounds(newPosition);
    }

    private Vector3 ApplyBounds(Vector3 position)
    {
        // 範囲制限を適用し、座標を修正
        if (position.x < minBounds.x)
        {
            position.x = maxBounds.x;
        }
        else if (position.x > maxBounds.x)
        {
            position.x = minBounds.x;
        }

        if (position.z < minBounds.z)
        {
            position.z = maxBounds.z;
        }
        else if (position.z > maxBounds.z)
        {
            position.z = minBounds.z;
        }

        return position;
    }
}
