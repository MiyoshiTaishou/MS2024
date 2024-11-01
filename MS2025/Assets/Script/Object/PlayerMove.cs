using UnityEngine;
using Fusion;
using Unity.VisualScripting;
using UnityEngine.UIElements;

public class PlayerMove : NetworkBehaviour
{
    private Rigidbody rb;
    Animator animator;

    [SerializeField, Header("�����x")]
    private float acceleration = 10f; // �����x
    [SerializeField, Header("�ő呬�x")]
    private float maxSpeed = 5f; // �ő呬�x

    private Vector3 currentVelocity;

    private Vector3 scale;

    bool isReflection = false;

    GameObject comboCountObject;

    [Networked]
    public bool isMove { get; set; }

    public override void Spawned()
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
        scale = transform.localScale;

        comboCountObject = GameObject.Find("Networkbox");
    }
  
    public override void FixedUpdateNetwork()
    {
        
        if(comboCountObject.GetComponent<ShareNumbers>().isSpecial)
        {
            return;
        }
        AnimatorStateInfo landAnimStateInfo = GetComponent<Animator>().GetCurrentAnimatorStateInfo(0);

        //�p���B���͓������Ȃ��悤�ɂ���
        if (landAnimStateInfo.IsName("APlayerParry") || landAnimStateInfo.IsName("APlayerCounter") || landAnimStateInfo.IsName("APlayerAttack")
            || landAnimStateInfo.IsName("APlayerAttack2")|| landAnimStateInfo.IsName("APlayerAttack3"))
        {
            rb.velocity = new Vector3(0, rb.velocity.y, 0);
            return;
        }

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

            Vector3 nomaldata = Vector3.Normalize(data.Direction);
            Vector3 nomalvel = Vector3.Normalize(currentVelocity);
            float speed = Vector3.Magnitude(currentVelocity);
            currentVelocity.x = nomaldata.x*speed;
            currentVelocity.z = nomaldata.z * speed;
            
            if(Mathf.Abs(currentVelocity.x) > maxSpeed)
            {
                currentVelocity.x = (currentVelocity.x>0)?maxSpeed:-maxSpeed;
            }
            if(Mathf.Abs(currentVelocity.z) > maxSpeed)
            {
                currentVelocity.z = (currentVelocity.z > 0) ? maxSpeed : -maxSpeed;
            }

            if ((data.Direction.x > 0.0f && currentVelocity.x < 0.0f)|| (data.Direction.x < 0.0f && currentVelocity.x > 0.0f))
            {
                currentVelocity.x = -currentVelocity.x;
            }
            if ((data.Direction.z > 0.0f && currentVelocity.z < 0.0f) || (data.Direction.z < 0.0f && currentVelocity.z > 0.0f))
            {
                currentVelocity.z = -currentVelocity.z;
            }

            // �����I�Ȉړ����s��
            rb.velocity = currentVelocity;
            if (data.Direction.x > 0.0f)
            {
                isReflection = false;
            }
            else if(data.Direction.x < 0.0f)
            {
                isReflection = true;
            }
        }
    }

    public override void Render()
    {
        if (isReflection ==false)
        {
            animator.SetBool("IsMove", true);
            transform.localScale = scale;
        }
        else if(isReflection == true)
        {
            animator.SetBool("IsMove", true);

            Vector3 temp = scale;
            temp.x = -scale.x;
            transform.localScale = temp;
        }
        else
        {
            animator.SetBool("IsMove", false);
        }
    }
}
