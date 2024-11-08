using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using TMPro;
using UnityEngine.SceneManagement;
using Fusion.Sockets;
using System;
using UnityEngine.UI;

public class GameLauncher : MonoBehaviour, INetworkRunnerCallbacks
{
    [SerializeField] private NetworkRunner networkRunnerPrefab;
    [SerializeField] private NetworkPrefabRef playerAvatarPrefab;
    [SerializeField] private NetworkPrefabRef bossAvatarPrefab;
    [SerializeField] private NetworkPrefabRef PlayerStatePrefab;
    [SerializeField] private InputField roomNameInputField;
    [SerializeField] private string gameScene; // SceneRef に変更
    [SerializeField] private int numBoss = 1;
    [SerializeField] Image LoadingImage;
    [SerializeField, Header("トランジションオブジェクト")] private GameObject[] transiton;
    [SerializeField, Header("プレイヤーを生成しないシーンリスト")] private string[] skipScenes;

    private NetworkRunner networkRunner;

    // ボタンを押してホストとしてゲームを開始する
    public void StartHost()
    {
        StartGame(GameMode.AutoHostOrClient, roomNameInputField.text);
    }

    // ボタンを押してクライアントとしてゲームに参加する
    public void StartClient()
    {
        StartGame(GameMode.Client, roomNameInputField.text);
    }

    // ゲームを開始し、シーンを遷移するメソッド
    private async void StartGame(GameMode mode, string roomName)
    {
        networkRunner = FindObjectOfType<NetworkRunner>();
        if (networkRunner == null)
        {
            networkRunner = Instantiate(networkRunnerPrefab);
        }

        // このスクリプトでコールバックを処理できるようにする
        networkRunner.AddCallbacks(this);
        networkRunner.ProvideInput = true;

        //ローディングの画像を出す
        LoadingImage.gameObject.SetActive(true);

        //トランジション再生開始
        foreach (var tran in transiton)
        {
            tran.GetComponent<Animator>().SetTrigger("Start");
        }

        // ゲームセッションの開始
        var result = await networkRunner.StartGame(new StartGameArgs
        {
            GameMode = mode,
            SessionName = roomName,
            SceneManager = networkRunner.GetComponent<NetworkSceneManagerDefault>()
        });

        if (result.Ok)
        {
            // ホストならゲームシーンに遷移
            if (networkRunner.IsServer)
            {
                var loadSceneParams = new NetworkLoadSceneParameters
                {
                    // 必要に応じてパラメータを設定
                };

                // SceneRef を使ってシーンをロード
                networkRunner.LoadScene(gameScene);
            }
        }
        else
        {
            Debug.LogError("Failed to start game: " + result.ShutdownReason);
        }
    }

    // INetworkRunnerCallbacksの実装
    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
        if (!runner.IsServer) { return; }

        if (runner.SessionInfo.PlayerCount == 2)
        {
            var randomValue = UnityEngine.Random.insideUnitCircle * 2f;
            var spawnPosition = new Vector3(randomValue.x, 5f, 0f);

            var avatar = runner.Spawn(playerAvatarPrefab, spawnPosition, Quaternion.identity, player);
            runner.SetPlayerObject(player, avatar);
        }

        //if (runner.SessionInfo.PlayerCount == 1)
        //{
        //    var avatar2 = runner.Spawn(PlayerStatePrefab, spawnPosition, Quaternion.identity, player);
        //}     

        //if (runner.SessionInfo.PlayerCount == numBoss)
        //{
        //    var spawnBossPosition = new Vector3(0f, 7f, 0f);
        //    runner.Spawn(bossAvatarPrefab, spawnBossPosition, Quaternion.identity, player);            
        //}
    }

    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {
        if (!runner.IsServer) { return; }
        if (runner.TryGetPlayerObject(player, out var avatar))
        {
            runner.Despawn(avatar);
        }
    }

    public void OnInput(NetworkRunner runner, NetworkInput input)
    {
        var data = new NetworkInputData();
        data.Direction = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
        data.Buttons.Set(NetworkInputButtons.Attack, Input.GetButton("Attack"));
        data.Buttons.Set(NetworkInputButtons.Jump, Input.GetButton("Jump"));
        data.Buttons.Set(NetworkInputButtons.Parry, Input.GetButton("Parry"));
        data.Buttons.Set(NetworkInputButtons.Special, Input.GetButton("SpecialAttack"));
        input.Set(data);
    }

    // 他のコールバック（空実装）
    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input) { }
    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason) { }
    public void OnConnectedToServer(NetworkRunner runner) { }
    public void OnDisconnectedFromServer(NetworkRunner runner) { }
    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token) { }
    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason) { }
    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message) { }
    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList) { }
    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data) { }
    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken) { }
    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ArraySegment<byte> data) { }
    public void OnSceneLoadDone(NetworkRunner runner)
    {
        // 現在のシーン名を取得
        string currentSceneName = SceneManager.GetActiveScene().name;

        // シーンがスキップ対象の場合はプレイヤー生成処理を行わない
        if (Array.Exists<string>(skipScenes, scene => currentSceneName.Contains(scene)))
        {
            Debug.Log($"Skipping player spawn for scene: {currentSceneName}");
            return;
        }

        if (runner.IsServer)
        {
            // 必要に応じてプレイヤーオブジェクトを再生成
            foreach (var player in runner.ActivePlayers)
            {
                if (!runner.TryGetPlayerObject(player, out _))
                {
                    var randomValue = UnityEngine.Random.insideUnitCircle * 2f;
                    var spawnPosition = new Vector3(randomValue.x, 5f, 0f);
                    var playerObject = runner.Spawn(playerAvatarPrefab, spawnPosition, Quaternion.identity, player);
                    runner.SetPlayerObject(player, playerObject);
                }
            }
        }
    }
    public void OnSceneLoadStart(NetworkRunner runner) { }

    public void OnObjectExitAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
    {
       
    }

    public void OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
    {
       
    }

    public void OnDisconnectedFromServer(NetworkRunner runner, NetDisconnectReason reason)
    {
       
    }

    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ReliableKey key, ArraySegment<byte> data)
    {
       
    }

    public void OnReliableDataProgress(NetworkRunner runner, PlayerRef player, ReliableKey key, float progress)
    {
       
    }
}
