using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using TMPro;
using UnityEngine.SceneManagement;
using Fusion.Sockets;
using System;
using UnityEngine.UI;
using System.Linq;

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
    [SerializeField, Header("開始人数")] private int playerNum;
    [SerializeField, Header("キャラ画像")] private GameObject[] charobj;

    private NetworkRunner networkRunner;

    private void Update()
    {
        // XBoxのAボタン（Submitに割り当てられている）を押したら接続を切る
        if (Input.GetButtonDown("Cancel"))
        {
            DisconnectFromServer();
        }
    }

    // 接続を切るメソッド
    private void DisconnectFromServer()
    {
        if (networkRunner != null)
        {
            Debug.Log("Disconnecting from server...");

            foreach (var obj in charobj)
            {
                obj.SetActive(false);
            }

            networkRunner.Shutdown(); // 接続を切る

            //ローディングの画像を出す
            LoadingImage.gameObject.SetActive(false);

            //トランジション再生開始
            foreach (var tran in transiton)
            {
                tran.GetComponent<Animator>().SetTrigger("Reverse");
            }

        }
        else
        {
            Debug.LogWarning("No active NetworkRunner instance to disconnect.");
        }
    }


    // ボタンを押してホストとしてゲームを開始する
    public void StartHost(string roomName) {
        StartGame(GameMode.AutoHostOrClient, roomName);
    }

    public void StartDebug() {
        StartGame(GameMode.AutoHostOrClient, roomNameInputField.text);
    }

    // ボタンを押してクライアントとしてゲームに参加する
    public void StartClient() {
        StartGame(GameMode.Client, roomNameInputField.text);
    }

    // ゲームを開始し、シーンを遷移するメソッド roomNameを松竹梅にする コントローラー対応
    private async void StartGame(GameMode mode, string roomName) {
        networkRunner = FindObjectOfType<NetworkRunner>();
        if (networkRunner == null) {
            networkRunner = Instantiate(networkRunnerPrefab);           
        }

        // このスクリプトでコールバックを処理できるようにする
        networkRunner.AddCallbacks(this);
        networkRunner.ProvideInput = true;

        //ローディングの画像を出す
        LoadingImage.gameObject.SetActive(true);

        //トランジション再生開始
        foreach (var tran in transiton) {
            tran.GetComponent<Animator>().SetTrigger("Start");
        }

        // ゲームセッションの開始
        var result = await networkRunner.StartGame(new StartGameArgs {
            GameMode = mode,
            SessionName = roomName,
            SceneManager = networkRunner.GetComponent<NetworkSceneManagerDefault>()
        });

        if (result.Ok) {
            if (networkRunner.IsServer) {
               if(networkRunner.ActivePlayers.Count() == 3)
                {
                    foreach (var obj in charobj)
                    {
                        obj.SetActive(true);
                    }
                    DisconnectFromServer();
                }
            }
        }
    }

    // INetworkRunnerCallbacksの実装
    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player) {
        //if (!runner.IsServer) { return; }

        //var randomValue = UnityEngine.Random.insideUnitCircle * 2f;
        //var spawnPosition = new Vector3(randomValue.x, 5f, 0f);

        //// ローカルプレイヤーオブジェクトを生成
        //var avatar = runner.Spawn(playerAvatarPrefab, spawnPosition, Quaternion.identity, player);

        //// SetPlayerObject を必ず呼び出す
        //if (avatar != null) {
        //    runner.SetPlayerObject(player, avatar);
        //}
        //else {
        //    Debug.LogError("Failed to spawn player avatar!");
        //}

        // プレイヤー数が 2 人以上になったらシーンをロード
        if (runner.ActivePlayers.Count() == playerNum)
        {           
            if (runner.IsServer)
            {
                networkRunner.LoadScene(gameScene);
            }
        }
    }

    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player) {
        if (!runner.IsServer) { return; }
        if (runner.TryGetPlayerObject(player, out var avatar)) {
            runner.Despawn(avatar);
        }
    }

    public void OnInput(NetworkRunner runner, NetworkInput input) {
        var data = new NetworkInputData();
        data.Direction = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
        data.Buttons.Set(NetworkInputButtons.Attack, Input.GetButton("Attack"));
        data.Buttons.Set(NetworkInputButtons.Jump, Input.GetButton("Jump"));
        data.Buttons.Set(NetworkInputButtons.Parry, Input.GetButton("Parry"));
        data.Buttons.Set(NetworkInputButtons.Special, Input.GetButton("SpecialAttack"));
        data.Buttons.Set(NetworkInputButtons.ChargeAttack, Input.GetButton("ChargeAttack"));
        input.Set(data);
    }

    // 他のコールバック（空実装）
    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input) {}
    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason) {}
    public void OnConnectedToServer(NetworkRunner runner) {}
    public void OnDisconnectedFromServer(NetworkRunner runner) {}
    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token) {}
    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason) {}
    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message) {}
    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList) {}
    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data) {}
    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken) {}
    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ArraySegment<byte> data) {}
    public void OnSceneLoadDone(NetworkRunner runner) {
        // シーンスキップ対象のチェック
        string currentSceneName = SceneManager.GetActiveScene().name;
        if (Array.Exists<string>(skipScenes, scene => currentSceneName.Contains(scene))) {
            Debug.Log($"Skipping player spawn for scene: {currentSceneName}");
            return;
        }

        if (runner.IsServer) {
            foreach (var player in runner.ActivePlayers) {
                // プレイヤーオブジェクトが存在しない場合にのみ生成
                if (!runner.TryGetPlayerObject(player, out _)) {
                    var randomValue = UnityEngine.Random.insideUnitCircle * 2f;
                    var spawnPosition = new Vector3(randomValue.x, 5f, 0f);

                    // プレイヤーオブジェクト生成
                    var playerObject = runner.Spawn(playerAvatarPrefab, spawnPosition, Quaternion.identity, player);

                    // SetPlayerObject を必ず呼び出す
                    if (playerObject != null) {
                        runner.SetPlayerObject(player, playerObject);
                        Debug.Log($"Player object assigned for player {player.RawEncoded}");
                    }
                    else {
                        Debug.LogError("Failed to spawn player object during OnSceneLoadDone.");
                    }
                }
            }
        }
    }

    public void OnSceneLoadStart(NetworkRunner runner) {}
    public void OnObjectExitAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player) {}
    public void OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player) {}
    public void OnDisconnectedFromServer(NetworkRunner runner, NetDisconnectReason reason) {}
    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ReliableKey key, ArraySegment<byte> data) {}
    public void OnReliableDataProgress(NetworkRunner runner, PlayerRef player, ReliableKey key, float progress) {}
}
