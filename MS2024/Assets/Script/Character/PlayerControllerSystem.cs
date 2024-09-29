using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControllerSystem : MonoBehaviour
{
    private Vector2 moveInput;
    public float moveSpeed = 5f;

    void Update()
    {
        // 2Dの入力（X, Y）を受け取るが、Y方向はZ方向に変換
        moveInput = GetComponent<PlayerInput>().actions["Move"].ReadValue<Vector2>();

        // 移動する方向を3Dに適用 (X はそのまま、Y の値を Z に置き換える)
        Vector3 move = new Vector3(moveInput.x, 0, moveInput.y);

        // プレイヤーを移動させる
        transform.Translate(move * Time.deltaTime * moveSpeed, Space.World);
    }
}
