using UnityEngine;
using Fusion;
using Unity.VisualScripting;
using UnityEngine.UIElements;

public class PlayerMove : NetworkBehaviour
{
    private Rigidbody rb;
    Animator animator;

    [SerializeField, Header("加速度")]
    private float acceleration = 10f; // 加速度
    [SerializeField, Header("最大速度")]
    private float maxSpeed = 5f; // 最大速度

    private Vector3 currentVelocity;

    private Vector3 scale;

    bool isReflection = false;


    private void Awake()
    {
        animator = GetComponent<Animator>();

        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody>();
        }

        // Rigidbodyの設定
        rb.constraints = RigidbodyConstraints.FreezeRotation; // 回転を固定
        //rb.useGravity = false; // 重力を使わない場合
        scale = transform.localScale;
    }

    public override void FixedUpdateNetwork()
    {
        // ネットワークインプットデータを受け取り計算する
        if (GetInput(out NetworkInputData data))
        {
            // 入力方向のベクトルを正規化する
            data.Direction.Normalize();

            // Y軸の速度は現在のRigidbodyのY軸速度を保持する
            float currentYVelocity = rb.velocity.y;

            // 加速の計算を行う（XとZ軸のみ計算）
            Vector3 targetVelocity = new Vector3(data.Direction.x * maxSpeed, currentYVelocity, data.Direction.z * maxSpeed);
            currentVelocity = Vector3.MoveTowards(new Vector3(rb.velocity.x, 0, rb.velocity.z), targetVelocity, acceleration * Time.deltaTime);

            // Y軸速度を維持しながら、XとZ軸の移動を反映させる
            currentVelocity.y = currentYVelocity;

            Vector3 nomaldata = Vector3.Normalize(data.Direction);
            Vector3 nomalvel = Vector3.Normalize(currentVelocity);
            float speed = Vector3.Magnitude(currentVelocity);
            currentVelocity.x = nomaldata.x*speed;
            currentVelocity.z = nomaldata.z*speed;


            if ((data.Direction.x > 0.0f && currentVelocity.x < 0.0f)|| (data.Direction.x < 0.0f && currentVelocity.x > 0.0f))
            {
                currentVelocity.x = -currentVelocity.x;
            }
            if ((data.Direction.z > 0.0f && currentVelocity.z < 0.0f) || (data.Direction.z < 0.0f && currentVelocity.z > 0.0f))
            {
                currentVelocity.z = -currentVelocity.z;
            }

            // 物理的な移動を行う
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
