using Fusion;
using Fusion.Addons.Physics;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class PlayerJumpNet : NetworkBehaviour
{
    Animator animator;
    AudioSource audioSource;

    [Header("�W�����vSE"), SerializeField] private AudioClip jumpSE;

    [Networked] public NetworkButtons ButtonsPrevious { get; set; }

    [Networked] private bool isGround { get; set; }
    public bool GetisGround(){return isGround; }
    [Networked] private bool isOnce { get; set; }

    [SerializeField, Header("�W�����v�̗�")] private float jumpPower = 10.0f;
    [SerializeField, Header("�d��")] private float gravity = 9.8f;

    [SerializeField, Tooltip("エフェクトオブジェクト")]
    GameObject effect;

    ParticleSystem particle;

    [Networked] public bool isEffect { get; set; } = false;

    [SerializeField, Networked] bool jumpstart { get; set; } = false;

    private HitStop hitstop;
    private PlayerAttack attack;
    private PlayerChargeAttack chargeattack;
    PlayerFreeze freeze;

    private Vector3 scale;
    bool isReflection;

    [Networked]  Vector3 velocity { get; set; }  // �v���C���[�̑��x
    private bool isJumping;    // �W�����v�����ǂ���    
    public bool GetisJumping() { return isJumping; }
    public override void Spawned()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();

        // Unity�̎����d�͂̓I�t�ɂ��Ă���
        GetComponent<NetworkRigidbody3D>().Rigidbody.useGravity = false;

        if(!particle)
            particle = effect.GetComponent<ParticleSystem>();
        hitstop=GetComponent<HitStop>();
        attack = GetComponent<PlayerAttack>();
        chargeattack = GetComponent<PlayerChargeAttack>();
        freeze = GetComponent<PlayerFreeze>();
        scale=transform.localScale;
    }

    public override void FixedUpdateNetwork()
    {
        AnimatorStateInfo landAnimStateInfo = GetComponent<Animator>().GetCurrentAnimatorStateInfo(0);

        if (hitstop.IsHitStopActive||chargeattack.isCharge||freeze.GetIsFreeze())
        {
            return;
        }
        if (velocity.y<0 && !landAnimStateInfo.IsName("APlayerJumpDown")&&!isGround)//ジャンプの降りアニメーション再生
        {
            //animator.Play("APlayerJumpDown");
        }
        if (Object.HasStateAuthority && GetInput(out NetworkInputData data))
        {
            var pressed = data.Buttons.GetPressed(ButtonsPrevious);
            ButtonsPrevious = data.Buttons;

            //Debug.Log("�n�ʂɒ����Ă��邩: " + isGround);

            // �W�����v�{�^����������A���n�ʂɂ���Ƃ��W�����v����
            if (pressed.IsSet(NetworkInputButtons.Jump) && isGround && !isJumping)
            {
                Instantiate(particle, this.gameObject.transform.position, Quaternion.identity);
                //particle.Play();
                RPC_Jump();
                isJumping = true;  // �W�����v���ɐݒ�
                jumpstart = true;

                Debug.Log("�W�����v���܂�");
            }
            if (data.Direction.x > 0.0f)
            {
                isReflection = false;
            }
            else if (data.Direction.x < 0.0f)
            {
                isReflection = true;
            }
            if (landAnimStateInfo.IsName("APlayerJumpDown") || landAnimStateInfo.IsName("APlayerJumpUp"))
            {
                if (GetComponent<PlayerMove>().isReflection)
                {
                    Vector3 temp = scale;
                    temp.x = -scale.x;
                    transform.localScale= temp;
                }
                else
                {
                    Vector3 temp = scale;

                    transform.localScale = temp;
                }
            }
            // ����̏d�͌v�Z��K�p
            ApplyGravity();
        }
    }
    public override void Render()
    {
        AnimatorStateInfo landAnimStateInfo = GetComponent<Animator>().GetCurrentAnimatorStateInfo(0);

        if (hitstop.IsHitStopActive)
        {
            return;
        }
        if (jumpstart && !landAnimStateInfo.IsName("APlayerJumpDown") && !landAnimStateInfo.IsName("APlayerJumpUp"))//ジャンプの上りアニメーション再生
        {

            isEffect = true;
        }


        if (velocity.y < 0 && !landAnimStateInfo.IsName("APlayerJumpDown") && !isGround)//ジャンプの降りアニメーション再生
        {
            //animator.Play("APlayerJumpDown");

        }

        if (isEffect)
        {
            Instantiate(particle, this.gameObject.transform.position, Quaternion.identity);
            isEffect = false;
        }
    }

    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void RPC_Jump()
    {
        audioSource.PlayOneShot(jumpSE);

        // �W�����v�̏����x��ݒ�
        velocity = new Vector3(velocity.x,jumpPower, velocity.z);
        isGround = false;  // �W�����v�����̂Œn�ʂɂ��Ȃ���Ԃɐݒ�

        Instantiate(particle, this.gameObject.transform.position, Quaternion.identity);
    }

    // �d�͂��蓮�Ōv�Z���ēK�p���郁�\�b�h
    private void ApplyGravity()
    {
        if (!isGround)  // �󒆂ɂ���ꍇ�ɂ̂ݏd�͂�K�p
        {
            // �d�͉����x�𑬓x�ɉ�����
            float vel = velocity.y;
            vel -= gravity * Runner.DeltaTime;
            velocity = new Vector3(velocity.x, vel, velocity.z);

            // �v�Z�������x��Rigidbody�ɓK�p
            Vector3 currentVelocity = GetComponent<NetworkRigidbody3D>().Rigidbody.velocity;
            GetComponent<NetworkRigidbody3D>().Rigidbody.velocity = new Vector3(currentVelocity.x, velocity.y, currentVelocity.z);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGround = true;
            isJumping = false;
            jumpstart = false;
            animator.speed = 1.0f;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        // �n�ʂ��痣�ꂽ�ꍇ�̏���
        if (collision.gameObject.CompareTag("Ground"))
        {
            animator.speed = 2.5f;
            isGround = false;
            animator.Play("APlayerJumpDown",-1,0f);
            
        }
    }
}
