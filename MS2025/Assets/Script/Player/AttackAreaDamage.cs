using Fusion;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class AttackAreaDamage : NetworkBehaviour
{
    GameObject player;
    [SerializeField] GameObject netobj;
    PlayerAttack attack;
    ShareNumbers sharenum;
    ComboSystem combo;
    PlayerRaise raise;
    [SerializeField, Tooltip("�q�b�g�X�g�b�v����(f)")] int stopFrame;
    [SerializeField, Tooltip("�A�g�U���q�b�g�X�g�b�v����(f)")] int buddyStopFrame;
    [SerializeField, Tooltip("�A�g�U���t�B�j�b�V���q�b�g�X�g�b�v����(f)")] int buddyFinalStopFrame;

    [SerializeField, Header("�_���[�W��")] int DamageNum = 100;
    [SerializeField, Header("�A�g�U���_���[�W��")] int buddyDamageNum = 100;
    [SerializeField, Header("�A�g�U���t�B�j�b�V���_���[�W��")] int buddyFinalDamageNum = 100;

    [SerializeField, Tooltip("�����̃X�v���C�g")] List<Sprite> damagesprite;
    [SerializeField, Tooltip("�����̃X�v���C�g")] GameObject damageobj;

    [SerializeField, Tooltip("�_���[�W����\�����鎞�̐����͈͂̍ŏ�")] float MinRange = -0.2f;
    [SerializeField, Tooltip("�_���[�W����\�����鎞�̐����͈͂̍ŏ�")] float MaxRange = 0.2f;

    NetworkRunner runner;
    [Networked] Vector3 bosspos { get; set; }
    [Networked] Vector3 bossscale { get; set; }

    [Networked,SerializeField] bool isGeki { get; set; } = false;

    [Networked] int hitdamege { get; set; } = 0;
        
    PlayerParryNet parry;

    public override void Spawned()
    {
        runner = GameObject.Find("Runner(Clone)").GetComponent<NetworkRunner>();

        player = transform.parent.gameObject;
        attack = player.GetComponent<PlayerAttack>();
        raise = player.GetComponent<PlayerRaise>();
        netobj = GameObject.Find("Networkbox");
        if (netobj == null)
        {
            Debug.LogError("�l�b�g�̔���������");
        }
        sharenum = netobj.GetComponent<ShareNumbers>();
        combo = netobj.GetComponent<ComboSystem>();
        parry = transform.parent.GetComponent<PlayerParryNet>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (Object.HasStateAuthority)
        {
            if (other.GetComponent<BossStatus>())
            {
                Debug.Log("�̂������Ă�Ȃ�" + other.GetComponent<BossAI>().GetCurrentAction().actionName);

                //
                if (other.GetComponent<BossAI>().Nokezori > 0)
                {
                    if (other.GetComponent<BossAI>().Nokezori == 1)
                    {
                        other.GetComponent<BossStatus>().RPC_Damage(buddyFinalDamageNum);
                        player.GetComponent<HitStop>().ApplyHitStop(buddyFinalStopFrame);
                        hitdamege = buddyFinalDamageNum;

                    }
                    else
                    {
                        other.GetComponent<BossStatus>().RPC_Damage(buddyDamageNum);
                        player.GetComponent<HitStop>().ApplyHitStop(buddyStopFrame);

                        hitdamege = buddyDamageNum;

                    }
                    //����������_���[�W���\��
                    if (parry.isTanuki)
                    {
                        GekiUI(other.transform);
                        // Debug.Log("�z�X�g�_���[�W��");

                    }
                    else
                    {
                        isGeki = true;
                        bosspos = other.transform.position;
                        bossscale = other.transform.localScale;
                        hitdamege = DamageNum;
                        //Debug.Log("�_���[�W��" + bosspos);

                    }
                    other.GetComponent<BossAI>().Nokezori--;
                    other.GetComponent<BossAI>().isInterrupted = true;
                    Debug.Log("�̂������Ă�Ȃ�" + other.GetComponent<BossAI>().Nokezori);
                    RPCCombo();
                    return;
                }
                if (raise.GetisRaise())
                {
                    Debug.Log("���đM");
                    sharenum.jumpAttackNum++;

                    if (other.GetComponent<BossAI>().isAir)
                    {
                        other.GetComponent<BossAI>().isDown = true;
                    }
                }
                other.GetComponent<BossStatus>().RPC_Damage(DamageNum);

                //����������_���[�W���\��
                if (parry.isTanuki)
                {
                    GekiUI(other.transform);
                   // Debug.Log("�z�X�g�_���[�W��");

                }
                else
                {
                    isGeki = true;
                    bosspos = other.transform.position;
                    bossscale = other.transform.localScale;
                    hitdamege = DamageNum;
                    //Debug.Log("�_���[�W��" + bosspos);

                }
                //Debug.Log(other.name);
                sharenum.AddHitnum();
                RPCCombo();
                player.GetComponent<HitStop>().ApplyHitStop(stopFrame);
            }
        }

    }
    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void RPCCombo()
    {
        attack.currentCombo = sharenum.nHitnum;
        Debug.Log("��������������������������������������������������������������������������������������������������������");
        combo.AddCombo();
    }

    public override void Render()
    {

        //�z�X�g�Ȃ�I��
        if (Runner.IsServer)
        {
            //Debug.Log("�_���[�W���z�X�g����");
            return;
        }

        if (isGeki)
        {
            Debug.Log("�N���C�A���g�_���[�W��");
            Transform boss = transform;
            boss.localScale = bossscale;
            boss.position= bosspos;
            GekiUI(boss);
            //boss = null;
            isGeki = false;
            this.enabled= false;
        }


    }

    public void GekiUI(Transform pos)
    {
        Debug.Log("gggeee�_���[�W��");
        DisplayNumber(hitdamege, pos);
    }

    public void DisplayNumber(int damage, Transform pos)
    {
        // �_���[�W�l�𕶎���Ƃ��Ĉ���
        string damageStr = damage.ToString();

        // �����_���ȃI�t�Z�b�g���v�Z
        float randomX = Random.Range(MinRange, MaxRange); // -0.2�`0.2�̊Ԃ�X���W�������_����
        float randomY = Random.Range(MinRange, MaxRange); // -0.2�`0.2�̊Ԃ�Y���W�������_����

        // �e���̐����𐶐�
        for (int i = 0; i < damageStr.Length; i++)
        {
            // �������擾
            int digit = int.Parse(damageStr[i].ToString());

            // �����I�u�W�F�N�g�𐶐�
            GameObject numberObj = Instantiate(damageobj, new Vector3(0, 0, 0), Quaternion.identity);

            // �X�v���C�g��ݒ�
            SpriteRenderer spriteRenderer = numberObj.GetComponent<SpriteRenderer>();
            spriteRenderer.sprite = damagesprite[digit];

            // �z�u�𒲐��i�����Ƃɉ��ɕ��ׂA�����_���Ȉʒu�ɂ��炷�j
            numberObj.transform.position = new Vector3(
                 pos.position.x + (i * 1f + randomX), // �����Ƃ̔z�u�Ƀ����_����X�I�t�Z�b�g��ǉ�
                pos.position.y + (pos.localScale.y / 4), // Y���W�ɂ������_���I�t�Z�b�g��ǉ�
                 pos.position.z
            );
           // Debug.Log("�_���[�W��" + numberObj.transform.position.y);

            // ���b��ɏ�����悤�ɐݒ�
            //Destroy(numberObj, 1.5f); // 1.5�b��ɃI�u�W�F�N�g���폜
        }
    }


}
