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
    [SerializeField, Tooltip("ヒットストップ時間(f)")] int stopFrame;
    [SerializeField, Tooltip("連携攻撃ヒットストップ時間(f)")] int buddyStopFrame;
    [SerializeField, Tooltip("連携攻撃フィニッシュヒットストップ時間(f)")] int buddyFinalStopFrame;

    [SerializeField, Header("ダメージ量")] int DamageNum = 100;
    [SerializeField, Header("連携攻撃ダメージ量")] int buddyDamageNum = 100;
    [SerializeField, Header("連携攻撃フィニッシュダメージ量")] int buddyFinalDamageNum = 100;

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
            Debug.LogError("ネットの箱が無いよ");
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
                Debug.Log("のけぞってるなう" + other.GetComponent<BossAI>().GetCurrentAction().actionName);

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
                    Debug.Log("のけぞってるなう" + other.GetComponent<BossAI>().Nokezori);
                    RPCCombo();
                    return;
                }
                if(raise.GetisRaise())
                {
                    Debug.Log("龍墜閃");
                    sharenum.jumpAttackNum++;

                    if (other.GetComponent<BossAI>().isAir)
                    {
                        other.GetComponent<BossAI>().isDown = true;                        
                    }
                }
                other.GetComponent<BossStatus>().RPC_Damage(DamageNum);

                //当たった位置に撃表示
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
        Debug.Log("ああああああああああああああああああああああああああああああああああああああああああああああああああああ");
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
