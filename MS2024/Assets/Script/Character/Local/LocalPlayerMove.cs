using UnityEngine.InputSystem;
using UnityEngine;

public class LocalPlayerMove : MonoBehaviour
{
    Rigidbody rb;
    [SerializeField,Tooltip("加速度")]float speedconf;
    [SerializeField, Tooltip("最大速度")] float MaxSpeed;

    Vector2 vec;
    public void OnMove(InputAction.CallbackContext context)
    {
        vec = context.ReadValue<Vector2>();
        if(context.ReadValue<Vector2>().x<0)
        {
            GetComponent<LocalPlayerAttack>().SetLeftAttack(true);
        }
        else 
        {
            GetComponent<LocalPlayerAttack>().SetLeftAttack(false);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {

        rb.AddForce(vec.x * speedconf, 0.0f, vec.y * speedconf);
        Vector3 vel = rb.velocity;
        if(vel.magnitude > MaxSpeed) //最大速度以上にならないように調整
        {
            Vector3 nomal = vel.normalized;
            float gap = vel.magnitude - MaxSpeed;
            nomal *= gap;
            vel.x -= nomal.x;
            vel.z -= nomal.z;
            rb.velocity = vel;
        }
    }
}
