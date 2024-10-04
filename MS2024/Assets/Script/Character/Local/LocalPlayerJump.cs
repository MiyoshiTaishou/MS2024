using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Windows;
using UnityEngine.InputSystem;

public class LocalPlayerJump : MonoBehaviour
{
    private Rigidbody rb;
    private Vector2 moveInput;

    private float rayDistance = 1.5f;
    private string groundTag = "Ground";

    private Animator animator;

    private float currentSpeed;
    // �W�����v�֘A
    [SerializeField, Tooltip("�W�����v����")] float jumpForce = 5.0f;
    [SerializeField, Tooltip("�������x")] float fallMultiplier = 2.5f; // �������x�̋���

    [SerializeField,ReadOnly] bool jump = true;
    PlayerInput input;


    void Start()
    {
        animator = GetComponent<Animator>();
        input = GetComponent<PlayerInput>();
        rb = GetComponent<Rigidbody>();
        rb.velocity = new Vector3(rb.velocity.x, jumpForce, rb.velocity.z);
    }


    public void Update()
    {
        //��������
        if (rb.velocity.y < 0)
        {
            Debug.Log("�W�����v�I���");


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
                    Debug.Log("�W�����v�I���");

                }
            }
        }



    }

    /// <summary>
    /// �R���g���[���[����
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
        }

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == groundTag)
        {
            jump = true;
        }
    }

}
