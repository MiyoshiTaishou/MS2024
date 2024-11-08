using Fusion;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuLoad : NetworkBehaviour
{
    [Header("メニューシーン名"), SerializeField]
    private String ResultSceneName;

    public void LoadMenu()
    {
        if (Object.HasStateAuthority)
        {
            // クライアントに先にシーン遷移を指示
            RPC_ClientSceneTransition();
        }
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    private void RPC_ClientSceneTransition()
    {
        // クライアントは先にシーン遷移を実行
        if (!Object.HasStateAuthority)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(ResultSceneName);
        }
        else
        {
            // ホスト側はクライアントの遷移が完了した後にシーン遷移
            StartCoroutine(HostSceneTransition());
        }
    }

    private IEnumerator HostSceneTransition()
    {
        yield return new WaitForSeconds(2); // クライアント側がシーン遷移するまでの時間を調整
        Runner.Shutdown();
        UnityEngine.SceneManagement.SceneManager.LoadScene(ResultSceneName);
    }
}
