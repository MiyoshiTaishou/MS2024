using Fusion;
using UnityEngine;

public class GameLauncher : MonoBehaviour
{
    [SerializeField]
    private NetworkRunner networkRunnerPrefab;

    [SerializeField]
    private NetworkPrefabRef playerAvatarPrefab;

    private NetworkRunner networkRunner;

   private async void Start()
    {
        networkRunner = Instantiate(networkRunnerPrefab);
        var result = await networkRunner.StartGame(new StartGameArgs
        {
            GameMode = GameMode.Shared,
            SceneManager = networkRunner.GetComponent<NetworkSceneManagerDefault>()
        });

        if (result.Ok)
        {
            // ランダムな生成位置（半径5の円の内部）を取得する
            var randomValue = Random.insideUnitCircle * 5f;
            var spawnPosition = new Vector3(randomValue.x, 5f, randomValue.y);
            // プレイヤー自身のアバターを生成する
            networkRunner.Spawn(playerAvatarPrefab, spawnPosition, Quaternion.identity, networkRunner.LocalPlayer);
        }
        else
        {
            Debug.Log("失敗！");            
        }
    }
}