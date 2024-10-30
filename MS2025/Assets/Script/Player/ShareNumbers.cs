using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShareNumbers : NetworkBehaviour
{
    [Networked] public int CurrentHP { get; set; }
    [Networked] public int nHitnum { get; set; }
    public int maxHitnum { get; set; }

    [Networked] public int nCombo { get; set; }

    [Networked] public bool isSpecial { get; set; }

    public GameObject Boss;

    [SerializeField] private GameObject[] HPUI;

    private bool isOnce = false;

    int magnification = 2;
    [SerializeField] int damage = 10;

    public void AddHitnum()
    {
        nHitnum++;
        if (nHitnum >= maxHitnum)
        {
            nHitnum = 0;
        }
        Debug.Log("連撃数:" + nHitnum);
    }

    /// <summary>
    /// ゲスト側に退出命令を送信
    /// </summary>
    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    public void RPC_Damage()
    {
        Debug.Log("子オブジェクト探索");

        // "MainGameUI" のオブジェクトが正しく取得できているか確認
        GameObject obj = GameObject.Find("MainGameUI");
        if (obj == null)
        {
            Debug.LogError("MainGameUI オブジェクトが見つかりません");
            return;
        }

        // GetComponentsInChildren で全ての階層の子オブジェクトを取得
        Transform[] allChildren = obj.GetComponentsInChildren<Transform>();

        // 子要素がいなければ終了
        if (allChildren.Length == 0)
        {
            Debug.LogError("MainGameUI の子要素がありません");
            return;
        }

        // HPUI 配列のサイズを全ての子オブジェクト数に合わせて初期化
        HPUI = new GameObject[5];

        int num = 0;
        foreach (Transform ob in allChildren)
        {
            if (ob.CompareTag("HPUI"))
            {
                Debug.Log("HPUI オブジェクト発見");
                HPUI[num] = ob.gameObject;
                num++;
            }
        }

        // HPUI が見つからなかった場合の対処
        if (num == 0)
        {
            Debug.LogError("HPUI が見つかりませんでした");
        }

        CurrentHP--;

        HPUI[CurrentHP].SetActive(false);
    }
    
    public void BossDamage()
    {        
        //倍率計算式
        magnification = (nCombo - 1) / 5 * 2 + 2;
        int totalDamage = magnification * damage;

        Debug.Log("必殺技ダメージ" + totalDamage);

        Boss.GetComponent<BossStatus>().RPC_Damage(totalDamage);

        nCombo = 0;

        isSpecial = false;
    }

    public void SpecialStart()
    {
        isSpecial = true;
    }

    public override void Spawned()
    {
        maxHitnum = 3;
        nHitnum = 0;
        CurrentHP =5;
        nCombo = 0;
        Debug.Log("プレイヤーのHPとか初期化");         
    }    
}
