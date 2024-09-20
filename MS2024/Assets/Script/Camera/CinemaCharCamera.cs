using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CinemaCharCamera : MonoBehaviour
{
    public CinemachineVirtualCamera virtualCamera;

    private Cinemachine.CinemachineImpulseSource impulseSource;

    private float defaultFOV; // �f�t�H���g�̃J������FOV
    private Vector3 defaultPositionOffset; // �f�t�H���g�̈ʒu�I�t�Z�b�g

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
    /// �J������h�炷�������Ă�
    /// </summary>
    public void CameraImpulse()
    {
        impulseSource.GenerateImpulse();
    }

    /// <summary>
    /// �J�������ꎞ�I�ɃY�[���C�����Ė߂�����
    /// </summary>
    /// <param name="zoomAmount">�Y�[���̋����i����p���ǂꂾ�����߂邩�j</param>
    /// <param name="offset">��ʒ�������̃I�t�Z�b�g</param>
    /// <param name="zoomDuration">�Y�[���C�����鎞��</param>
    public void CameraZoom(Vector2 offset, float zoomAmount, float zoomDuration)
    {
        StartCoroutine(ZoomInAndOutCoroutine(offset, zoomAmount, zoomDuration));
    }

    private IEnumerator ZoomInAndOutCoroutine(Vector2 offset, float zoomAmount, float zoomDuration)
    {
        // CinemachineTransposer���g���ăJ�����̒Ǐ]�ʒu�𑀍�
        CinemachineTransposer transposer = virtualCamera.GetCinemachineComponent<CinemachineTransposer>();

        // �I�t�Z�b�g�̐ݒ�
        Vector3 newOffset = new Vector3(offset.x, offset.y, transposer.m_FollowOffset.z);
        transposer.m_FollowOffset = newOffset;

        // �J�����̎���p���Y�[���C���i�������j����
        virtualCamera.m_Lens.FieldOfView = defaultFOV - zoomAmount;

        // �w�肳�ꂽ���Ԃ����҂�
        yield return new WaitForSeconds(zoomDuration);

        // �J�����̎���p�ƃI�t�Z�b�g�����ɖ߂�
        virtualCamera.m_Lens.FieldOfView = defaultFOV;
        transposer.m_FollowOffset = defaultPositionOffset;
    }
}
