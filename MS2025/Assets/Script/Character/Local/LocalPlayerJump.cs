using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
//using UnityEngine.Windows;
using UnityEngine.InputSystem;

public class LocalPlayerJump : MonoBehaviour
{
    private Rigidbody rb;

    private float rayDistance = 1.5f;
    private string groundTag = "Ground";

    private Animator animator;

    // �W�����v�֘A
    [SerializeField, Tooltip("�W�����v����")] float jumpForce = 5.0f;
    [SerializeField, Tooltip("�������x")] float fallMultiplier = 2.5f; // �������x�̋���

    /// <summary>
    /// �W�����v�\���ǂ���
    /// </summary>
    [field:SerializeField,ReadOnly] public bool jump = true;

    /// <summary>
    /// �W�����v�����ǂ���
    /// </summary>
    public bool jumpnuw { get; private set; } = false;

    /// <summary>
    /// ���E�̕��������ɂ���ĕς���
    /// </summary>
    private Vector3 initscale;

    AudioSource audioSource;

    AudioManager audioManager;

    void Start()
    {
        //SE�ǂݍ���
        audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        audioSource = GetComponent<AudioSource>();

        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        jumpnuw = false;

        initscale = transform.localScale;

    }


    public void Update()
    {
        //�f�o�b�N�p-----------------------
        if(Input.GetKeyDown(KeyCode.I))
        {
            animator.Play("APlayerJumpUp");
            rb.AddForce(new Vector3(rb.velocity.x, jumpForce, rb.velocity.z), ForceMode.Impulse);

            jump = false;
            jumpnuw = true;
        }
        //-------------------------------

        //��������
        if (rb.velocity.y < 0)
        {

            //��������������A�j���[�V�������X�V
            AnimatorStateInfo animStateInfo = GetComponent<Animator>().GetCurrentAnimatorStateInfo(0);
            if (animStateInfo.IsName("APlayerJumpUp"))
            {
                //�����A�j���[�V����
                animator.Play("APlayerJumpDown");
                audioManager.PlaySE(audioSource, AudioManager.SESoundData.SE.JumpDown);

            }
            rb.velocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1) * Time.deltaTime;


        }

        //�n�ʂɓ�����܂ŗ����A�j���[�V�������Đ�
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, rayDistance))
        {
            if (hit.collider.CompareTag(groundTag))
            {
                AnimatorStateInfo landAnimStateInfo = GetComponent<Animator>().GetCurrentAnimatorStateInfo(0);
                if (landAnimStateInfo.IsName("APlayerJumpDown") && landAnimStateInfo.normalizedTime >= 1.0f)
                {
                    //����������A�C�h���ɖ߂�
                    animator.Play("APlayerIdle");
                    jumpnuw = false;//�W�����v�I��
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
        //�{�^�����������Ƃ��ȊO�͏I��
        if (!context.started)
        {
            return;
        }
        
        //�W�����v�\��
        if(jump)
        {
            audioManager.PlaySE(audioSource, AudioManager.SESoundData.SE.JumpUp);

            //�W�����v�����J�n
            animator.Play("APlayerJumpUp");
            rb.AddForce(new Vector3(rb.velocity.x, jumpForce, rb.velocity.z), ForceMode.Impulse);

            jump = false;
            jumpnuw = true;
        }

    }

    private void OnCollisionEnter(Collision collision)
    {
        //�n�ʂɒ��n������W�����v�\�ɂ���
        if (collision.gameObject.tag == groundTag)
        {
            jump = true;
        }
    }

}
