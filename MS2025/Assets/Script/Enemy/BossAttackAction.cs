using UnityEngine;

[CreateAssetMenu(fileName = "AttackAction", menuName = "Boss/Actions/Attack")]
public class AttackAction : BossActionData
{
    public float attackRange;
    public float attackDuration;  // 攻撃を行うまでの待機時間
    public float attackAnimSpeed;  // アニメーションの速度

    public string attackAreaName; // 攻撃エリアの名前（ボスの子オブジェクトの名前）
    private GameObject attackArea; // 既存の攻撃エリアの参照

    public AudioClip attackClip;

    private float attackStartTime;   

    private Transform attackTarget;

    public override void InitializeAction(GameObject boss, Transform player)
    {
        attackTarget = player;
        attackStartTime = Time.time;

        Debug.Log(player);

        // ボスの子オブジェクトから攻撃エリアを取得
        attackArea = boss.transform.Find(attackAreaName)?.gameObject;
        boss.GetComponent<Animator>().speed = attackAnimSpeed;
    }

    public override bool ExecuteAction(GameObject boss)
    {       
        // 攻撃開始までの時間を待機
        if (Time.time - attackStartTime < attackDuration)
        {
            // 攻撃待機中に何かしらの動作をしたい場合（例：アニメーションなど）、ここに処理を入れることができます
            return false; // まだ実行中
        }
        
        // プレイヤーが攻撃範囲内かどうかを確認
        if (Vector3.Distance(boss.transform.position, attackTarget.position) <= attackRange)
        {
            // プレイヤーが攻撃範囲内なら攻撃
            Debug.Log("攻撃");
            attackArea.SetActive(true);
        }
        else
        {
            // 範囲外でも空振りの攻撃を行う
            Debug.Log("空振り");
            attackArea.SetActive(true);
        }

        // 音を再生
        if (boss.GetComponent<AudioSource>() != null && attackClip != null)
        {
            boss.GetComponent<AudioSource>().clip = attackClip;
            boss.GetComponent<AudioSource>().Play();
        }

        boss.GetComponent<Animator>().speed = 1;

        return true; // アクション完了
    }
}
