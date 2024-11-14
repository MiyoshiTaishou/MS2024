using UnityEngine;
using Fusion;

public class PlayerChargeAttack : NetworkBehaviour
{
    [Networked] public NetworkButtons ButtonsPrevious { get; set; }
    GameObject attackArea;
    [SerializeField]
    GameObject netobj;
    ShareNumbers sharenum;

    public bool isCharge=false;//�`���[�W�����ۂ�
    bool isAttack = false;

    [SerializeField, Tooltip("����f")]
    int Startup;
    [SerializeField, Tooltip("����f")]
    int Active;
    [SerializeField, Tooltip("�d��f")]
    int Recovery;
    [SerializeField, Tooltip("���߂ɕK�v�ȃt���[��")]
    int maxCharge;
    int Count;
    int chargeCount;

    [SerializeField, Tooltip("���߃G�t�F�N�g")]
    GameObject chargeeffect;

    [SerializeField, Tooltip("���ߍU���G�t�F�N�g")]
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
            Debug.LogError("�l�b�g�I�u�W�F�N�g�Ȃ���");
        }
        sharenum = netobj.GetComponent<ShareNumbers>();
        chargeparticle = chargeeffect.GetComponent<ParticleSystem>();
        attackparticle = attackeffect.GetComponent<ParticleSystem>();

    }
    public override void FixedUpdateNetwork()
    {
        if (Object.HasStateAuthority && GetInput(out NetworkInputData data))
        {
            AnimatorStateInfo landAnimStateInfo = GetComponent<Animator>().GetCurrentAnimatorStateInfo(0);
            if (landAnimStateInfo.IsName("APlayerParry") ||//�p���B���͍U�����Ȃ�
                landAnimStateInfo.IsName("APlayerJumpUp") || landAnimStateInfo.IsName("APlayerJumpDown"))//�W�����v���͍U�����Ȃ�
            {
                return;
            }

            var released = data.Buttons.GetReleased(ButtonsPrevious);
            ButtonsPrevious = data.Buttons;


            // Attack�{�^���������ꂽ���A���A�j���[�V�������Đ����łȂ����`�F�b�N
            if (data.Buttons.IsSet(NetworkInputButtons.ChargeAttack))
            {
                Debug.Log("���߂Ă܂�" + chargeCount);
                isCharge = true;
                chargeCount++;
                isEffect = true;
            }
            else if (released.IsSet(NetworkInputButtons.ChargeAttack) && isCharge&&chargeCount>maxCharge)
            {
                chargeparticle.Stop();
                attackArea.SetActive(true);
                chargeCount = 0;
                isCharge = false;
                isAttackEffect = true;
                isAttack = true;
            }
            else
            {
                isCharge = false;
                chargeCount = 0;
                isAttackEffect = false;
                chargeparticle.Stop();
            }
            Attack();
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

    void Attack()
    {
        if (isAttack == false)
        {
            return;
        }
        Debug.Log("���ߍU��");
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
        else if (Count >= Startup + Active + Recovery)
        {
            Count = 0;
            isAttack = false;
        }
    }
}
