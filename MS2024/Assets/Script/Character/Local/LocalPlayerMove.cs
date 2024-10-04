using UnityEngine.InputSystem;
using UnityEngine;

public class LocalPlayerMove : MonoBehaviour
{
    Rigidbody rb;
    [SerializeField,Tooltip("�����x")]float speedconf;
    [SerializeField, Tooltip("�ő呬�x")] float MaxSpeed;

    Vector2 vec;
    public void OnMove(InputAction.CallbackContext context)
    {
        vec = context.ReadValue<Vector2>();
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
        if(vel.magnitude > MaxSpeed) //�ő呬�x�ȏ�ɂȂ�Ȃ��悤�ɒ���
        {
            Vector3 nomal = vel.normalized;
            float gap = vel.magnitude - MaxSpeed;
            nomal *= gap;
            vel -= nomal;
            rb.velocity = vel;
        }
    }
}
