using Cinemachine;
using System.Collections;
using UnityEngine;

public class CinemaCharCamera : MonoBehaviour
{
    public CinemachineVirtualCamera virtualCamera;

    private CinemachineImpulseSource impulseSource;

    private float defaultFOV; // デフォルトのカメラのFOV
    private Vector3 defaultPositionOffset; // デフォルトの位置オフセット
    private Transform defaultLookAt; // デフォルトの LookAt

    private void Start()
    {
        impulseSource = GetComponent<CinemachineImpulseSource>();

        // CinemachineTransposer が存在する場合、デフォルトのオフセットを保存
        CinemachineTransposer transposer = virtualCamera.GetCinemachineComponent<CinemachineTransposer>();
        if (transposer != null)
        {
            defaultFOV = virtualCamera.m_Lens.FieldOfView;
            defaultPositionOffset = transposer.m_FollowOffset;
            defaultLookAt = virtualCamera.LookAt; // 初期のLookAtを保存
        }
        else
        {
            Debug.LogError("CinemachineTransposer が見つかりません。インスペクタで設定してください。");
        }
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
    /// <param name="zoomTarget">ズームの中心となるオブジェクトの Transform</param>
    /// <param name="zoomAmount">ズームの強さ（視野角をどれだけ狭めるか）</param>
    /// <param name="zoomDuration">ズームインする時間</param>
    public void CameraZoom(Transform zoomTarget, float zoomAmount, float zoomDuration)
    {
        StartCoroutine(ZoomInAndOutCoroutine(zoomTarget, zoomAmount, zoomDuration));
    }

    private IEnumerator ZoomInAndOutCoroutine(Transform zoomTarget, float zoomAmount, float zoomDuration)
    {
        // ズームする対象のオブジェクトをLookAtに設定
        virtualCamera.LookAt = zoomTarget;

        // カメラの視野角をズームイン（小さく）する
        virtualCamera.m_Lens.FieldOfView = defaultFOV - zoomAmount;

        // 指定された時間だけ待つ
        yield return new WaitForSeconds(zoomDuration);

        // カメラの視野角とLookAtを元に戻す
        virtualCamera.m_Lens.FieldOfView = defaultFOV;
        virtualCamera.LookAt = defaultLookAt; // 元のLookAtに戻す
    }
}
