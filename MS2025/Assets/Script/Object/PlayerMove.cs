using UnityEngine;
using Fusion;

public class PlayerMove : NetworkBehaviour
{
    private Rigidbody rb;
    Animator animator;

    [SerializeField, Header("�����x")]
    private float acceleration = 10f; // �����x
    [SerializeField, Header("�ő呬�x")]
    private float maxSpeed = 5f; // �ő呬�x

    private Vector3 currentVelocity;

    private void Awake()
    {
        animator = GetComponent<Animator>();

        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody>();
        }

        // Rigidbody�̐ݒ�
        rb.constraints = RigidbodyConstraints.FreezeRotation; // ��]���Œ�
        //rb.useGravity = false; // �d�͂��g��Ȃ��ꍇ
    }

    public override void FixedUpdateNetwork()
    {
        // �l�b�g���[�N�C���v�b�g�f�[�^���󂯎��v�Z����
        if (GetInput(out NetworkInputData data))
        {
            // ���͕����̃x�N�g���𐳋K������
            data.Direction.Normalize();

            // Y���̑��x�͌��݂�Rigidbody��Y�����x��ێ�����
            float currentYVelocity = rb.velocity.y;

            // �����̌v�Z���s���iX��Z���̂݌v�Z�j
            Vector3 targetVelocity = new Vector3(data.Direction.x * maxSpeed, currentYVelocity, data.Direction.z * maxSpeed);
            currentVelocity = Vector3.MoveTowards(new Vector3(rb.velocity.x, 0, rb.velocity.z), targetVelocity, acceleration * Time.deltaTime);

            // Y�����x���ێ����Ȃ���AX��Z���̈ړ��𔽉f������
            currentVelocity.y = currentYVelocity;

            // �����I�Ȉړ����s��
            rb.velocity = currentVelocity;
        }
    }

    public override void Render()
    {
        if(rb.velocity.x != 0.0f)
        {
            animator.SetBool("IsMove", true);
        }
        else
        {
            animator.SetBool("IsMove", false);
        }
    }
}
