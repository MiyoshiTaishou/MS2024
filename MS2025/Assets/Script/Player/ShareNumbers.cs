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

    [SerializeField] private GameObject[] HPUI;

    private bool isOnce = false;

    public void AddHitnum()
    {
        nHitnum++;
        if (nHitnum >= maxHitnum)
        {
            nHitnum = 0;
        }
        Debug.Log("連撃数:" + nHitnum);
    }



    public override void Spawned()
    {
        maxHitnum = 3;
        nHitnum = 0;
        CurrentHP =5;
        nCombo = 0;
        Debug.Log("プレイヤーのHPとか初期化");         
    }

    public override void FixedUpdateNetwork()
    {
        if (!isOnce)
        {
            isOnce = true;

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
            HPUI = new GameObject[allChildren.Length];

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
        }

        switch (CurrentHP)
        {
            case 0:
                HPUI[CurrentHP].SetActive(false);
                break;
            case 1:
                HPUI[CurrentHP].SetActive(false);
                break;
            case 2:
                HPUI[CurrentHP].SetActive(false);
                break;
            case 3:
                HPUI[CurrentHP].SetActive(false);
                break;
            case 4:
                HPUI[CurrentHP].SetActive(false);
                break;
        }
    }

}
