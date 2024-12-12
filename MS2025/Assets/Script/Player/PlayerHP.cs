using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHP : NetworkBehaviour
{
    GameObject box;
    [Networked]public bool isDamage { get; set; }
    [SerializeField,Header("硬直フレーム")]int damageFrame;
    [SerializeField, Header("ノックバック距離")] float knockbackDistance;
    [SerializeField, Header("減衰ノックバック距離")] float knockbackSlowDistance;
    int frame4_3;
    int frame4_1;
    int Count;
    GameObject boss;
    [Networked] public int inbisibleFrame { get; set; }
    SpriteRenderer sprite;
    
    public override void Spawned()
    {
        inbisibleFrame = 0;
        box = GameObject.Find("Networkbox");
        boss = GameObject.Find("Boss2D");
        if(!boss)
        {
            Debug.LogError("ボスないよ");
        }
        sprite=GetComponent<SpriteRenderer>();
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    public void RPC_DamageAnim()
    {
        if (inbisibleFrame == 0)
        {
            Count = damageFrame;
            inbisibleFrame = damageFrame * 3;
            isDamage = true;
            GetComponent<PlayerAnimChange>().RPC_InitAction("APlayerHurt");
            GetComponent<PlayerFreeze>().Freeze(damageFrame);
            GetComponent<PlayerDamageReceived>().DamageReceived();
            frame4_3 = (damageFrame / 4) * 3;
            frame4_1 = damageFrame / 4;
        }
    }

    public override void FixedUpdateNetwork()
    {
        if(inbisibleFrame!=0)
        {
            inbisibleFrame--;
            if (inbisibleFrame%4==0) 
            {
                Color color = sprite.color;
                color.a = 0.0f;
                sprite.color = color;
            }
            else if (inbisibleFrame % 4 == 0)
            {
                Color color = sprite.color;
                color.a = 0.5f;
                sprite.color = color;
            }
            else 
            {
                Color color = sprite.color;
                color.a = 1.0f;
                sprite.color = color;
            }
        }
        if(isDamage)
        {
            Vector3 bosspos=boss.transform.position;
            Vector3 pos= transform.position;
            frame4_1 = damageFrame / 4;
            frame4_3 = frame4_1* 3;
            bool isRight = pos.x < bosspos.x ? true : false;
            float knockback=0;
            if ((frame4_3+frame4_1>Count)&&(frame4_1<Count))
            {
                knockback = knockbackDistance / frame4_3;
            }
            else
            {
                knockback = knockbackSlowDistance/frame4_1;
            }
            pos.x += isRight ? -knockback : knockback;
            transform.position = pos;
            Count--;
        }
        if(!GetComponent<PlayerFreeze>().GetIsFreeze()) 
        {
            Count = 0;
            isDamage = false;
        }
    }

    /// <summary>
    /// ゲスト側に退出命令を送信
    /// </summary>
    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    public void RPC_ExitGameForGuests()
    {
        if (!Object.HasStateAuthority)
        {
            // ゲストがルームを退出してシーンを変更する
            Runner.Shutdown();
            UnityEngine.SceneManagement.SceneManager.LoadScene("MenuScene");
        }
        else
        {
            // ホストは少し待ってから自身の処理を実行
            StartCoroutine(HandleHostShutdown());
        }
    }

    /// <summary>
    /// ホストが終了する処理
    /// </summary>
    private IEnumerator HandleHostShutdown()
    {
        // ゲストが退出するのを待つ（1秒程度の遅延を入れる）
        yield return new WaitForSeconds(1.0f);

        // ホストが退出してシーンを変更する
        Runner.Shutdown();
        UnityEngine.SceneManagement.SceneManager.LoadScene("MenuScene");
    }
}
