using Fusion;
using Fusion.Sockets;
using UnityEngine;

public class GameLauncher : MonoBehaviour
{
    [SerializeField]
    private NetworkRunner networkRunnerPrefab;

    [SerializeField]
    private NetworkPrefabRef playerAvatarPrefab;

    [SerializeField]
    private string ipAddress = "127.0.0.1"; // 接続するサーバーのIPアドレス

    [SerializeField]
    private ushort port = 27015; // 接続するポート番号

    private NetworkRunner networkRunner;

    private async void Start()
    {
        networkRunner = Instantiate(networkRunnerPrefab);

        // LANでゲームをホストまたはクライアントとして開始する
        var startArgs = new StartGameArgs
        {
            GameMode = GameMode.Host,  // ホストとして開始、クライアントとして接続する場合は GameMode.Client
            Address = NetAddress.CreateFromIpPort(ipAddress, port),
            SceneManager = networkRunner.GetComponent<NetworkSceneManagerDefault>()
        };

        var result = await networkRunner.StartGame(startArgs);

        if (result.Ok)
        {
            if (networkRunner.IsServer) // サーバーの場合
            {
                // ランダムな生成位置（半径5の円の内部）を取得する
                var randomValue = Random.insideUnitCircle * 5f;
                var spawnPosition = new Vector3(randomValue.x, 5f, randomValue.y);
                // プレイヤー自身のアバターを生成する
                networkRunner.Spawn(playerAvatarPrefab, spawnPosition, Quaternion.identity, networkRunner.LocalPlayer);
            }
        }
        else
        {
            Debug.LogError("接続失敗！");
        }
    }
}
