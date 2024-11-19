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
    [SerializeField] private string gameScene; // SceneRef �ɕύX
    [SerializeField] private int numBoss = 1;
    [SerializeField] Image LoadingImage;
    [SerializeField, Header("�g�����W�V�����I�u�W�F�N�g")] private GameObject[] transiton;
    [SerializeField, Header("�v���C���[�𐶐����Ȃ��V�[�����X�g")] private string[] skipScenes;

    private NetworkRunner networkRunner;

    // �{�^���������ăz�X�g�Ƃ��ăQ�[�����J�n����
    public void StartHost()
    {
        StartGame(GameMode.AutoHostOrClient, roomNameInputField.text);
    }public void StartDebug()
    {
        StartGame(GameMode.AutoHostOrClient, roomNameInputField.text);
    }

    // �{�^���������ăN���C�A���g�Ƃ��ăQ�[���ɎQ������
    public void StartClient()
    {
        StartGame(GameMode.Client, roomNameInputField.text);
    }

    // �Q�[�����J�n���A�V�[����J�ڂ��郁�\�b�h roomName�����|�~�ɂ��� �R���g���[���[�Ή�
    private async void StartGame(GameMode mode, string roomName)
    {
        networkRunner = FindObjectOfType<NetworkRunner>();
        if (networkRunner == null)
        {
            networkRunner = Instantiate(networkRunnerPrefab);
        }

        // ���̃X�N���v�g�ŃR�[���o�b�N�������ł���悤�ɂ���
        networkRunner.AddCallbacks(this);
        networkRunner.ProvideInput = true;

        //���[�f�B���O�̉摜���o��
        LoadingImage.gameObject.SetActive(true);

        //�g�����W�V�����Đ��J�n
        foreach (var tran in transiton)
        {
            tran.GetComponent<Animator>().SetTrigger("Start");
        }

        // �Q�[���Z�b�V�����̊J�n
        var result = await networkRunner.StartGame(new StartGameArgs
        {
            GameMode = mode,
            SessionName = roomName,
            SceneManager = networkRunner.GetComponent<NetworkSceneManagerDefault>()
        });

        if (result.Ok)
        {
            if (networkRunner.IsServer)
            {
                //// �K���V�[�������[�h����O�Ƀv���C���[�I�u�W�F�N�g�𐶐�
                //foreach (var player in networkRunner.ActivePlayers)
                //{
                //    if (!networkRunner.TryGetPlayerObject(player, out _))
                //    {
                //        var spawnPosition = new Vector3(0, 5f, 0);
                //        var playerObject = networkRunner.Spawn(playerAvatarPrefab, spawnPosition, Quaternion.identity, player);
                //        networkRunner.SetPlayerObject(player, playerObject);
                //    }
                //}

                // �V�[�����[�h
                networkRunner.LoadScene(gameScene);
            }
        }
    }

    // INetworkRunnerCallbacks�̎���
    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
        if (!runner.IsServer) { return; }

        var randomValue = UnityEngine.Random.insideUnitCircle * 2f;
        var spawnPosition = new Vector3(randomValue.x, 5f, 0f);

        // ���[�J���v���C���[�I�u�W�F�N�g�𐶐�
        var avatar = runner.Spawn(playerAvatarPrefab, spawnPosition, Quaternion.identity, player);

        // SetPlayerObject ��K���Ăяo��
        if (avatar != null)
        {
            runner.SetPlayerObject(player, avatar);
        }
        else
        {
            Debug.LogError("Failed to spawn player avatar!");
        }
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
        data.Buttons.Set(NetworkInputButtons.ChargeAttack, Input.GetButton("ChargeAttack"));
        input.Set(data);
    }

    // ���̃R�[���o�b�N�i������j
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
        // �V�[���X�L�b�v�Ώۂ̃`�F�b�N
        string currentSceneName = SceneManager.GetActiveScene().name;
        if (Array.Exists<string>(skipScenes, scene => currentSceneName.Contains(scene)))
        {
            Debug.Log($"Skipping player spawn for scene: {currentSceneName}");
            return;
        }

        if (runner.IsServer)
        {
            foreach (var player in runner.ActivePlayers)
            {
                // �v���C���[�I�u�W�F�N�g�����݂��Ȃ��ꍇ�ɂ̂ݐ���
                if (!runner.TryGetPlayerObject(player, out _))
                {
                    var randomValue = UnityEngine.Random.insideUnitCircle * 2f;
                    var spawnPosition = new Vector3(randomValue.x, 5f, 0f);

                    // �v���C���[�I�u�W�F�N�g����
                    var playerObject = runner.Spawn(playerAvatarPrefab, spawnPosition, Quaternion.identity, player);

                    // SetPlayerObject ��K���Ăяo��
                    if (playerObject != null)
                    {
                        runner.SetPlayerObject(player, playerObject);
                        Debug.Log($"Player object assigned for player {player.RawEncoded}");
                    }
                    else
                    {
                        Debug.LogError("Failed to spawn player object during OnSceneLoadDone.");
                    }
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
