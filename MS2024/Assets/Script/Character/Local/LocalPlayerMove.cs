using UnityEngine.InputSystem;
using UnityEngine;

public class LocalPlayerMove : MonoBehaviour
{
    Rigidbody rb;
    [SerializeField,Tooltip("加速度")]float speedconf;
    [SerializeField, Tooltip("最大速度")] float MaxSpeed;

    public void OnMove(InputAction.CallbackContext context)
    {
        Vector2 vec = context.ReadValue<Vector2>();
        rb.AddForce(vec.x, 0.0f, vec.y);
    }
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 vel = rb.velocity;
        if(vel.magnitude > MaxSpeed) //最大速度以上にならないように調整
        {
            Vector3 nomal = vel.normalized;
            float gap = vel.magnitude - MaxSpeed;
            nomal *= gap;
            vel -= nomal;
            rb.velocity = vel;
        }
    }
}
