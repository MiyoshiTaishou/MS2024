using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHP : NetworkBehaviour
{
    [Networked, SerializeField]
    private int nPlayerHP { get; set; }

    /// <summary>
    /// ダメージ処理
    /// </summary>
    /// <param name="_damage"></param>
    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void RPC_Damage(int _damage)
    {
        nPlayerHP -= _damage;

        // HPが0以下なら死亡処理を呼ぶ
        if (nPlayerHP <= 0)
        {
            if (Object.HasStateAuthority)
            {
                // まずゲストに退出命令を送信する
                RPC_ExitGameForGuests();
            }
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
