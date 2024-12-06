using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public  enum PARRYTYPE
{
    ALL,
    TANUKI,
    KITUNE,
    DOUBLE,
};

public class BossAttackArea : NetworkBehaviour
{
    GameObject box;
    GameObject parent;

    [SerializeField]
    public float deactivateTime = 0.5f; // 攻撃エリアの非表示にするまでの時間

    [Networked]private float timer { get; set; }

    [Networked] private bool isAttack { get; set; }
    private ParticleSystem newParticle;
    [Tooltip("攻撃エフェクト")]
    public ParticleSystem AttackParticle;

    private GameObject Pare;

    // 元の位置を保持する
    private Vector3 originalPosition;

    [Networked] public  PARRYTYPE Type { get; set; }

    [Networked] public bool isTanuki { get; set; }
    [Networked] public bool isKitune { get; set; }

    [SerializeField, Header("チュートリアルモード")]
    private bool isTutorial = false;

    public override void Spawned()
    {
        box = GameObject.Find("Networkbox");
        parent = transform.parent.gameObject;
        timer = deactivateTime;
        Pare = transform.parent.gameObject;
        isTanuki= false;
        isKitune= false;
        // 元の位置を記録
        originalPosition = transform.position;
    }

    // SetActive(true)のたびに呼び出す
    public void OnEnable()
    {
        Debug.Log("攻撃エフェクト生成");
        isAttack = true;
        isTanuki = false;
        isKitune = false;
    }

    public override void Render()
    {
        if (isAttack)
        {
            // パーティクルシステムのインスタンスを生成
            newParticle = Instantiate(AttackParticle);
            // 攻撃方向に基づいて位置を設定
            if (Pare.transform.localScale.x >= 0)
            {
                newParticle.transform.position = new Vector3(transform.position.x - 4.0f, transform.position.y - 2.0f, transform.position.z);
            }
            else
            {
                newParticle.transform.position = new Vector3(transform.position.x + 4.0f, transform.position.y - 2.0f, transform.position.z);
            }

            // パーティクルを発生させる
            newParticle.Play();
            // インスタンス化したパーティクルシステムのGameObjectを1秒後に削除
            Destroy(newParticle.gameObject, 1f);
            isAttack = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //ここから
        if (other.CompareTag("Player"))
        {
            timer = deactivateTime;
            // パリィ不可攻撃かどうか
            if (!parent.GetComponent<BossAI>().isParry)
            {
                if (other.GetComponent<PlayerParryNet>().ParryCheck()&&
                    ((other.GetComponent<PlayerParryNet>().isTanuki&&Type==PARRYTYPE.TANUKI)||
                    (!other.GetComponent<PlayerParryNet>().isTanuki&&Type==PARRYTYPE.KITUNE)||
                     Type==PARRYTYPE.ALL))
                {
                    Debug.Log("パリィ成功");
                    other.GetComponent<PlayerParryNet>().RPC_ParrySystem();

                    // ノックバック可能かどうか
                    if (parent.GetComponent<BossAI>().isKnockBack)
                    {
                        parent.GetComponent<BossAI>().RPC_AnimName();
                    }

                    ResetToOriginalPosition(); // 元の位置に戻す
                    gameObject.SetActive(false);
                    return;
                }
                else if(other.GetComponent<PlayerParryNet>().ParryCheck() &&Type == PARRYTYPE.DOUBLE)
                {
                    if(other.GetComponent<PlayerParryNet>().isTanuki) 
                    {
                        isTanuki = true;
                    }
                    else if(other.GetComponent<PlayerParryNet>().isTanuki==false) 
                    {
                        isKitune = true;
                    }
                    if(isTanuki&&isKitune)
                    {
                        Debug.Log("パリィ成功");
                        other.GetComponent<PlayerParryNet>().RPC_ParrySystem();

                        // ノックバック可能かどうか
                        if (parent.GetComponent<BossAI>().isKnockBack)
                        {
                            parent.GetComponent<BossAI>().RPC_AnimName();
                        }

                        ResetToOriginalPosition(); // 元の位置に戻す
                        gameObject.SetActive(false);
                        return;
                    }
                }
            }
            //ここまでTriggerStay
            Debug.Log("攻撃ヒット");
            if (other.GetComponent<PlayerHP>().inbisibleFrame == 0)
            {
                if(!isTutorial)
                {
                    box.GetComponent<ShareNumbers>().CurrentHP--;
                    box.GetComponent<ShareNumbers>().RPC_Damage();
                }
                other.GetComponent<PlayerHP>().RPC_DamageAnim();
            }
            Render();
            ResetToOriginalPosition(); // 元の位置に戻す
            gameObject.SetActive(false);
        }
    }

    // 元の位置に戻すメソッド
    private void ResetToOriginalPosition()
    {
        transform.position = originalPosition;
    }

    public override void FixedUpdateNetwork()
    {
        // タイマーを減らし、一定時間後に非表示にする
        if (timer > 0)
        {
            timer -= Runner.DeltaTime;
            if (timer <= 0)
            {
                ResetToOriginalPosition(); // タイムアウト時にも元の位置に戻す
                gameObject.SetActive(false);
                timer = deactivateTime;
            }
        }
    }
}
