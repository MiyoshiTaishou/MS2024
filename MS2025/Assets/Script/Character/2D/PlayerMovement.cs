using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5.0f; // �ړ����x��ݒ�

    void Update()
    {
        // �L�[�{�[�h���͂��擾
        float horizontal = Input.GetAxis("Horizontal"); // "A"��"D"�L�[�ō��E�E
        float vertical = Input.GetAxis("Vertical"); // "W"��"S"�L�[�őO�E��

        // ���͂Ɋ�Â��Ĉړ��x�N�g�����쐬
        Vector3 movement = new Vector3(horizontal, 0, vertical) * speed * Time.deltaTime;

        // �I�u�W�F�N�g���ړ�
        transform.Translate(movement, Space.World);
    }
}
