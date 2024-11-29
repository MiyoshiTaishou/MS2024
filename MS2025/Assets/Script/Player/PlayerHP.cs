using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHP : NetworkBehaviour
{
    GameObject box;
    [Networked]public bool isDamage { get; set; }


    public override void Spawned()
    {
        box = GameObject.Find("Networkbox");
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    public void RPC_DamageAnim()
    {
        GetComponent<Animator>().Play("APlayerHurt");
        GetComponent<PlayerFreeze>().Freeze(120);
        GetComponent<PlayerDamageReceived>().DamageReceived();
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
