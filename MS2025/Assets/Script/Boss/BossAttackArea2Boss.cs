using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttackArea2Boss : NetworkBehaviour
{
    GameObject box;
    GameObject parent;  

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
        parent = GameObject.Find("Boss2D");       
        Pare = GameObject.Find("Boss2D");
        isTanuki = false;
        isKitune = false;        
        originalPosition = transform.position;
    }  

    public override void Render()
    {
       
            //// パーティクルシステムのインスタンスを生成
            //newParticle = Instantiate(AttackParticle);
            //if (Pare.transform.localScale.x >= 0)
            //{
            //    newParticle.transform.position = new Vector3(transform.position.x - 4.0f, transform.position.y - 2.0f, transform.position.z);
            //}
            //else
            //{
            //    newParticle.transform.position = new Vector3(transform.position.x + 4.0f, transform.position.y - 2.0f, transform.position.z);
            //}

            //newParticle.Play();
            //Destroy(newParticle.gameObject, 1f);
        
    }
    
    private void OnTriggerEnter(Collider other)
    {        
        if (other.CompareTag("Player"))
        {
            // パリィ処理
            if (!parent.GetComponent<BossAI>().isParry)
            {
                // パリィ不可攻撃かどうか
                if (!parent.GetComponent<BossAI>().isParry)
                {
                    //パリィ成功したかどうか
                    if (other.GetComponent<PlayerParryNet>().ParryCheck())
                    {
                        //狸で攻撃も狸ならパリィ成功
                        if (other.GetComponent<PlayerParryNet>().isTanuki && Type == PARRYTYPE.TANUKI)
                        {
                            Debug.Log("パリィ成功");
                            other.GetComponent<PlayerParryNet>().RPC_ParrySystem();

                            // ノックバック可能かどうか
                            if (parent.GetComponent<BossAI>().isKnockBack)
                            {
                                parent.GetComponent<BossAI>().RPC_AnimName();
                            }
                           
                            //gameObject.SetActive(false);
                            return;
                        }

                        //狐で攻撃も狐ならパリィ成功
                        if (!other.GetComponent<PlayerParryNet>().isTanuki && Type == PARRYTYPE.KITUNE)
                        {
                            Debug.Log("パリィ成功");
                            other.GetComponent<PlayerParryNet>().RPC_ParrySystem();

                            // ノックバック可能かどうか
                            if (parent.GetComponent<BossAI>().isKnockBack)
                            {
                                parent.GetComponent<BossAI>().RPC_AnimName();
                            }
                           
                            //gameObject.SetActive(false);
                            return;
                        }

                        //どれでもパリィ可能攻撃
                        if (Type == PARRYTYPE.ALL)
                        {
                            Debug.Log("パリィ成功");
                            other.GetComponent<PlayerParryNet>().RPC_ParrySystem();

                            // ノックバック可能かどうか
                            if (parent.GetComponent<BossAI>().isKnockBack)
                            {
                                parent.GetComponent<BossAI>().RPC_AnimName();
                            }
                          
                            //gameObject.SetActive(false);
                            return;
                        }

                        //ダブルパリィ
                        if (Type == PARRYTYPE.DOUBLE)
                        {
                            if (other.GetComponent<PlayerParryNet>().isTanuki)
                            {
                                isTanuki = true;
                            }
                            else if (other.GetComponent<PlayerParryNet>().isTanuki == false)
                            {
                                isKitune = true;
                            }
                            if (isTanuki && isKitune)
                            {
                                Debug.Log("パリィ成功");
                                other.GetComponent<PlayerParryNet>().RPC_ParrySystem();

                                // ノックバック可能かどうか
                                if (parent.GetComponent<BossAI>().isKnockBack)
                                {
                                    parent.GetComponent<BossAI>().RPC_AnimName();
                                }
                                
                                //gameObject.SetActive(false);
                                return;
                            }
                        }
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
        }
    }   
}
