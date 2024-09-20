using UnityEngine;
using Fusion;

public class PlayerMove : NetworkBehaviour
{
    private Rigidbody rb;

    [SerializeField, Header("加速度")]
    private float acceleration = 10f; // 加速度
    [SerializeField, Header("最大速度")]
    private float maxSpeed = 5f; // 最大速度

    private Vector3 currentVelocity;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody>();
        }

        // Rigidbodyの設定
        rb.constraints = RigidbodyConstraints.FreezeRotation; // 回転を固定
        //rb.useGravity = false; // 重力を使わない場合
    }

    void Update()
    {
       
    }

    public override void FixedUpdateNetwork()
    {
        Debug.Log("クララが立った！");
        // ネットワークインプットデータを受け取り計算する
        if (GetInput(out NetworkInputData data))
        {
            // 入力方向のベクトルを正規化する
            data.direction.Normalize();

            // 加速の計算を行う
            Vector3 targetVelocity = data.direction * maxSpeed;
            currentVelocity = Vector3.MoveTowards(currentVelocity, targetVelocity, acceleration * Time.deltaTime);

            // 物理的な移動を行う
            rb.velocity = currentVelocity;
        }
    }
}
