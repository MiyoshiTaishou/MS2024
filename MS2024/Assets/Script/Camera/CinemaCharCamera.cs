using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
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

        // CinemachineTransposer �����݂���ꍇ�A�f�t�H���g�̃I�t�Z�b�g��ۑ�
        CinemachineTransposer transposer = virtualCamera.GetCinemachineComponent<CinemachineTransposer>();
        if (transposer != null)
        {
            defaultFOV = virtualCamera.m_Lens.FieldOfView;
            defaultPositionOffset = transposer.m_FollowOffset;
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
