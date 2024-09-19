using Fusion;
using Fusion.Sockets;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;  // Input System API

public class PlayerInputNet : MonoBehaviour, INetworkRunnerCallbacks
{
    // PlayerInput��ێ�
    private PlayerInput playerInput;

    // ���̃��\�b�h���g�p����PlayerInput�R���|�[�l���g���擾���܂�
    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
    }

    public void OnInput(NetworkRunner runner, NetworkInput input)
    {
        var data = new NetworkInputData();

        // PlayerInput�̒l���擾 (move��2D�x�N�g��)
        Vector2 moveInput = playerInput.actions["Move"].ReadValue<Vector2>();
        bool jumpInput = playerInput.actions["Jump"].WasPressedThisFrame();

        // NetworkInputData�ɓ��͂�ݒ�
        data.direction = moveInput;
        data.buttons.Set(NetworkInputButtons.Jump, jumpInput);

        Debug.Log("���͂����f�[�^" + data);

        // �l�b�g���[�N���͂Ƃ��ăZ�b�g
        input.Set(data);
    }

    // �K�v�ȃR�[���o�b�N���\�b�h��ǉ��i���g�p�Ȃ��̎����ł�OK�j
    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player) { }
    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player) { }
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
    public void OnSceneLoadDone(NetworkRunner runner) { }
    public void OnSceneLoadStart(NetworkRunner runner) { }
    public void OnObjectExitAOI(NetworkRunner runner, NetworkObject obj) { }
    public void OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj) { }
    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ReliableKey key, System.ArraySegment<byte> data) { }
    public void OnReliableDataProgress(NetworkRunner runner, PlayerRef player, ReliableKey key, float progress) { }

    public void OnObjectExitAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
    {
        throw new System.NotImplementedException();
    }

    public void OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
    {
        throw new System.NotImplementedException();
    }

    public void OnDisconnectedFromServer(NetworkRunner runner, NetDisconnectReason reason)
    {
        throw new System.NotImplementedException();
    }
}
