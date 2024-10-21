using Fusion;
using UnityEngine;

[CreateAssetMenu(fileName = "MoveAction", menuName = "Boss/Actions/Move")]
public class BossMoveAction : BossActionData
{
    public float moveSpeed;
    public float stoppingDistance;   

    private Transform player;
    private Rigidbody bossRigidbody;

    public override void InitializeAction(GameObject boss)
    {
        // Rigidbody の参照を取得
        bossRigidbody = boss.GetComponent<Rigidbody>();
        player = GameObject.FindWithTag("Player").transform;       
    }

    public override bool ExecuteAction(GameObject boss)
    {
        if (player == null || bossRigidbody == null)
            return false;

        // プレイヤーまでの距離を計算
        Vector3 directionToPlayer = player.position - boss.transform.position;
        float distanceToPlayer = directionToPlayer.magnitude;

        // 指定距離まで近づいていたら、移動を終了
        if (distanceToPlayer <= stoppingDistance)
        {
            bossRigidbody.velocity = Vector3.zero; // ボスを停止させる
            return true; // アクション完了
        }

        // プレイヤーの方へ移動（正規化された方向ベクトルにスピードを掛ける）
        Vector3 moveDirection = directionToPlayer.normalized * moveSpeed;
        bossRigidbody.velocity = new Vector3(moveDirection.x, bossRigidbody.velocity.y, moveDirection.z); // Y軸の速度は維持

        return false; // まだ実行中
    }
}
