using UnityEngine;
using Fusion;

public class PlayerChargeAttack : NetworkBehaviour
{
    [Networked] public NetworkButtons ButtonsPrevious { get; set; }
    GameObject attackArea;
    [SerializeField]
    GameObject netobj;
    ShareNumbers sharenum;

    bool isCharge=false;//チャージ中か否か

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

    }
    public override void FixedUpdateNetwork()
    {
        if (Object.HasStateAuthority && GetInput(out NetworkInputData data))
        {
            //AnimatorStateInfo landAnimStateInfo = GetComponent<Animator>().GetCurrentAnimatorStateInfo(0);
            //if (landAnimStateInfo.IsName("APlayerParry") ||//パリィ時は攻撃しない
            //    landAnimStateInfo.IsName("APlayerJumpUp") || landAnimStateInfo.IsName("APlayerJumpDown"))//ジャンプ中は攻撃しない
            //{
            //    return;
            //}

            var released = data.Buttons.GetReleased(ButtonsPrevious);
            ButtonsPrevious = data.Buttons;


            // Attackボタンが押されたか、かつアニメーションが再生中でないかチェック
            if (data.Buttons.IsSet(NetworkInputButtons.ChargeAttack) && !isCharge)
            {
                isCharge = true;
                chargeCount++;
                Count++;
                isEffect = true;
            }
            else if (released.IsSet(NetworkInputButtons.ChargeAttack) && isCharge)
            {
                chargeparticle.Stop();
                attackArea.SetActive(true);
                chargeCount = 0;
                isCharge = false;
                isAttackEffect = true;
            }
            //if(released.IsSet(chargeCount)&&chargeCount>maxCharge)
            //{
            //    Debug.Log("溜め攻撃ﾅｱｱｱｱｱｱﾝ");
            //}
        }



    }

    public override void Render()
    {

        if (isEffect)
        {
            chargeparticle.Play();
            isEffect = false;
        }
        if (isAttackEffect)
        {
            attackparticle.Play();
            isAttackEffect = false;
        }
    }
}
