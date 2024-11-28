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

    [Networked] public bool Hit {  get; private set; } =false;
    [Networked] public bool isRaaise { get; private set; } = false;
    GameObject obj;
    public override void Spawned()
    {

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
            Hit = false;
            player.SetParryflg(false);
            gameObject.SetActive(false);
           
        }
    }

    public override void Render()
    {
        if(isRaaise) 
        {
            player.isRaise= true;
            isRaaise= false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Enemy")
        {
            Debug.Log("ひっと");
            Hit = true;
        }
        if(other.GetComponent<PlayerRaise>())
        {
            if(other.GetComponent<PlayerRaise>().GetisRaise() == false && other.GetComponent<PlayerJumpNet>().GetisJumping())
            {
                isRaaise= true;
            }
        }

    }
}
