using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttackArea : NetworkBehaviour
{
    GameObject box;
    GameObject parent;
    private float deactivateTime = 0.5f; // 攻撃エリアの非表示にするまでの時間
    private float timer;
    private Vector3 PlayerPos;
    private bool isAttack=false;
    [Tooltip("攻撃エフェクト")]
    public ParticleSystem AttackParticle;

    public override void Spawned()
    {
        box = GameObject.Find("Networkbox");
        parent = transform.parent.gameObject;
        timer = deactivateTime;
       
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (other.GetComponent<PlayerParryNet>().ParryCheck())
            {
                Debug.Log("パリィ成功");
                other.GetComponent<PlayerParryNet>().RPC_ParrySystem();
                parent.GetComponent<BossAI>().RPC_AnimName();
                gameObject.SetActive(false);

                return;
            }

            Debug.Log("攻撃ヒット");
            box.GetComponent<ShareNumbers>().RPC_Damage();
            other.GetComponent<PlayerHP>().RPC_DamageAnim();
            isAttack = true;
            PlayerPos = other.transform.position;
            gameObject.SetActive(false);
        }
    }

    public override void Render()
    {
     

        if(isAttack)
        {
            // パーティクルシステムのインスタンスを生成
            ParticleSystem newParticle = Instantiate(AttackParticle);
            //パーティクルを生成
            newParticle.transform.position = (this.transform.position + PlayerPos)/2;
            // パーティクルを発生させる
            newParticle.Play();
            // インスタンス化したパーティクルシステムのGameObjectを1秒後に削除
            Destroy(newParticle.gameObject, 0.4f);
            isAttack = false;
        }
    }

    public override void FixedUpdateNetwork()
    {
        // タイマーを減らし、一定時間後に非表示にする
        if (timer > 0)
        {
            timer -= Runner.DeltaTime;
            if (timer <= 0)
            {
                gameObject.SetActive(false);
                timer = deactivateTime;
            }
        }
    }
}
