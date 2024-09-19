using UnityEngine.InputSystem;
using UnityEngine;
using Fusion;

public class PlayerJump : MonoBehaviour
{
    InputAction jumpAction=null;

    public void OnJump(InputAction.CallbackContext context)
    {
        if(context.started)
        {
            GetComponent<NetworkCharacterController>().Jump();
            Debug.Log("�W�����v���܂���aa");


        }
    }
    // Start is called before the first frame update
    void Start()
    {
        var pinput = GetComponent<PlayerInput>();
        var actionMap = pinput.actions;
        jumpAction = actionMap["Jump"];
        if (jumpAction == null)
        {
            Debug.Log("�Ȃ��悶����");

        }
    }
        // Update is called once per frame
        void Update()
    {
        if (jumpAction != null && jumpAction.triggered)
        {
            // �{�^�����u�����ꂽ�Ƃ��v�ɂ����W�����v�����s����
            if (jumpAction.ReadValue<float>() > 0) // �{�^����������Ă��鎞�̒l�͒ʏ� 1.0f
            {
                GetComponent<NetworkCharacterController>().Jump();
            }
        }

    }
}
