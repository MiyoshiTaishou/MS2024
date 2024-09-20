using UnityEngine;
using Fusion;

public class PlayerMove : NetworkBehaviour
{
    private Rigidbody rb;

    [SerializeField, Header("�����x")]
    private float acceleration = 10f; // �����x
    [SerializeField, Header("�ő呬�x")]
    private float maxSpeed = 5f; // �ő呬�x

    private Vector3 currentVelocity;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody>();
        }

        // Rigidbody�̐ݒ�
        rb.constraints = RigidbodyConstraints.FreezeRotation; // ��]���Œ�
        //rb.useGravity = false; // �d�͂��g��Ȃ��ꍇ
    }

    void Update()
    {
       
    }

    public override void FixedUpdateNetwork()
    {
        Debug.Log("�N�������������I");
        // �l�b�g���[�N�C���v�b�g�f�[�^���󂯎��v�Z����
        if (GetInput(out NetworkInputData data))
        {
            // ���͕����̃x�N�g���𐳋K������
            data.direction.Normalize();

            // �����̌v�Z���s��
            Vector3 targetVelocity = data.direction * maxSpeed;
            currentVelocity = Vector3.MoveTowards(currentVelocity, targetVelocity, acceleration * Time.deltaTime);

            // �����I�Ȉړ����s��
            rb.velocity = currentVelocity;
        }
    }
}
