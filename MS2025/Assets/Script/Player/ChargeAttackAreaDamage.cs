using Fusion;
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
                isGeki = true;
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

}
