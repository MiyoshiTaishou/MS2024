using Fusion;

public class Player : NetworkBehaviour
{
    private NetworkCharacterController characterController;
    public float HP = 10;

    private void Awake()
    {
        characterController = GetComponent<NetworkCharacterController>();
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
        }
    }
}