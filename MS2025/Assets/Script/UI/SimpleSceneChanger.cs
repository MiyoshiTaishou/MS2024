using Fusion;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SimpleSceneChanger : NetworkBehaviour
{
    [Header("設定")]
    public string targetScene; // 切り替えるシーン名
    public bool itIsPossibleToChangeScene;
    public bool useAButtonInput = true; // Submitで反応するかどうか
    public bool useBButtonInput = false; // Cancelで反応するかどうか

    void Update() {
        // itIsPossibleToChangeSceneがfalseならシーンチェンジしない
        if (!itIsPossibleToChangeScene) return;

        // Submitボタンでシーン変更
        if (useAButtonInput && Input.GetButtonDown("Submit")) {
            RPC_ClientSceneTransition();
        }

        // Cancelボタンでシーン変更
        if (useBButtonInput && Input.GetButtonDown("Cancel")) {
            RPC_ClientSceneTransition();
        }
    }

    // シーン変更を実行する
    private void ChangeScene() {
        if (!string.IsNullOrEmpty(targetScene)) {
            SceneManager.LoadScene(targetScene);
        }
        else {
            Debug.LogError("ターゲットシーンが設定されていません。");
        }
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    private void RPC_ClientSceneTransition()
    {
        // クライアントは先にシーン遷移を実行
        if (!Object.HasStateAuthority)
        {
            SceneManager.LoadScene(targetScene);
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
        SceneManager.LoadScene(targetScene);
    }
}
