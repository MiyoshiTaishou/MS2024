using Fusion;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class ChargeAttackAreaDamage : NetworkBehaviour
{
    GameObject player;
    [SerializeField] GameObject netobj;
    PlayerChargeAttack attack;
    ShareNumbers sharenum;
    ComboSystem combo;
    [SerializeField, Tooltip("ヒットストップ時間(f)")] int stopFrame;
    [SerializeField] int ChargeDamege = 500;
    [SerializeField] GameObject Gekiobj;
    NetworkRunner runner;

    [SerializeField, Tooltip("数字のスプライト")] List<Sprite> damagesprite;
    [SerializeField, Tooltip("数字のスプライト")] GameObject damageobj;

    [SerializeField, Tooltip("ダメージ数を表示する時の生成範囲の最小")] float MinRange = -0.2f;
    [SerializeField, Tooltip("ダメージ数を表示する時の生成範囲の最小")] float MaxRange = 0.2f;

    [Networked] Vector3 bosspos { get; set; }
    [Networked] Vector3 bossscale { get; set; }


    [Networked] bool isGeki { get; set; } = false;

    PlayerParryNet parry;
    GameObject change;
    private int Tutorial=0;

    [Networked] bool ishitstop { get; set; } = false;
    int Count = 0;
    public override void Spawned()
    {
        change = GameObject.Find("ChangeAction");
        player = transform.parent.gameObject;
        attack = player.GetComponent<PlayerChargeAttack>();
        netobj = GameObject.Find("Networkbox");
        if (netobj == null)
        {
            Debug.LogError("ネットの箱が無いよ");
        }

        if (SceneManager.GetActiveScene().name == "TutorialScene_Miyoshi" || SceneManager.GetActiveScene().name == "TutorialScene_Build")
        {
            Tutorial = 1;
        }
        sharenum = netobj.GetComponent<ShareNumbers>();
        combo = netobj.GetComponent<ComboSystem>();
        runner = GameObject.Find("Runner(Clone)").GetComponent<NetworkRunner>();
        parry = transform.parent.GetComponent<PlayerParryNet>();

    }

    private void OnTriggerEnter(Collider other)
    {
        if (Object.HasStateAuthority)
        {
            if (other.GetComponent<BossStatus>())
            {
                Debug.Log("チャージアタック成功"+ other.transform);
                //other.GetComponent<BossAI>().isNokezoriTanuki = parry.isTanuki;
                other.GetComponent<BossStatus>().RPC_Damage(ChargeDamege);
                switch(Tutorial)
                {
                    case 1:
                        if (change.GetComponent<ChangeBossAction>().TextNo == 4)
                        {
                            change.GetComponent<ChangeBossAction>().TextNo = 5;
                        }
                        break;
                }
              
                Camera.main.GetComponent<CameraEffectPlay>().RPC_CameraEffect();
                Camera.main.GetComponent<CameraShake>().RPC_CameraShake(0.3f, 0.3f);


                //当たった位置に撃表示
                //当たった位置に撃表示
                if (parry.isTanuki)
                {
                    DisplayNumber(ChargeDamege, other.transform);
                    Debug.Log("ホストダメージ数");
                    player.GetComponent<HitStop>().ApplyHitStop(stopFrame);

                }
                else
                {
                    isGeki = true;
                    bosspos = other.transform.position;
                    bossscale = other.transform.localScale;

                    //ヒットストップ
                    ishitstop = true;

                }
                RPCCombo();
                //Debug.Log("ダメージ数" + other.GetComponent<BossAI>().isAir);

                if (!other.GetComponent<BossAI>().isAir)
                {
                   // Debug.Log("のけぞりダメージ数");

                    other.GetComponent<BossAI>().RPC_AnimNameRegist();
                }

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
        if (Count > 0)
        {
            Count--;
        }
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
            boss.position = bosspos;
            GekiUI(boss);
            //boss = null;
            isGeki = false;

            gameObject.SetActive(false);
        }

        if(ishitstop)
        {
            player.GetComponent<HitStop>().ApplyHitStop(stopFrame);
            ishitstop = false;
        }

    }


    public void GekiUI(Transform pos)
    {
        DisplayNumber(ChargeDamege, pos);
    }

    public void DisplayNumber(int damage, Transform pos)
    {
        if (Count > 0)
        {
            return;
        }
        Count = 1;

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
            Debug.Log("ダメージ数" + numberObj.transform.position.y);

            // 数秒後に消えるように設定
            //Destroy(numberObj, 1.5f); // 1.5秒後にオブジェクトを削除
        }
    }
}
