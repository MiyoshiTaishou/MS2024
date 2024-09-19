using Fusion;
using UnityEngine;

public class Player : NetworkBehaviour
{
    private NetworkCharacterController characterController;
    private Quaternion initialRotation;  // Å‰‚Ì‰ñ“]
    Camera MainCamera;

    public float HP = 10;
    private void Awake()
    {
        characterController = GetComponent<NetworkCharacterController>();
        initialRotation = transform.rotation;  // ‰Šú‚Ì‰ñ“]‚ğ•Û‘¶
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
            // “ü—Í•ûŒü‚ÌƒxƒNƒgƒ‹‚ğ³‹K‰»‚·‚é
            data.direction.Normalize();
            // “ü—Í•ûŒü‚ğˆÚ“®•ûŒü‚Æ‚µ‚Ä‚»‚Ì‚Ü‚Ü“n‚·
            characterController.Move(data.direction);

            if (data.buttons.IsSet(NetworkInputButtons.Jump))
            {
                characterController.Jump();
            }

            // ƒvƒŒƒCƒ„[‚Ì‰ñ“]‚ğŒÅ’è
            transform.rotation = initialRotation;
        }
    }
}
