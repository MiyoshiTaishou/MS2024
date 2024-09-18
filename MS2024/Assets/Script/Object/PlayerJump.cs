using UnityEngine.InputSystem;
using UnityEngine;
using Fusion;

public class PlayerJump : MonoBehaviour
{
    InputAction jumpAction=null;

    public void OnJump(InputAction.CallbackContext context)
    {
        GetComponent<NetworkCharacterController>().Jump();
        Debug.Log("ジャンプしましたaa");

    }
    // Start is called before the first frame update
    void Start()
    {
        var pinput = GetComponent<PlayerInput>();
        var actionMap = pinput.actions;
        jumpAction = actionMap["Jump"];
        if (jumpAction == null)
        {
            Debug.Log("ないよじゃんぷ");

        }
    }
        // Update is called once per frame
        void Update()
    {
        if (jumpAction != null && jumpAction.triggered)
        {
            // ボタンが「押されたとき」にだけジャンプを実行する
            if (jumpAction.ReadValue<float>() > 0) // ボタンが押されている時の値は通常 1.0f
            {
                GetComponent<NetworkCharacterController>().Jump();
            }
        }
    }
}
