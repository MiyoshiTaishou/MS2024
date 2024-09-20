using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CinemaCharCamera : MonoBehaviour
{
    public CinemachineVirtualCamera virtualCamera;

    private Cinemachine.CinemachineImpulseSource impulseSource;

    private float defaultFOV; // デフォルトのカメラのFOV
    private Vector3 defaultPositionOffset; // デフォルトの位置オフセット

    private void Start()
    {
        impulseSource = GetComponent<Cinemachine.CinemachineImpulseSource>();
    }

    public void SetTarget(Transform targetTransform)
    {
        virtualCamera.Follow = targetTransform;
        virtualCamera.LookAt = targetTransform;
    }

    /// <summary>
    /// カメラを揺らす処理を呼ぶ
    /// </summary>
    public void CameraImpulse()
    {
        impulseSource.GenerateImpulse();
    }

    /// <summary>
    /// カメラを一時的にズームインして戻す処理
    /// </summary>
    /// <param name="zoomAmount">ズームの強さ（視野角をどれだけ狭めるか）</param>
    /// <param name="offset">画面中央からのオフセット</param>
    /// <param name="zoomDuration">ズームインする時間</param>
    public void CameraZoom(Vector2 offset, float zoomAmount, float zoomDuration)
    {
        StartCoroutine(ZoomInAndOutCoroutine(offset, zoomAmount, zoomDuration));
    }

    private IEnumerator ZoomInAndOutCoroutine(Vector2 offset, float zoomAmount, float zoomDuration)
    {
        // CinemachineTransposerを使ってカメラの追従位置を操作
        CinemachineTransposer transposer = virtualCamera.GetCinemachineComponent<CinemachineTransposer>();

        // オフセットの設定
        Vector3 newOffset = new Vector3(offset.x, offset.y, transposer.m_FollowOffset.z);
        transposer.m_FollowOffset = newOffset;

        // カメラの視野角をズームイン（小さく）する
        virtualCamera.m_Lens.FieldOfView = defaultFOV - zoomAmount;

        // 指定された時間だけ待つ
        yield return new WaitForSeconds(zoomDuration);

        // カメラの視野角とオフセットを元に戻す
        virtualCamera.m_Lens.FieldOfView = defaultFOV;
        transposer.m_FollowOffset = defaultPositionOffset;
    }
}
