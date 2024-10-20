using UnityEngine;

[CreateAssetMenu(fileName = "AttackAction", menuName = "Boss/Actions/Attack")]
public class AttackAction : BossActionData
{
    public float attackRange;
    public float attackDuration;   

    public AudioClip attackClip;
  
    private float attackStartTime;
    private Transform player;

    public override void InitializeAction(GameObject boss)
    {
        player = GameObject.FindWithTag("Player").transform;
        attackStartTime = Time.time;        
    }

    public override bool ExecuteAction(GameObject boss)
    {
        //プレイヤーが生成される前に実行されることがあるので無い場合はとりあえず行動を終わらせる
        if(player == null)
        {
            return true;
        }

        // プレイヤーが攻撃範囲内かどうかを判定
        if (Vector3.Distance(boss.transform.position, player.position) <= attackRange)
        {
            if (Time.time - attackStartTime >= attackDuration)
            {
                // 攻撃処理の完了（例：プレイヤーにダメージを与える）
                Debug.Log("Attacking Player!");
                boss.GetComponent<AudioSource>().clip = attackClip;
                boss.GetComponent<AudioSource>().Play();
                return true; // アクション完了
            }
        }
        else
        {
            Debug.Log("Player is out of range");
        }
        return false; // まだ実行中
    }
}
