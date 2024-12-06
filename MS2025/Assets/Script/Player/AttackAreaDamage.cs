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
    [SerializeField, Tooltip("ヒットストップ時間(f)")] int stopFrame;
    [SerializeField, Tooltip("連携攻撃ヒットストップ時間(f)")] int buddyStopFrame;
    [SerializeField, Tooltip("連携攻撃フィニッシュヒットストップ時間(f)")] int buddyFinalStopFrame;

    [SerializeField, Header("ダメージ量")] int DamageNum = 100;
    [SerializeField, Header("連携攻撃ダメージ量")] int buddyDamageNum = 100;
    [SerializeField, Header("連携攻撃フィニッシュダメージ量")] int buddyFinalDamageNum = 100;

    [SerializeField, Tooltip("数字のスプライト")] List<Sprite> damagesprite;
    [SerializeField, Tooltip("数字のスプライト")] GameObject damageobj;

    [SerializeField, Tooltip("ダメージ数を表示する時の生成範囲の最小")] float MinRange = -0.2f;
    [SerializeField, Tooltip("ダメージ数を表示する時の生成範囲の最小")] float MaxRange = 0.2f;

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
            Debug.LogError("ネットの箱が無いよ");
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
                Debug.Log("のけぞってるなう" + other.GetComponent<BossAI>().GetCurrentAction().actionName);

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
                    //当たったらダメージ数表示
                    if (parry.isTanuki)
                    {
                        GekiUI(other.transform);
                        // Debug.Log("ホストダメージ数");

                    }
                    else
                    {
                        isGeki = true;
                        bosspos = other.transform.position;
                        bossscale = other.transform.localScale;
                        hitdamege = DamageNum;
                        //Debug.Log("ダメージ数" + bosspos);

                    }
                    other.GetComponent<BossAI>().Nokezori--;
                    other.GetComponent<BossAI>().isInterrupted = true;
                    Debug.Log("のけぞってるなう" + other.GetComponent<BossAI>().Nokezori);
                    RPCCombo();
                    return;
                }
                if (raise.GetisRaise())
                {
                    Debug.Log("龍墜閃");
                    sharenum.jumpAttackNum++;

                    if (other.GetComponent<BossAI>().isAir)
                    {
                        other.GetComponent<BossAI>().isDown = true;
                    }
                }
                other.GetComponent<BossStatus>().RPC_Damage(DamageNum);

                //当たったらダメージ数表示
                if (parry.isTanuki)
                {
                    GekiUI(other.transform);
                   // Debug.Log("ホストダメージ数");

                }
                else
                {
                    isGeki = true;
                    bosspos = other.transform.position;
                    bossscale = other.transform.localScale;
                    hitdamege = DamageNum;
                    //Debug.Log("ダメージ数" + bosspos);

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
        Debug.Log("ああああああああああああああああああああああああああああああああああああああああああああああああああああ");
        combo.AddCombo();
    }

    public override void Render()
    {

        //ホストなら終了
        if (Runner.IsServer)
        {
            //Debug.Log("ダメージ数ホストだよ");
            return;
        }

        if (isGeki)
        {
            Debug.Log("クライアントダメージ数");
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
        Debug.Log("gggeeeダメージ数");
        DisplayNumber(hitdamege, pos);
    }

    public void DisplayNumber(int damage, Transform pos)
    {
        // ダメージ値を文字列として扱う
        string damageStr = damage.ToString();

        // ランダムなオフセットを計算
        float randomX = Random.Range(MinRange, MaxRange); // -0.2～0.2の間でX座標をランダム化
        float randomY = Random.Range(MinRange, MaxRange); // -0.2～0.2の間でY座標をランダム化

        // 各桁の数字を生成
        for (int i = 0; i < damageStr.Length; i++)
        {
            // 数字を取得
            int digit = int.Parse(damageStr[i].ToString());

            // 数字オブジェクトを生成
            GameObject numberObj = Instantiate(damageobj, new Vector3(0, 0, 0), Quaternion.identity);

            // スプライトを設定
            SpriteRenderer spriteRenderer = numberObj.GetComponent<SpriteRenderer>();
            spriteRenderer.sprite = damagesprite[digit];

            // 配置を調整（桁ごとに横に並べつつ、ランダムな位置にずらす）
            numberObj.transform.position = new Vector3(
                 pos.position.x + (i * 1f + randomX), // 桁ごとの配置にランダムなXオフセットを追加
                pos.position.y + (pos.localScale.y / 4), // Y座標にもランダムオフセットを追加
                 pos.position.z
            );
           // Debug.Log("ダメージ数" + numberObj.transform.position.y);

            // 数秒後に消えるように設定
            //Destroy(numberObj, 1.5f); // 1.5秒後にオブジェクトを削除
        }
    }


}
