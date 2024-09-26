using System;
using System.Collections.Generic;
using System.Linq;
using Fusion;
using Fusion.Sockets;
using UnityEngine;

// INetworkRunnerCallbacks���������āANetworkRunner�̃R�[���o�b�N���������s�ł���悤�ɂ���
public class GameLauncher : MonoBehaviour, INetworkRunnerCallbacks
{
    [SerializeField]
    private NetworkRunner networkRunnerPrefab;  

    private NetworkRunner networkRunner;

    [SerializeField]
    private NetworkPrefabRef playerAvatarPrefab;

    [SerializeField, Header("�X�|�[���|�W�V����")]
    private GameObject spawnPos;

    [SerializeField] private NetworkPrefabRef bossPrefab; // �{�X�̃v���n�u

    [SerializeField, Header("�I�t���C���ɂ��邩�ǂ���")] private bool isLocal;

    [SerializeField,Header("�L�����N�^�[�Ǐ]�J����")] private GameObject cameraPrefab; // �J�����̃v���n�u

    [SerializeField, Header("�L�����Ǐ]�J����")] private CinemaCharCamera charCamera;

    private async void Start()
    {
        // PlayerPrefs���烋�[�������擾
        string roomName = PlayerPrefs.GetString("RoomName", "DefaultRoom"); // �f�t�H���g�l���w��

        Debug.Log("���[����" + roomName);

        networkRunner = Instantiate(networkRunnerPrefab);
        // NetworkRunner�̃R�[���o�b�N�ΏۂɁA���̃X�N���v�g�iGameLauncher�j��o�^����
        networkRunner.AddCallbacks(this);
        if (!isLocal) // �I�����C�����[�h�̂Ƃ�
        {
            var result = await networkRunner.StartGame(new StartGameArgs
            {
                GameMode = GameMode.AutoHostOrClient,
                SessionName = roomName,
                SceneManager = networkRunner.GetComponent<NetworkSceneManagerDefault>()
            });

            if (result.Ok)
            {
                Debug.Log("�I�����C�����[�h�ŃQ�[���J�n: " + roomName);
            }
            else
            {
                Debug.LogError("�Q�[���J�n�Ɏ��s: " + result.ShutdownReason);
            }
        }
        else // �I�t���C�����[�h�̂Ƃ�
        {
            var result = await networkRunner.StartGame(new StartGameArgs
            {
                GameMode = GameMode.Single, // �V���O���v���C���[���[�h
                SessionName = "LocalTestSession",  // ���[�J���e�X�g�p�Z�b�V������
                SceneManager = networkRunner.GetComponent<NetworkSceneManagerDefault>()
            });

            if (result.Ok)
            {
                Debug.Log("�I�t���C�����[�h�ŃQ�[���J�n");
            }
            else
            {
                Debug.LogError("�I�t���C�����[�h�J�n�Ɏ��s: " + result.ShutdownReason);
            }
        }
    }

    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
        // �z�X�g�i�T�[�o�[���N���C�A���g�j���ǂ�����IsServer�Ŕ���ł���
        if (!runner.IsServer) { return; }
        // �����_���Ȑ����ʒu�i���a5�̉~�̓����j���擾����
        var randomValue = UnityEngine.Random.insideUnitCircle * 5f;
        var spawnPosition = new Vector3(randomValue.x, 5f, randomValue.y);

        if(isLocal)
        {
            spawnPosition = spawnPos.transform.position;
        }

        // �Q�������v���C���[�̃A�o�^�[�𐶐�����
        var avatar = runner.Spawn(playerAvatarPrefab, spawnPosition, Quaternion.identity, player);
        // �v���C���[�iPlayerRef�j�ƃA�o�^�[�iNetworkObject�j���֘A�t����
        runner.SetPlayerObject(player, avatar);

        // avatar����NetworkObject���擾���āAHasInputAuthority���m�F����
        var networkObject = avatar.GetComponent<NetworkObject>();
        //if (networkObject.HasInputAuthority)  // ���[�J���v���C���[�̂݃J�����𐶐�����
        //{
        //    var playerCamera = Instantiate(cameraPrefab);

        //    // �J�����̃^�[�Q�b�g���v���C���[�ɐݒ�
        //    charCamera = playerCamera.GetComponent<CinemaCharCamera>();
        //    charCamera.SetTarget(avatar.transform);
        //}

        // ���݂̃v���C���[�l�����擾
        int playerCount = runner.ActivePlayers.Count();

        // �v���C���[��2�l�ɂȂ�����{�X������
        if (playerCount == 2)
        {
            Debug.Log("2 players joined. Summoning the boss!");

            // �{�X�̐����ʒu�i��Ƃ��ČŒ�ʒu�j
            Vector3 bossSpawnPosition = new Vector3(0f, 5f, 0f);
            runner.Spawn(bossPrefab, bossSpawnPosition, Quaternion.identity);                  
        }        
    }

    // �Z�b�V��������v���C���[���ޏo�������ɌĂ΂��R�[���o�b�N
    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {
        if (!runner.IsServer) { return; }
        // �ޏo�����v���C���[�̃A�o�^�[��j������
        if (runner.TryGetPlayerObject(player, out var avatar))
        {
            runner.Despawn(avatar);
        }
    }

    // ���͂����W���鎞�ɌĂ΂��R�[���o�b�N
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