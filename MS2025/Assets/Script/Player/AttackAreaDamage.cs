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

    [SerializeField, Header("ダメージ量")] int DamageNum = 100;

    public override void Spawned()
    {
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

                if (other.GetComponent<BossAI>().GetCurrentAction().actionName=="Idol"
                    && other.GetComponent<BossAI>().Nokezori>0)
                {               
                    if(other.GetComponent<BossAI>().Nokezori==1)
                    {
                        other.GetComponent<BossStatus>().RPC_Damage(350);
                    }
                    other.GetComponent<BossAI>().Nokezori--;
                    other.GetComponent<BossAI>().isInterrupted = true;
                    other.GetComponent<BossStatus>().RPC_Damage(40);
                    Debug.Log("のけぞってるなう" + other.GetComponent<BossAI>().Nokezori);
                }
                other.GetComponent<BossStatus>().RPC_Damage(DamageNum);
                sharenum.AddHitnum();
                RPCCombo();
                if(raise.GetisRaise())
                {
                    Debug.Log("龍墜閃");

                    if(other.GetComponent<BossAI>().isAir)
                    {
                        other.GetComponent<BossAI>().isDown = true;
                    }
                }
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
}
