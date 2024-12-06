using UnityEngine;

[CreateAssetMenu(fileName = "AttackAction", menuName = "Boss/Actions/Attack")]
public class AttackAction : BossActionData
{
    [SerializeField,Header("攻撃が届く範囲別になくてもいいかも")]
    public float attackRange;

    [SerializeField, Header("攻撃開始するまでの待機時間　アニメーションの速度が1なら1.4")]
    private float attackDuration;

    [SerializeField, Header("アニメーションの速度　通常が2")]
    private float attackAnimSpeed;

    [SerializeField, Header("攻撃範囲のアニメーションの速度　通常が1")]
    private float attackAreaAnimSpeed;

    [SerializeField, Header("攻撃の当たり判定の大きさ")]
    public Vector3 attackScale;

    [SerializeField, Header("カメラを揺らす処理を適用するかどうか")]
    public bool isCameraShake;

    [SerializeField, Header("何時まで揺らすか")]
    private float cameraDuration;

    [SerializeField, Header("揺れの強さ")]
    private float magnitude;

    [SerializeField, Header("パリィ不可能な攻撃かどうか")]
    private bool canParry;

    [SerializeField, Header("ノックバック可能な攻撃かどうか")]
    private bool canKnockBack;

    public string attackAreaName; // 攻撃エリアの名前（ボスの子オブジェクトの名前）
    private GameObject attackArea; // 既存の攻撃エリアの参照
    private GameObject attackAreaView; // 既存の攻撃エリアの参照

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
        attackAreaView = boss.transform.Find("Area")?.gameObject;
        attackArea.transform.localScale = attackScale;
        boss.GetComponent<Animator>().speed = attackAnimSpeed;

        // 攻撃の判定の強さを決める
        boss.GetComponent<BossAI>().isKnockBack = canKnockBack;
        boss.GetComponent<BossAI>().isParry = canParry;

        // 攻撃エリアをプレイヤー方向に配置
        Vector3 directionToPlayer = (attackTarget.position - boss.transform.position).normalized; // プレイヤーへの方向を正規化
        Vector3 attackPosition = boss.transform.position + directionToPlayer * attackRange;      // 攻撃エリアの新しい位置
        attackArea.transform.position = attackPosition;
        attackAreaView.transform.position = new Vector3(attackPosition.x, 2f, attackPosition.z);
        attackAreaView.GetComponent<PulsatingCircle>().SetMaxScale(attackScale.x);
        attackAreaView.GetComponent<PulsatingCircle>().SetSpeed(attackAnimSpeed);
    }


    public override bool ExecuteAction(GameObject boss, Transform player)
    {       
        // 攻撃開始までの時間を待機
        if (Time.time - attackStartTime < attackDuration)
        {
            // 攻撃待機中に何かしらの動作をしたい場合（例：アニメーションなど）、ここに処理を入れることができます
            attackAreaView.SetActive(true);
            return false; // まだ実行中
        }
        
        // プレイヤーが攻撃範囲内かどうかを確認
        if (Vector3.Distance(boss.transform.position, attackTarget.position) <= attackRange)
        {
            // プレイヤーが攻撃範囲内なら攻撃
            Debug.Log("攻撃");
            attackArea.SetActive(true);
            attackAreaView.SetActive(false);
        }
        else
        {
            // 範囲外でも空振りの攻撃を行う
            Debug.Log("空振り");
            attackArea.SetActive(true);
            attackAreaView.SetActive(false);
        }

        // 音を再生
        if (boss.GetComponent<AudioSource>() != null && attackClip != null)
        {
            boss.GetComponent<AudioSource>().clip = attackClip;
            boss.GetComponent<AudioSource>().Play();
        }

        //カメラを揺らす処理
        if(isCameraShake)
        {
            boss.GetComponent<HitStop>().ApplyHitStop(30);
            Camera.main.GetComponent<CameraShake>().RPC_CameraShake(cameraDuration, magnitude);           
        }

        boss.GetComponent<Animator>().speed = 1;            

        return true; // アクション完了
    }
}
