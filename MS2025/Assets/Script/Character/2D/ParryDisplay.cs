using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class ParryDisplay : MonoBehaviour
{
    // 計測用のタイマー
    private float timer = 0.0f;

    PlayerParry player;

    public bool Hit {  get; private set; } =false;

    private void Start()
    {
        player = transform.parent.GetComponent<PlayerParry>();
        //Init();
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
