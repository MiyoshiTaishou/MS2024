using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class GekiDisplay : NetworkBehaviour
{
    // 計測用のタイマー
    [SerializeField,ReadOnly] private float timer = 0.0f;

    [SerializeField] private float Limit = 10.0f;

    


    public override void FixedUpdateNetwork()
    {
        Debug.Log("撃げきgeki");

        // 時間を計測
        timer += Time.deltaTime;

        // 指定した秒数を超えたらオブジェクトを非表示にする
        if (timer >= Limit)
        {
            Destroy(gameObject);

        }
    }
}
