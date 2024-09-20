using Fusion;
using UnityEngine;

public class Player : NetworkBehaviour
{
    private NetworkCharacterController characterController;
    private Quaternion initialRotation;  // 最初の回転

    [Tooltip("プレイヤーのタイトルを決めます")]
    public float HP = 10;
    private void Awake()
    {
        characterController = GetComponent<NetworkCharacterController>();
        initialRotation = transform.rotation;  // 初期の回転を保存
    }

    public override void FixedUpdateNetwork()
    {
        //if (GetInput(out NetworkInputData data))
        //{
        //    // 入力方向のベクトルを正規化する
        //    data.direction.Normalize();
        //    // 入力方向を移動方向としてそのまま渡す
        //    characterController.Move(data.direction);

        //    if (data.buttons.IsSet(NetworkInputButtons.Jump))
        //    {
        //        characterController.Jump();
        //    }

        //    // プレイヤーの回転を固定
        //    transform.rotation = initialRotation;
        //}
    }
}
