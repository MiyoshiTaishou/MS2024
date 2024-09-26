using UnityEngine;
using Cinemachine;
using Fusion;

public class PlayerController : NetworkBehaviour
{
    public GameObject cinemachineCameraPrefab;
    private CinemachineVirtualCamera virtualCamera;

    public override void Spawned()
    {
        // ���[�J���v���C���[�̂݃J�����𐶐�
        if (Object.HasInputAuthority)
        {
            SetupCamera();
        }
    }

    private void SetupCamera()
    {
        // �J�����̃C���X�^���X�𐶐�
        GameObject cameraInstance = Instantiate(cinemachineCameraPrefab);

        // CinemachineVirtualCamera���擾���A�v���C���[���^�[�Q�b�g�ɐݒ�
        virtualCamera = cameraInstance.GetComponent<CinemachineVirtualCamera>();
        virtualCamera.Follow = transform;  // �v���C���[�ɒǏ]
        virtualCamera.LookAt = transform;  // �v���C���[�𒍎�
    }
}
