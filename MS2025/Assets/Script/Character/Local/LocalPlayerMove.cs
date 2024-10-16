using UnityEngine.InputSystem;
using UnityEngine;

public class LocalPlayerMove : MonoBehaviour
{
    Rigidbody rb;
    [SerializeField,Tooltip("加速度")]float speedconf;
    [SerializeField, Tooltip("最大速度")] float MaxSpeed;

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
            //入力なし
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

        //パリィ中は動かせないようにする
        if(landAnimStateInfo.IsName("APlayerParry") || landAnimStateInfo.IsName("APlayerAtack1") || landAnimStateInfo.IsName("APlayerAtack2") || landAnimStateInfo.IsName("APlayerAtack3"))
        {
            return;
        }

        if (!landAnimStateInfo.IsName("APlayerWalk") && !landAnimStateInfo.IsName("APlayerJumpUp") && !landAnimStateInfo.IsName("APlayerJumpDown"))
            if (Input.GetAxis("Vertical") != 0 || Input.GetAxis("Horizontal") != 0)
            {
                animator.Play("APlayerWalk");
            }

        rb.AddForce(vec.x * speedconf, 0.0f, vec.y * speedconf);
        Vector3 vel = rb.velocity;
        if (vel.magnitude > MaxSpeed) //最大速度以上にならないように調整
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
