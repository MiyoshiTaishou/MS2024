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
                other.GetComponent<BossStatus>().RPC_Damage(10);
                sharenum.AddHitnum();
                RPCCombo();
                if(raise.GetisRaise())
                {
                    Debug.Log("龍墜閃");
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
