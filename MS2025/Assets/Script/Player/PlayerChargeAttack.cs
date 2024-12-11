using UnityEngine;
using Fusion;
using UnityEditor.Rendering;
using UnityEngine.SceneManagement;

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
    PlayerFreeze freeze;

    [SerializeField, Tooltip("チャージ完了エフェクト")] private Color color;

    private SpriteRenderer sprite;

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
        Scene scene=SceneManager.GetActiveScene();
        GameObject[] allobj=scene.GetRootGameObjects();
        foreach (GameObject obj in allobj)
        {
            if(obj.CompareTag("Enemy"))
            {
                BossObj = obj;
                Debug.Log("ぼすの名前" + obj.name);
                break;
            }
        }
        if (BossObj == null)
        {
            Debug.LogError("ぼすないよ");
        }
        freeze= GetComponent<PlayerFreeze>();

        sprite=GetComponent<SpriteRenderer>();
    }
    public override void FixedUpdateNetwork()
    {
        if(sharenum.CurrentHP == 0)
        {
            return;
        }
        if (Object.HasStateAuthority && GetInput(out NetworkInputData data))
        {
            if(GetComponent<PlayerJumpNet>().GetisJumping())
            {
                chargeCount = 0;
                return;
            }
            AnimatorStateInfo landAnimStateInfo = GetComponent<Animator>().GetCurrentAnimatorStateInfo(0);
            if (landAnimStateInfo.IsName("APlayerJumpUp") || landAnimStateInfo.IsName("APlayerJumpDown")//ジャンプ中は攻撃しない
                || freeze.GetIsFreeze())
            {
                chargeCount= 0;
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
            else
            {
                isCharge = false;
                chargeCount = 0;
                isAttackEffect = false;
                chargeparticle.Stop();
            }
            if (data.Buttons.IsSet(NetworkInputButtons.ChargeAttack) && isCharge&&chargeCount>maxCharge)
            {
                chargeparticle.Stop();
               // attackArea.SetActive(true);
                chargeCount = 0;
                isCharge = false;
                isAttackEffect = true;
                isAttack = true;
                isWait = true;

            }
        }

        if(chargeCount > maxCharge)
        {
            sprite.color = color;
        }
        else if (GetComponent<PlayerHP>().inbisibleFrame == 0)
        {
            sprite.color = Color.white;

        }

    }

    public override void Render()
    {
        Attack();

        if (freeze.GetIsFreeze())
        {
            Debug.Log("硬直中");
            effectRengekiTime.SetActive(false);
        }
        else
        {
            Debug.Log("硬直終わり");

            if (BossObj.GetComponent<BossAI>().Nokezori > 0  )
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
            freeze.Freeze(Active + Recovery);
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
