using UnityEngine.InputSystem;
using UnityEngine;

public class LocalPlayerMove : MonoBehaviour
{
    Rigidbody rb;
    [SerializeField,Tooltip("â¡ë¨ìx")]float speedconf;
    [SerializeField, Tooltip("ç≈ëÂë¨ìx")] float MaxSpeed;

    Vector2 vec;

    private Animator animator;

    private Vector3 initscale;


    public void OnMove(InputAction.CallbackContext context)
    {
        AnimatorStateInfo landAnimStateInfo = GetComponent<Animator>().GetCurrentAnimatorStateInfo(0);

        vec = context.ReadValue<Vector2>();
        Vector3 scale = transform.localScale;

        if (context.ReadValue<Vector2>().x<0)
        {
            GetComponent<LocalPlayerAttack>().SetLeftAttack(true);
            scale.x = initscale.x* - 1;
            if (!landAnimStateInfo.IsName("APlayerJumpUp") && !landAnimStateInfo.IsName("APlayerJumpDown"))
                transform.localScale = scale;
        }
        else if(context.ReadValue<Vector2>().x > 0)
        {
            GetComponent<LocalPlayerAttack>().SetLeftAttack(false);
            if (!landAnimStateInfo.IsName("APlayerJumpUp") && !landAnimStateInfo.IsName("APlayerJumpDown"))
                transform.localScale = initscale;
        }
        else
        {
            //ì¸óÕÇ»Çµ
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();

        initscale = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        AnimatorStateInfo landAnimStateInfo = GetComponent<Animator>().GetCurrentAnimatorStateInfo(0);
        if (!landAnimStateInfo.IsName("APlayerWalk") && !landAnimStateInfo.IsName("APlayerJumpUp") && !landAnimStateInfo.IsName("APlayerJumpDown"))
            if (Input.GetAxis("Vertical") != 0 || Input.GetAxis("Horizontal") != 0)
            {
                animator.Play("APlayerWalk");
            }

        rb.AddForce(vec.x * speedconf, 0.0f, vec.y * speedconf);
        Vector3 vel = rb.velocity;
        if (vel.magnitude > MaxSpeed) //ç≈ëÂë¨ìxà»è„Ç…Ç»ÇÁÇ»Ç¢ÇÊÇ§Ç…í≤êÆ
        {
            Vector3 nomal = vel.normalized;
            float gap = vel.magnitude - MaxSpeed;
            nomal *= gap;
            vel.x -= nomal.x;
            vel.z -= nomal.z;
            rb.velocity = vel;
        }

        if (landAnimStateInfo.IsName("APlayerWalk"))
        {
            if (Input.GetAxis("Vertical") == 0 && Input.GetAxis("Horizontal") == 0)
            {
                animator.Play("APlayerIdle");
            }
        }
    }
}
