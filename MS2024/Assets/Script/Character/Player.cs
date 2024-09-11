using Fusion;

public class Player : NetworkBehaviour
{
    private NetworkCharacterController characterController;

    private void Awake()
    {
        characterController = GetComponent<NetworkCharacterController>();
    }

    public override void FixedUpdateNetwork()
    {
        if (GetInput(out NetworkInputData data))
        {
            // “ü—Í•ûŒü‚ÌƒxƒNƒgƒ‹‚ğ³‹K‰»‚·‚é
            data.direction.Normalize();
            // “ü—Í•ûŒü‚ğˆÚ“®•ûŒü‚Æ‚µ‚Ä‚»‚Ì‚Ü‚Ü“n‚·
            characterController.Move(data.direction);

            if (data.buttons.IsSet(NetworkInputButtons.Jump))
            {
                characterController.Jump();
            }
        }
    }
}