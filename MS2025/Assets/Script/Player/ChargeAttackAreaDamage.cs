using Fusion;
using System.Collections.Generic;
using UnityEngine;

public class ChargeAttackAreaDamage : NetworkBehaviour
{
    GameObject player;
    [SerializeField] GameObject netobj;
    PlayerChargeAttack attack;
    ShareNumbers sharenum;
    ComboSystem combo;
    [SerializeField, Tooltip("�q�b�g�X�g�b�v����(f)")] int stopFrame;
    [SerializeField] int ChargeDamege = 500;
    [SerializeField] GameObject Gekiobj;
    NetworkRunner runner;

    [SerializeField, Tooltip("�����̃X�v���C�g")] List<Sprite> damagesprite;
    [SerializeField, Tooltip("�����̃X�v���C�g")] GameObject damageobj;

    [SerializeField, Tooltip("�_���[�W����\�����鎞�̐����͈͂̍ŏ�")] float MinRange = -0.2f;
    [SerializeField, Tooltip("�_���[�W����\�����鎞�̐����͈͂̍ŏ�")] float MaxRange = 0.2f;

    [Networked] bool isGeki { get; set; } = false;

    public override void Spawned()
    {
        player = transform.parent.gameObject;
        attack = player.GetComponent<PlayerChargeAttack>();
        netobj = GameObject.Find("Networkbox");
        if (netobj == null)
        {
            Debug.LogError("�l�b�g�̔���������");
        }
        sharenum = netobj.GetComponent<ShareNumbers>();
        combo = netobj.GetComponent<ComboSystem>();
        runner = GameObject.Find("Runner(Clone)").GetComponent<NetworkRunner>();

    }

    private void OnTriggerEnter(Collider other)
    {
        if (Object.HasStateAuthority)
        {
            if (other.GetComponent<BossStatus>())
            {
                Debug.Log("�`���[�W�A�^�b�N����");
                other.GetComponent<BossStatus>().RPC_Damage(ChargeDamege);

                Camera.main.GetComponent<CameraEffectPlay>().RPC_CameraEffect();
                Camera.main.GetComponent<CameraShake>().RPC_CameraShake(0.3f, 0.3f);

                //���������ʒu�Ɍ��\��
                DisplayNumber(ChargeDamege, other.transform);
                other.GetComponent<BossAI>().isInterrupted = true;
                RPCCombo();
                player.GetComponent<HitStop>().ApplyHitStop(stopFrame);
                other.GetComponent<BossAI>().RPC_AnimName();
            }
        }
    }
    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void RPCCombo()
    {
        combo.AddCombo();
    }

    public override void Render()
    {
        if (isGeki)
        {
            NetworkObject geki = runner.Spawn(Gekiobj, player.transform.position + Gekiobj.transform.position, Quaternion.identity, runner.LocalPlayer);
            geki.GetComponent<GekiDisplay>().SetPos(player.transform.position + Gekiobj.transform.position);
            isGeki = false;
        }


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
            Debug.Log("�_���[�W��" + numberObj.transform.position.y);

            // ���b��ɏ�����悤�ɐݒ�
            //Destroy(numberObj, 1.5f); // 1.5�b��ɃI�u�W�F�N�g���폜
        }
    }
}
