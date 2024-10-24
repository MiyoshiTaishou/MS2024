using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class ParryDisplayNet : NetworkBehaviour
{
    // 計測用のタイマー
    private float timer = 0.0f;

    [SerializeField] PlayerParryNet player;

    public bool Hit {  get; private set; } =false;

    public override void Spawned()
    {
        Debug.Log("パリィエリア");

        player = transform.parent.GetComponent<PlayerParryNet>();
        Init();
    }

    public void Init()
    {
        timer = 0.0f;
        gameObject.SetActive(false);
    }

    public override void FixedUpdateNetwork()
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

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Enemy")
        {
            Debug.Log("ひっと");
            Hit = true;
        }

    }
}
