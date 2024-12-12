using UnityEngine;
using Fusion;

public class UIController : NetworkBehaviour
{
    [SerializeField] private GameObject hostUI; // ホスト用のUI
    [SerializeField] private GameObject clientUI; // クライアント用のUI
    private NetworkRunner runner;

    [SerializeField, Header("開始時に表示するかどうか")]
    private bool isStart = true;

    public override void Spawned()
    {
        // NetworkRunnerのインスタンスを取得
        runner = NetworkRunner.FindObjectOfType<NetworkRunner>();

        if (!isStart)
        {
            return;
        }

        // ホスト/クライアントを判定してUIを表示
        if (runner != null)
        {          
            if (runner.IsServer)
            {
                ShowHostUI();
            }
            else if (runner.IsClient)
            {
                ShowClientUI();
            }
        }
    }

    [Rpc(RpcSources.All, RpcTargets.All)]
    public void RPC_ShowUI()
    {
        // ホスト/クライアントを判定してUIを表示
        if (runner != null)
        {
            if (runner.IsServer)
            {
                ShowHostUI();
            }
            else if (runner.IsClient)
            {
                ShowClientUI();
            }
        }
    }
    
    private void ShowHostUI()
    {
        hostUI.SetActive(true);
        clientUI.SetActive(false);
    }

    private void ShowClientUI()
    {
        hostUI.SetActive(false);
        clientUI.SetActive(true);
    }
}
