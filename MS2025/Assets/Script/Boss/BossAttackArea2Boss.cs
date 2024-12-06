using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttackArea2Boss : NetworkBehaviour
{
    GameObject box;
    GameObject parent;

    [SerializeField]
    public float deactivateTime = 0.5f; // 攻撃エリアを無効化するまでの時間
    [SerializeField]
    private float returnDuration = 1.0f; // 元の位置に戻るのにかける時間

    [Networked] private float timer { get; set; }
    [Networked] private bool isAttackActive { get; set; } // 攻撃が有効かどうか

    private ParticleSystem newParticle;
    [Tooltip("攻撃エフェクト")]
    public ParticleSystem AttackParticle;

    private GameObject Pare;
    private Vector3 originalPosition;

    [Networked] public PARRYTYPE Type { get; set; }
    [Networked] public bool isTanuki { get; set; }
    [Networked] public bool isKitune { get; set; }

    private bool isReturningToPosition = false;
    private float returnTimer = 0f; // 元の位置に戻る際のタイマー
    private Vector3 startPosition; // 現在の位置を記録

    public override void Spawned()
    {
        box = GameObject.Find("Networkbox");
        parent = transform.parent.gameObject;
        timer = deactivateTime;
        Pare = transform.parent.gameObject;
        isTanuki = false;
        isKitune = false;
        isAttackActive = false; // 初期状態は無効化
        originalPosition = transform.position;
    }

    public void OnEnable()
    {
        Debug.Log("攻撃エリア有効化");
        SetAttackActive(true);
    }

    public override void Render()
    {
        if (isAttackActive)
        {
            // パーティクルシステムのインスタンスを生成
            newParticle = Instantiate(AttackParticle);
            if (Pare.transform.localScale.x >= 0)
            {
                newParticle.transform.position = new Vector3(transform.position.x - 4.0f, transform.position.y - 2.0f, transform.position.z);
            }
            else
            {
                newParticle.transform.position = new Vector3(transform.position.x + 4.0f, transform.position.y - 2.0f, transform.position.z);
            }

            newParticle.Play();
            Destroy(newParticle.gameObject, 1f);
        }
    }

    public void SetAttackActive(bool isActive)
    {
        isAttackActive = isActive;
        if (isActive)
        {
            timer = deactivateTime; // タイマーをリセット
        }
        else
        {
            StartReturningToPosition(); // 無効化時に元の位置に戻る処理を開始
        }
    }

    private void StartReturningToPosition()
    {
        isReturningToPosition = true;
        returnTimer = 0f;
        startPosition = transform.position;
    }

    private void ReturnToOriginalPosition()
    {
        if (!isReturningToPosition) return;

        returnTimer += Runner.DeltaTime;
        float t = returnTimer / returnDuration;
        transform.position = Vector3.Lerp(startPosition, originalPosition, t);

        if (t >= 1f)
        {
            isReturningToPosition = false; // 完了したらフラグをリセット
            transform.position = originalPosition; // 最終的に正確な元の位置にセット
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isAttackActive) return; // 攻撃が無効なら処理をスキップ

        if (other.CompareTag("Player"))
        {
            timer = deactivateTime;

            // パリィ処理
            if (!parent.GetComponent<BossAI>().isParry)
            {
                if (other.GetComponent<PlayerParryNet>().ParryCheck() &&
                    ((other.GetComponent<PlayerParryNet>().isTanuki && Type == PARRYTYPE.TANUKI) ||
                    (!other.GetComponent<PlayerParryNet>().isTanuki && Type == PARRYTYPE.KITUNE) ||
                    Type == PARRYTYPE.ALL))
                {
                    Debug.Log("パリィ成功");
                    other.GetComponent<PlayerParryNet>().RPC_ParrySystem();

                    if (parent.GetComponent<BossAI>().isKnockBack)
                    {
                        parent.GetComponent<BossAI>().RPC_AnimName();
                    }

                    SetAttackActive(false);
                    return;
                }
                else if (other.GetComponent<PlayerParryNet>().ParryCheck() && Type == PARRYTYPE.DOUBLE)
                {
                    if (other.GetComponent<PlayerParryNet>().isTanuki)
                    {
                        isTanuki = true;
                    }
                    else if (!other.GetComponent<PlayerParryNet>().isTanuki)
                    {
                        isKitune = true;
                    }
                    if (isTanuki && isKitune)
                    {
                        Debug.Log("パリィ成功");
                        other.GetComponent<PlayerParryNet>().RPC_ParrySystem();

                        if (parent.GetComponent<BossAI>().isKnockBack)
                        {
                            parent.GetComponent<BossAI>().RPC_AnimName();
                        }

                        SetAttackActive(false);
                        return;
                    }
                }
            }

            // ダメージ処理
            Debug.Log("攻撃ヒット");
            if (other.GetComponent<PlayerHP>().inbisibleFrame == 0)
            {
                box.GetComponent<ShareNumbers>().CurrentHP--;
                box.GetComponent<ShareNumbers>().RPC_Damage();
                other.GetComponent<PlayerHP>().RPC_DamageAnim();
            }

            SetAttackActive(false);
        }
    }

    public override void FixedUpdateNetwork()
    {
        if (isAttackActive)
        {
            timer -= Runner.DeltaTime;
            if (timer <= 0)
            {
                SetAttackActive(false);
            }
        }

        ReturnToOriginalPosition();
    }
}
