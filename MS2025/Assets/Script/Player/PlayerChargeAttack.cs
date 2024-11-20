using UnityEngine;
using Fusion;

public class PlayerChargeAttack : NetworkBehaviour
{
    [Networked] public NetworkButtons ButtonsPrevious { get; set; }
    GameObject attackArea;
    [SerializeField]
    GameObject netobj;
    ShareNumbers sharenum;

    public bool isCharge=false;//チャージ中か否か
    [SerializeField, Networked] bool isAttack { get; set; } = false;

    [SerializeField, Tooltip("発生f")]
    int Startup;
    [SerializeField, Tooltip("持続f")]
    int Active;
    [SerializeField, Tooltip("硬直f")]
    int Recovery;
    [SerializeField, Tooltip("溜めに必要なフレーム")]
    int maxCharge;
    int Count;
    int chargeCount;

    [SerializeField, Tooltip("溜めエフェクト")]
    GameObject chargeeffect;

    [SerializeField, Tooltip("溜め攻撃エフェクト")]
    GameObject attackeffect;

    ParticleSystem chargeparticle;
    ParticleSystem attackparticle;

    [Networked] private bool isEffect { get; set; }
    [Networked] private bool isAttackEffect { get; set; }

    GameObject BossObj = null;

    [SerializeField, Tooltip("連携攻撃可能時間のエフェクト")]
    GameObject effectRengekiTime;

    [Networked] public bool isWait { get; private set; }


    // Start is called before the first frame update
    public override void Spawned()
    {
        attackArea = gameObject.transform.Find("ChargeAttackArea").gameObject;
        attackArea.SetActive(false);
        netobj = GameObject.Find("Networkbox");
        if (netobj == null)
        {
            Debug.LogError("ネットオブジェクトないよ");
        }
        sharenum = netobj.GetComponent<ShareNumbers>();
        chargeparticle = chargeeffect.GetComponent<ParticleSystem>();
        attackparticle = attackeffect.GetComponent<ParticleSystem>();

        BossObj = GameObject.Find("Boss2D");
        if (BossObj == null)
        {
            Debug.LogError("ぼすないよ");
        }
    }
    public override void FixedUpdateNetwork()
    {
        if (Object.HasStateAuthority && GetInput(out NetworkInputData data))
        {
            AnimatorStateInfo landAnimStateInfo = GetComponent<Animator>().GetCurrentAnimatorStateInfo(0);
            if (landAnimStateInfo.IsName("APlayerParry") ||//パリィ時は攻撃しない
                landAnimStateInfo.IsName("APlayerJumpUp") || landAnimStateInfo.IsName("APlayerJumpDown"))//ジャンプ中は攻撃しない
            {
                return;
            }

            var released = data.Buttons.GetReleased(ButtonsPrevious);
            ButtonsPrevious = data.Buttons;


            // Attackボタンが押されたか、かつアニメーションが再生中でないかチェック
            if (data.Buttons.IsSet(NetworkInputButtons.ChargeAttack))
            {
                Debug.Log("溜めてます" + chargeCount);
                isCharge = true;
                chargeCount++;
                isEffect = true;
            }
            else if (released.IsSet(NetworkInputButtons.ChargeAttack) && isCharge&&chargeCount>maxCharge)
            {
                chargeparticle.Stop();
               // attackArea.SetActive(true);
                chargeCount = 0;
                isCharge = false;
                isAttackEffect = true;
                isAttack = true;
                isWait = true;

            }
            else
            {
                isCharge = false;
                chargeCount = 0;
                isAttackEffect = false;
                chargeparticle.Stop();
            }
        }



    }

    public override void Render()
    {
        Attack();


        if (isWait)
        {
            Debug.Log("硬直中");
            effectRengekiTime.SetActive(false);
        }
        else
        {
            Debug.Log("硬直終わり");

            if (BossObj.GetComponent<BossAI>().Nokezori > 0)
            {
                effectRengekiTime.SetActive(true);
            }
            else
            {
                effectRengekiTime.SetActive(false);

            }

        }

        if (isEffect)
        {
            chargeparticle.Play();
            isEffect = false;
        }
        else
        {
            chargeparticle.Stop();

        }
        if (isAttackEffect)
        {
            attackparticle.Play();
            isAttackEffect = false;
        }
    }

    void Attack()
    {
        if (isAttack == false)
        {
            isWait = false;

            return;
        }
        Debug.Log("溜め攻撃");
        if (Count < Startup)
        {
            Count++;

        }
        else if (Count < Startup + Active)
        {
            Count++;
            attackArea.SetActive(true);

        }
        else if (Count < Startup + Active + Recovery)
        {
            Count++;
            attackArea.SetActive(false);

        }
        else if (Count == Startup + Active + Recovery)
        {

            Count = 0;
            isAttack = false;
        }
        else if (Count > Startup + Active + Recovery)
        {

            Count = 0;
            isAttack = false;
        }

    }
}
