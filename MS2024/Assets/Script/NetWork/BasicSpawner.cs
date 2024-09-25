using System;
using System.Collections.Generic;
using System.Linq;
using Fusion;
using Fusion.Sockets;
using UnityEngine;

public class GameLauncher : MonoBehaviour, INetworkRunnerCallbacks
{
    [SerializeField]
    private NetworkRunner networkRunnerPrefab;

    private NetworkRunner networkRunner;

    [SerializeField]
    private NetworkPrefabRef playerAvatarPrefab;

    [SerializeField, Header("スポーンポジション")]
    private GameObject spawnPos;

    [SerializeField] private NetworkPrefabRef bossPrefab; // ボスのプレハブ

    [SerializeField, Header("オフラインにするかどうか")] private bool isLocal;

    [SerializeField, Header("キャラクター追従カメラ")] private GameObject cameraPrefab; // カメラのプレハブ

    [SerializeField, Header("キャラ追従カメラ")] private CinemaCharCamera charCamera;

    private async void Start()
    {
        // PlayerPrefsからルーム名を取得
        string roomName = PlayerPrefs.GetString("RoomName", "DefaultRoom"); // デフォルト値を指定

        Debug.Log("ルーム名: " + roomName);

        networkRunner = Instantiate(networkRunnerPrefab);
        // NetworkRunnerのコールバック対象に、このスクリプト（GameLauncher）を登録する
        networkRunner.AddCallbacks(this);

        if (!isLocal) // オンラインモードのとき
        {
            var result = await networkRunner.StartGame(new StartGameArgs
            {
                GameMode = GameMode.AutoHostOrClient,
                SessionName = roomName,
                SceneManager = networkRunner.GetComponent<NetworkSceneManagerDefault>()
            });

            if (result.Ok)
            {
                Debug.Log("オンラインモードでゲーム開始: " + roomName);
            }
            else
            {
                Debug.LogError("ゲーム開始に失敗: " + result.ShutdownReason);
            }
        }
        else // オフラインモードのとき
        {
            var result = await networkRunner.StartGame(new StartGameArgs
            {
                GameMode = GameMode.Single, // シングルプレイヤーモード
                SessionName = "LocalTestSession",  // ローカルテスト用セッション名
                SceneManager = networkRunner.GetComponent<NetworkSceneManagerDefault>()
            });

            if (result.Ok)
            {
                Debug.Log("オフラインモードでゲーム開始");
            }
            else
            {
                Debug.LogError("オフラインモード開始に失敗: " + result.ShutdownReason);
            }
        }
    }

    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
        Debug.Log($"Player Joined: {player}, Is Server: {runner.IsServer}");

        // ホスト（サーバー兼クライアント）かどうかはIsServerで判定できる
        if (!runner.IsServer) { return; }

        // ランダムな生成位置（半径5の円の内部）を取得する
        var randomValue = UnityEngine.Random.insideUnitCircle * 5f;
        var spawnPosition = new Vector3(randomValue.x, 5f, randomValue.y);

        if (isLocal)
        {
            spawnPosition = spawnPos.transform.position;
        }

        // プレイヤーのアバターを生成
        var avatar = runner.Spawn(playerAvatarPrefab, spawnPosition, Quaternion.identity, player);
        runner.SetPlayerObject(player, avatar);
        Debug.Log($"Spawned avatar for player {player}, at position {spawnPosition}");

        // アバターのNetworkObjectを取得して、HasInputAuthorityを確認
        var networkObject = avatar.GetComponent<NetworkObject>();
        Debug.Log($"NetworkObject HasInputAuthority: {networkObject.HasInputAuthority}");

        // ローカルプレイヤーのみカメラを生成
        //if (networkObject.HasInputAuthority)
        //{
        //    var playerCamera = Instantiate(cameraPrefab);
        //    charCamera = playerCamera.GetComponent<CinemaCharCamera>();
        //    charCamera.SetTarget(avatar.transform);
        //    Debug.Log("Camera assigned to local player.");
        //}

        // 現在のプレイヤー人数を取得
        int playerCount = runner.ActivePlayers.Count();

        // プレイヤーが2人になったらボスを召喚
        if (playerCount == 2)
        {
            Debug.Log("2 players joined. Summoning the boss!");

            // ボスの生成位置（例として固定位置）
            Vector3 bossSpawnPosition = new Vector3(0f, 5f, 0f);
            if (runner.IsServer)
            {
                runner.Spawn(bossPrefab, bossSpawnPosition, Quaternion.identity);
                Debug.Log("Boss spawned at position: " + bossSpawnPosition);
            }
        }
    }

    // セッションからプレイヤーが退出した時に呼ばれるコールバック
    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {
        Debug.Log($"Player Left: {player}");

        if (!runner.IsServer) { return; }

        // 退出したプレイヤーのアバターを破棄
        if (runner.TryGetPlayerObject(player, out var avatar))
        {
            runner.Despawn(avatar);
            Debug.Log("Despawned avatar for player: " + player);
        }
    }

    // 入力を収集する時に呼ばれるコールバック
    public void OnInput(NetworkRunner runner, NetworkInput input)
    {
        var data = new NetworkInputData();

        data.direction = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
        data.buttons.Set(NetworkInputButtons.Jump, Input.GetKey(KeyCode.Space));

        input.Set(data);
    }

    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input) { }
    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason) { }
    public void OnConnectedToServer(NetworkRunner runner) { }
    public void OnDisconnectedFromServer(NetworkRunner runner, NetDisconnectReason reason) { }
    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token) { }
    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason) { }
    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message) { }
    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList) { }
    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data) { }
    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken) { }
    public void OnSceneLoadDone(NetworkRunner runner) { }
    public void OnSceneLoadStart(NetworkRunner runner) { }
    public void OnObjectExitAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player) { }
    public void OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player) { }
    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ReliableKey key, ArraySegment<byte> data) { }
    public void OnReliableDataProgress(NetworkRunner runner, PlayerRef player, ReliableKey key, float progress) { }
}
