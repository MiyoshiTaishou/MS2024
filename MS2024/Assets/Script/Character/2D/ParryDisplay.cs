using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class ParryDisplay : MonoBehaviour
{
    // 計測用のタイマー
    private float timer = 0.0f;

    PlayerState player;

    private void Start()
    {
        player = transform.parent.GetComponent<PlayerState>();
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

        Debug.Log(timer);

        // 指定した秒数を超えたらオブジェクトを非表示にする
        if (timer >= player.ParryActivetime / 60)
        {
            timer = 0.0f;
            gameObject.SetActive(false);
           
        }
    }
}
