using Fusion;
using UnityEngine;

public class Player : NetworkBehaviour
{
    private NetworkCharacterController characterController;
    private Quaternion initialRotation;  // �ŏ��̉�]
    Camera MainCamera;

    public float HP = 10;
    private void Awake()
    {
        characterController = GetComponent<NetworkCharacterController>();
        initialRotation = transform.rotation;  // �����̉�]��ۑ�
    }

    private void Start()
    {
        MainCamera = Camera.main;
        MainCamera.GetComponent<CameraMove>().player = this.transform;
    }

    public override void FixedUpdateNetwork()
    {
        if (GetInput(out NetworkInputData data))
        {
            // ���͕����̃x�N�g���𐳋K������
            data.direction.Normalize();
            // ���͕������ړ������Ƃ��Ă��̂܂ܓn��
            characterController.Move(data.direction);

            if (data.buttons.IsSet(NetworkInputButtons.Jump))
            {
                characterController.Jump();
            }

            // �v���C���[�̉�]���Œ�
            transform.rotation = initialRotation;
        }
    }
}
