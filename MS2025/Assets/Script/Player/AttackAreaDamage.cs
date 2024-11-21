using Fusion;
using UnityEngine;

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

    [SerializeField] GameObject Gekiobj;
    NetworkRunner runner;

    [Networked] bool isGeki { get; set; } = false;

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
    }
    private void OnTriggerEnter(Collider other)
    {
        if (Object.HasStateAuthority)
        {
            if (other.GetComponent<BossStatus>())
            {
                Debug.Log("�̂������Ă�Ȃ�" + other.GetComponent<BossAI>().GetCurrentAction().actionName);

                //
                if (other.GetComponent<BossAI>().Nokezori>0)
                {
                    if (other.GetComponent<BossAI>().Nokezori == 1)
                    {
                        other.GetComponent<BossStatus>().RPC_Damage(buddyFinalDamageNum);
                        player.GetComponent<HitStop>().ApplyHitStop(buddyFinalStopFrame);
                    }
                    else
                    {
                        other.GetComponent<BossStatus>().RPC_Damage(buddyDamageNum);
                        player.GetComponent<HitStop>().ApplyHitStop(buddyStopFrame);
                    }

                    other.GetComponent<BossAI>().Nokezori--;
                    other.GetComponent<BossAI>().isInterrupted = true;
                    Debug.Log("�̂������Ă�Ȃ�" + other.GetComponent<BossAI>().Nokezori);
                    RPCCombo();
                    return;
                }
                if(raise.GetisRaise())
                {
                    Debug.Log("���đM");
                    sharenum.jumpAttackNum++;

                    if (other.GetComponent<BossAI>().isAir)
                    {
                        other.GetComponent<BossAI>().isDown = true;                        
                    }
                }
                other.GetComponent<BossStatus>().RPC_Damage(DamageNum);

                //���������ʒu�Ɍ��\��
                isGeki = true;
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
        if(isGeki)
        {
            NetworkObject geki =  runner.Spawn(Gekiobj, player.transform.position + Gekiobj.transform.position, Quaternion.identity, runner.LocalPlayer);
            geki.GetComponent<GekiDisplay>().SetPos(player.transform.position + Gekiobj.transform.position);
            isGeki = false;
        }


    }
}
