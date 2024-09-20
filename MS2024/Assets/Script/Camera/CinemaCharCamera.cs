using Cinemachine;
using System.Collections;
using UnityEngine;

public class CinemaCharCamera : MonoBehaviour
{
    public CinemachineVirtualCamera virtualCamera;

    private CinemachineImpulseSource impulseSource;

    private float defaultFOV; // �f�t�H���g�̃J������FOV
    private Vector3 defaultPositionOffset; // �f�t�H���g�̈ʒu�I�t�Z�b�g
    private Transform defaultLookAt; // �f�t�H���g�� LookAt

    private void Start()
    {
        impulseSource = GetComponent<CinemachineImpulseSource>();

        // CinemachineTransposer �����݂���ꍇ�A�f�t�H���g�̃I�t�Z�b�g��ۑ�
        CinemachineTransposer transposer = virtualCamera.GetCinemachineComponent<CinemachineTransposer>();
        if (transposer != null)
        {
            defaultFOV = virtualCamera.m_Lens.FieldOfView;
            defaultPositionOffset = transposer.m_FollowOffset;
            defaultLookAt = virtualCamera.LookAt; // ������LookAt��ۑ�
        }
        else
        {
            Debug.LogError("CinemachineTransposer ��������܂���B�C���X�y�N�^�Őݒ肵�Ă��������B");
        }
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
    /// <param name="zoomTarget">�Y�[���̒��S�ƂȂ�I�u�W�F�N�g�� Transform</param>
    /// <param name="zoomAmount">�Y�[���̋����i����p���ǂꂾ�����߂邩�j</param>
    /// <param name="zoomDuration">�Y�[���C�����鎞��</param>
    public void CameraZoom(Transform zoomTarget, float zoomAmount, float zoomDuration)
    {
        StartCoroutine(ZoomInAndOutCoroutine(zoomTarget, zoomAmount, zoomDuration));
    }

    private IEnumerator ZoomInAndOutCoroutine(Transform zoomTarget, float zoomAmount, float zoomDuration)
    {
        // �Y�[������Ώۂ̃I�u�W�F�N�g��LookAt�ɐݒ�
        virtualCamera.LookAt = zoomTarget;

        // �J�����̎���p���Y�[���C���i�������j����
        virtualCamera.m_Lens.FieldOfView = defaultFOV - zoomAmount;

        // �w�肳�ꂽ���Ԃ����҂�
        yield return new WaitForSeconds(zoomDuration);

        // �J�����̎���p��LookAt�����ɖ߂�
        virtualCamera.m_Lens.FieldOfView = defaultFOV;
        virtualCamera.LookAt = defaultLookAt; // ����LookAt�ɖ߂�
    }
}
