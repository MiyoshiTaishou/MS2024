using UnityEngine;

[CreateAssetMenu(fileName = "JumpAction", menuName = "Boss/Actions/Jump")]
public class BossJumpAction : BossActionData
{
    public float jumpDuration;  // 空中で留まる時間
    public float jumpPower;     // ジャンプ力
    public float jumpHeight;    // ジャンプの高さ制限
    public AudioClip attackClip;

    private float jumpStartTime;
    private bool isJumping;     // ジャンプ中かどうか
    private bool atPeak;        // 最高到達点に達したかどうか
    private Vector3 startPos;

    private GameObject attackAreaView; // 既存の攻撃エリアの参照

    [SerializeField, Header("次の攻撃を入れる")]
    private AttackAction attack;

    [SerializeField, Header("アニメーションの速度　通常が2")]
    private float attackAnimSpeed;

    public override void InitializeAction(GameObject boss, Transform player)
    {
        // ジャンプ開始時刻を記録
        jumpStartTime = Time.time;
        isJumping = true;
        atPeak = false;

        // 開始地点の記録
        startPos = boss.transform.position;

        // ジャンプ力を計算
        float targetY = startPos.y + jumpHeight;

        boss.GetComponent<AudioSource>().clip = attackClip;
        boss.GetComponent<AudioSource>().Play();
        boss.GetComponent<BossAI>().isAir = true;

        attackAreaView = boss.transform.Find("Area")?.gameObject;

        // 攻撃エリアをプレイヤー方向に配置
        Vector3 directionToPlayer = (player.position - boss.transform.position).normalized; // プレイヤーへの方向を正規化
        Vector3 attackPosition = boss.transform.position + directionToPlayer * attack.attackRange;      // 攻撃エリアの新しい位置       
        attackAreaView.transform.position = new Vector3(attackPosition.x, 2f, attackPosition.z);
        attackAreaView.GetComponent<PulsatingCircle>().RPC_Scale(attack.attackScale.x);
        attackAreaView.GetComponent<PulsatingCircle>().RPC_Spedd(attackAnimSpeed);
        attackAreaView.GetComponent<PulsatingCircle>().RPC_Active(true);
    }

    public override bool ExecuteAction(GameObject boss, Transform player)
    {
        Vector3 position = boss.transform.position;

        // 攻撃エリアをプレイヤー方向に配置
        Vector3 directionToPlayer = (player.position - position).normalized; // プレイヤーへの方向を正規化
        Vector3 attackPosition = position + directionToPlayer * attack.attackRange;      // 攻撃エリアの新しい位置       
        attackAreaView.transform.position = new Vector3(attackPosition.x, 2f, attackPosition.z);
        attackAreaView.GetComponent<PulsatingCircle>().RPC_Scale(attack.attackScale.x);

        if (isJumping)
        {
            float currentHeight = position.y - startPos.y;

            if (!atPeak)
            {
                // 上昇中の処理
                position.y += jumpPower * Time.deltaTime;

                if (currentHeight >= jumpHeight)
                {
                    position.y = startPos.y + jumpHeight; // 高さを制限
                    atPeak = true;
                    jumpStartTime = Time.time; // 空中滞在時間の計測を開始
                }
            }
            else
            {
                // 空中滞在中の処理
                if (Time.time - jumpStartTime >= jumpDuration)
                {
                    isJumping = false; // 落下開始
                }
            }
        }
        else
        {
            // 落下中の処理
            position.y -= jumpPower * Time.deltaTime;

            if (position.y <= startPos.y)
            {
                position.y = startPos.y; // 地面で停止
                boss.GetComponent<BossAI>().isAir = false;
                return true; // ジャンプ完了
            }
        }

        // ボスの位置を更新
        boss.transform.position = position;

        return false;
    }
}
