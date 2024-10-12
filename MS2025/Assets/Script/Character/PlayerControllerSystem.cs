using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControllerSystem : MonoBehaviour
{
    private Vector2 moveInput;
    public float moveSpeed = 5f;

    void Update()
    {
        // 2D�̓��́iX, Y�j���󂯎�邪�AY������Z�����ɕϊ�
        moveInput = GetComponent<PlayerInput>().actions["Move"].ReadValue<Vector2>();

        // �ړ����������3D�ɓK�p (X �͂��̂܂܁AY �̒l�� Z �ɒu��������)
        Vector3 move = new Vector3(moveInput.x, 0, moveInput.y);

        // �v���C���[���ړ�������
        transform.Translate(move * Time.deltaTime * moveSpeed, Space.World);
    }
}
