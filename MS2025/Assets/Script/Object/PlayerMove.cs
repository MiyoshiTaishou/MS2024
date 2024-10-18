using UnityEngine;
using Fusion;

public class PlayerMove : NetworkBehaviour
{
    private Rigidbody rb;
    Animator animator;

    [SerializeField, Header("加速度")]
    private float acceleration = 10f; // 加速度
    [SerializeField, Header("最大速度")]
    private float maxSpeed = 5f; // 最大速度

    private Vector3 currentVelocity;

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

            // 物理的な移動を行う
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
