using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParryDisplay : MonoBehaviour
{
    // 計測用のタイマー
    private float timer = 0.0f;

    PlayerParry player;

    private void Start()
    {
        player = transform.parent.GetComponent<PlayerParry>();
        Init();
    }

    public void Init()
    {
        timer = 0.0f;
        gameObject.SetActive(false);
    }

    void Update()
    {
        // 時間を計測
        timer += Time.deltaTime;

        // 指定した秒数を超えたらオブジェクトを非表示にする
        if (timer >= player.GetParryActiveTime())
        {
            timer = 0.0f;
            player.SetParryflg(false);
            gameObject.SetActive(false);
           
        }
    }
}
