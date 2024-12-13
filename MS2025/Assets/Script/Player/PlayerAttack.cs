using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : NetworkBehaviour
{
    Animator animator;
    AudioSource audioSource;
    GameObject attackArea;
    [SerializeField]
    GameObject netobj;
    [Networked] public int currentCombo { get; set; }

    ShareNumbers sharenum;

    [Networked] public bool isAttack { get; private set; }
    [Networked] private bool isOnce { get; set; }
    [Networked] private bool isPlayingAnimation { get; set; }
    [Networked] public NetworkButtons ButtonsPrevious { get; set; }

    [Header("攻撃SE"), SerializeField] private AudioClip[] attackSE;


    [SerializeField, Tooltip("発生f")]
    int Startup;
    [SerializeField, Tooltip("持続f")]
    int Active;
    [SerializeField, Tooltip("硬直f")]
    int Recovery;
    [SerializeField, Tooltip("連携フィニッシュ発生f")]
    int buddyStartup;
    [SerializeField, Tooltip("連携フィニッシュ持続f")]
    int buddyActive;
    [SerializeField, Tooltip("連携フィニッシュ硬直f")]
    int buddyRecovery;

    int Count;

    [SerializeField, Tooltip("攻撃三種のエフェクト")]
    List<GameObject> effectList;

    [SerializeField, Tooltip("攻撃三種のエフェクト")]
    GameObject effectobj;
    ParticleSystem particle;

    [SerializeField, Tooltip("連携攻撃可能時間のエフェクト")]
    GameObject effectRengekiTime;
    [SerializeField, Tooltip("離れる距離")]
    float disX = 0;
    [Networked] private bool isEffect { get; set; }

    HitStop hitStop;
    GameObject BossObj = null;
    bool flashFlg = false;//連携攻撃による瞬間移動をしたか
    PlayerFreeze freeze;
    [Networked]public bool aaaa { get; set; }
    public override void Spawned()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();        
        attackArea = gameObject.transform.Find("AttackArea").gameObject;
        attackArea.SetActive(false);
        netobj = GameObject.Find("Networkbox");
        if(netobj==null) 
        {
            Debug.LogError("ネットオブジェクトないよ");
        }
        sharenum = netobj.GetComponent<ShareNumbers>();

        particle = effectobj.GetComponent<ParticleSystem>();
        hitStop = GetComponent<HitStop>();
        BossObj = GameObject.Find("Boss2D");
        if (BossObj == null)
        {
            Debug.LogError("ぼすないよ");
        }
        flashFlg = false;
        freeze = GetComponent<PlayerFreeze>();
    }

    public override void FixedUpdateNetwork()
    {
        Debug.Log("aaaa=" + aaaa);
        if(GetComponent<HitStop>().IsHitStopActive||sharenum.CurrentHP==0) 
        {
            return;
        }
        Attack();
        if(freeze.GetIsFreeze())
        {
            return;
        }
        if (Object.HasStateAuthority && GetInput(out NetworkInputData data) && !hitStop.IsHitStopActive)
        {
            AnimatorStateInfo landAnimStateInfo = GetComponent<Animator>().GetCurrentAnimatorStateInfo(0);

            if(landAnimStateInfo.IsName("APlayerJumpUp")||landAnimStateInfo.IsName("APlayerJumpDown")
                || freeze.GetIsFreeze() ||//ジャンプ中は攻撃しない
                GetComponent<PlayerChargeAttack>().isCharge)//溜め中には攻撃しない
                //(BossObj.GetComponent<BossAI>().Nokezori>0&&BossObj.GetComponent<BossAI>().isNokezoriTanuki==GetComponent<PlayerParryNet>().isTanuki))//連携攻撃が一人では発動しない
            {
                return;
            }
            var pressed = data.Buttons.GetPressed(ButtonsPrevious);
            ButtonsPrevious = data.Buttons;

            // Attackボタンが押されたか、かつアニメーションが再生中でないかチェック
            if (pressed.IsSet(NetworkInputButtons.Attack) && !isAttack && currentCombo<2)
            {
                aaaa = true;
                isAttack = true; // 攻撃フラグを立てる
                isPlayingAnimation = true;
                isOnce = true;
                //全プレイヤーにSEを再生する
                RPC_SE();
                //particle.Play();
            }
            else if(pressed.IsSet(NetworkInputButtons.Attack) && currentCombo >= 2)
            {
                isAttack = true; // 攻撃フラグを立てる
                isPlayingAnimation = true;
                isOnce = true;
                //全プレイヤーにSEを再生する
                RPC_SE();
                //isEffect= true;

            }
        }
    }

    public override void Render()
    {
        // 現在のアニメーションの状態を取得
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        if (freeze.GetIsFreeze())
        {
            effectRengekiTime.SetActive(false);
        }
        else
        {
            if (isOnce && BossObj.GetComponent<BossAI>().Nokezori > 0)
                effectRengekiTime.SetActive(true);

        }

        // 攻撃フラグが立っている場合にアニメーションをトリガー
        if (isOnce&&BossObj.GetComponent<BossAI>().Nokezori>0)
        {
            Debug.Log("連携攻撃いいいい");
            GetComponent<PlayerAnimChange>().RPC_InitAction("APlayerAttack");
            isEffect = true;
            isOnce = false; // フラグをリセット
        }
        else if (isOnce&& currentCombo==0)
        {
            //Debug.LogError("壱の秘剣");
            //animator.SetTrigger("Attack"); // アニメーションのトリガー
            GetComponent<PlayerAnimChange>().RPC_InitAction("APlayerAttack");
            //effectList[0].GetComponent<ParticleSystem>().Play();
            isOnce = false; // フラグをリセット
        }
        else if (isOnce&& currentCombo==1)
        {
            //Debug.LogError("弐の秘剣");
            //animator.SetTrigger("Attack2"); // アニメーションのトリガー
            GetComponent<PlayerAnimChange>().RPC_InitAction("APlayerAttack2");
            //effectList[1].GetComponent<ParticleSystem>().Play();
            isOnce = false; // フラグをリセット

        }
        else if (isOnce&& currentCombo>=2)
        {
            //Debug.LogError("終の秘剣");
            //animator.SetTrigger("Attack3"); // アニメーションのトリガー
            GetComponent<PlayerAnimChange>().RPC_InitAction("APlayerAttack3");
           // effectList[2].GetComponent<ParticleSystem>().Play();
            isOnce = false; // フラグをリセット
           
        }

        if (isEffect)
        {
            particle.Play();
            isEffect = false;
        }


        //// アニメーションが再生中である場合の処理
        //if (stateInfo.IsName("APlayerAttack"))
        //{
        //    attackArea.SetActive(true); // 攻撃エリアをアクティブに       
        //}
        //else
        //{
        //    attackArea.SetActive(false); // アニメーションが再生中でない場合は攻撃エリアを無効化
        //    isAttack = false;
        //}
    }

    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void RPC_SE()
    {
        switch (currentCombo) 
        {
            case 0:
                audioSource.PlayOneShot(attackSE[0]);
                break;
            case 1:
                audioSource.PlayOneShot(attackSE[1]);
                break;
            case 2:
                audioSource.PlayOneShot(attackSE[2]);
                break;
            default: 
                break;
        }
        audioSource.PlayOneShot(attackSE[0]);
        isAttack = true; // 攻撃フラグを立てる
        isPlayingAnimation = true;
        isOnce = true;
        currentCombo = sharenum.nHitnum;

    }

    void Attack()
    {
        if(isAttack==false||GetComponent<HitStop>().IsHitStopActive )
        {
            return;
        }
        
        //のけぞり状態に対しての攻撃(連携攻撃)
        if (BossObj.GetComponent<BossAI>().Nokezori > 0)
        {
            //連携フィニッシュ攻撃
            if (BossObj.GetComponent<BossAI>().Nokezori == 1)
            {
                if (Count < buddyStartup)
                {
                    freeze.Freeze(buddyActive + buddyRecovery);
                    Count++;
                    Debug.Log("瞬間移動");
                    //瞬間移動
                    Vector3 pos = transform.position;
                    Vector3 bosspos = BossObj.transform.position;
                    if (!flashFlg)
                    {
                        if (pos.x < bosspos.x)
                        {
                            pos.x = bosspos.x - disX;
                        }
                        else if (pos.x > bosspos.x)
                        {
                            pos.x = bosspos.x + disX;
                        }
                        pos.z = bosspos.z;
                        transform.position = pos;
                        flashFlg = true;
                    }
                }
                else if (Count < buddyStartup + buddyActive)
                {

                    Count++;
                    attackArea.SetActive(true);
                    attackArea.GetComponent<AttackAreaDamage>().enabled= true;
                }
                else if (Count < buddyStartup + buddyActive + buddyRecovery)
                {
                    Count++;
                    attackArea.SetActive(false);
                }
                else if (Count >= buddyStartup + buddyActive + buddyRecovery)
                {
                    flashFlg = false;
                    Count = 0;
                    isAttack = false;
                }
                return;
            }
            else
            {
                if (Count < Startup)
                {
                    freeze.Freeze(Active + Recovery);
                    Count++;
                    Debug.Log("瞬間移動");
                    //瞬間移動
                    Vector3 pos = transform.position;
                    Vector3 bosspos = BossObj.transform.position;
                    if (!flashFlg)
                    {
                        if (pos.x < bosspos.x)
                        {
                            pos.x = bosspos.x - disX;
                        }
                        else if (pos.x > bosspos.x)
                        {
                            pos.x = bosspos.x + disX;
                        }
                        pos.z = bosspos.z;
                        transform.position = pos;
                        flashFlg = true;
                    }
                }
                else if (Count < Startup + Active)
                {
                    Count++;
                    attackArea.SetActive(true);
                    attackArea.GetComponent<AttackAreaDamage>().enabled = true;

                }
                else if (Count < Startup + Active + Recovery)
                {
                    Count++;
                    attackArea.SetActive(false);
                }
                else if (Count >= Startup + Active + Recovery)
                {
                    flashFlg = false;
                    Count = 0;
                    isAttack = false;
                }
                return;
            }
        }
        //通常攻撃
        else if (Count < Startup) 
        {
            freeze.Freeze(Active + Recovery);
            Count++;
        }
        else if(Count <Startup+Active)
        {

            Count++;
        }
        else if(Count == Startup+Active)
        {
            Count++;
            attackArea.SetActive(true);
            attackArea.GetComponent<AttackAreaDamage>().enabled = true;

        }
        else if(Count < Startup + Active + Recovery)
        {
            Count++;
        }
        else if(Count == Startup+Active+Recovery)
        {
            Count++;
            attackArea.SetActive(false);
        }
        else if(Count > Startup + Active + Recovery)
        {
            Count = 0;
            isAttack = false;
        }
    }
}
