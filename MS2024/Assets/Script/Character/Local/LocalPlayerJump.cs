using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
//using UnityEngine.Windows;
using UnityEngine.InputSystem;

public class LocalPlayerJump : MonoBehaviour
{
    private Rigidbody rb;
    private Vector2 moveInput;

    private float rayDistance = 1.5f;
    private string groundTag = "Ground";

    private Animator animator;

    private float currentSpeed;
    // ジャンプ関連
    [SerializeField, Tooltip("ジャンプ距離")] float jumpForce = 5.0f;
    [SerializeField, Tooltip("落下速度")] float fallMultiplier = 2.5f; // 落下速度の強化

    [field:SerializeField,ReadOnly] public bool jump { get; private set; } = true;
    PlayerInput input;

    public bool jumpnuw { get; private set; } = false;


    void Start()
    {
        animator = GetComponent<Animator>();
        input = GetComponent<PlayerInput>();
        rb = GetComponent<Rigidbody>();
        jumpnuw = false;
    }


    public void Update()
    {
        if(Input.GetKeyDown(KeyCode.I))
        {
            animator.Play("APlayerJumpUp");
            rb.AddForce(new Vector3(rb.velocity.x, jumpForce, rb.velocity.z), ForceMode.Impulse);

            jump = false;
            jumpnuw = true;
        }

        //落下処理
        if (rb.velocity.y < 0)
        {


            AnimatorStateInfo animStateInfo = GetComponent<Animator>().GetCurrentAnimatorStateInfo(0);
            if (animStateInfo.IsName("APlayerJumpUp"))
            {

                animator.Play("APlayerJumpDown");

            }
            rb.velocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1) * Time.deltaTime;


        }

        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, rayDistance))
        {
            if (hit.collider.CompareTag(groundTag))
            {
                AnimatorStateInfo landAnimStateInfo = GetComponent<Animator>().GetCurrentAnimatorStateInfo(0);
                if (landAnimStateInfo.IsName("APlayerJumpDown") && landAnimStateInfo.normalizedTime >= 1.0f)
                {
                    animator.Play("APlayerIdle");
                    jumpnuw = false;
                }
            }
        }



    }

    /// <summary>
    /// コントローラー入力
    /// </summary>
    /// <param name="context"></param>
    public void Jump(InputAction.CallbackContext context)
    {
        if (!context.started)
        {
            return;
        }
        
        if(jump)
        {
            animator.Play("APlayerJumpUp");
            rb.AddForce(new Vector3(rb.velocity.x, jumpForce, rb.velocity.z), ForceMode.Impulse);

            jump = false;
            jumpnuw = true;
        }

    }

    private void OnCollisionEnter(Collision collision)
    {
        //地面に着地したらジャンプ可能にする
        if (collision.gameObject.tag == groundTag)
        {
            jump = true;
        }
    }

}
