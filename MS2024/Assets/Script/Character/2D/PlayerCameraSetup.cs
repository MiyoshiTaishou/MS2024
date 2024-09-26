using UnityEngine;
using Cinemachine;
using Fusion;

public class PlayerController : NetworkBehaviour
{
    public GameObject cinemachineCameraPrefab;
    private CinemachineVirtualCamera virtualCamera;

    public override void Spawned()
    {
        // ローカルプレイヤーのみカメラを生成
        if (Object.HasInputAuthority)
        {
            SetupCamera();
        }
    }

    private void SetupCamera()
    {
        // カメラのインスタンスを生成
        GameObject cameraInstance = Instantiate(cinemachineCameraPrefab);

        // CinemachineVirtualCameraを取得し、プレイヤーをターゲットに設定
        virtualCamera = cameraInstance.GetComponent<CinemachineVirtualCamera>();
        virtualCamera.Follow = transform;  // プレイヤーに追従
        virtualCamera.LookAt = transform;  // プレイヤーを注視
    }
}
