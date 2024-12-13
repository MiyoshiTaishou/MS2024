using Fusion;
using Fusion.Addons.Physics;
using UnityEngine;
using AIE2D;

public class PlayerJumpNet : NetworkBehaviour
{
    Animator animator;
    AudioSource audioSource;

    [Header("�W�����vSE"), SerializeField] private AudioClip jumpSE;

    [Networked] public NetworkButtons ButtonsPrevious { get; set; }

    [Networked] private bool isGround { get; set; }
    public bool GetisGround() { return isGround; }
    [Networked] private bool isOnce { get; set; }

    [SerializeField, Header("�W�����v�̗�")] private float jumpPower = 10.0f;
    [SerializeField, Header("�d��")] private float gravity = 9.8f;
  
    [Networked] public bool isEffect { get; set; } = false;

    [SerializeField, Networked] bool jumpstart { get; set; } = false;

    private HitStop hitstop;
    private PlayerAttack attack;
    private PlayerChargeAttack chargeattack;
    PlayerFreeze freeze;

    private Vector3 scale;
    bool isReflection;
    [Networked] public bool isAnim { get; set; }

    [Networked] Vector3 velocity { get; set; }  // �v���C���[�̑��x
    [Networked] private bool isJumping { get; set; }    // �W�����v�����ǂ���    
    public bool GetisJumping() { return isJumping; }
    int count = 0;

    StaticAfterImageEffect2DPlayer afterImage;

    ShareNumbers sharenum;
    public override void Spawned()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        sharenum=GameObject.Find("Networkbox").GetComponent<ShareNumbers>();
        // Unity�̎����d�͂̓I�t�ɂ��Ă���
        GetComponent<NetworkRigidbody3D>().Rigidbody.useGravity = false;
      
        hitstop = GetComponent<HitStop>();
        attack = GetComponent<PlayerAttack>();
        chargeattack = GetComponent<PlayerChargeAttack>();
        freeze = GetComponent<PlayerFreeze>();
        scale = transform.localScale;
        isAnim = false;

        afterImage = GetComponent<StaticAfterImageEffect2DPlayer>();
    }

    public override void FixedUpdateNetwork()
    {
        // ����̏d�͌v�Z��K�p
        ApplyGravity();

        AnimatorStateInfo landAnimStateInfo = GetComponent<Animator>().GetCurrentAnimatorStateInfo(0);
        GameObject BossObj = GameObject.Find("Boss2D");
        afterImage.SetActive(isJumping);

        if (hitstop.IsHitStopActive || chargeattack.isCharge || freeze.GetIsFreeze() || sharenum.CurrentHP == 0 ||
            BossObj.GetComponent<BossStatus>().nBossHP <= 0)
        {
            return;
        }
        if (count > 0)
        {
            count--;
        }
        else if (count == 0)
        {
            RPC_Jump();
            count = -1;
        }
        if (velocity.y < 0 && !landAnimStateInfo.IsName("APlayerJumpDown") && !isGround)//ジャンプの降りアニメーション再生
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
                //particle.Play();
                count = 5;
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
                    transform.localScale = temp;
                }
                else
                {
                    Vector3 temp = scale;

                    transform.localScale = temp;
                }
            }
        }
    }
    public override void Render()
    {
        AnimatorStateInfo landAnimStateInfo = GetComponent<Animator>().GetCurrentAnimatorStateInfo(0);

        if (hitstop.IsHitStopActive || sharenum.CurrentHP == 0)
        {
            return;
        }
        if (jumpstart && !landAnimStateInfo.IsName("APlayerJumpDown") && !landAnimStateInfo.IsName("APlayerJumpUp"))//ジャンプの上りアニメーション再生
        {
            animator.Play("APlayerIdle");
            GetComponent<PlayerAnimChange>().RPC_InitAction("APlayerJumpDown");
            jumpstart = false;
            isEffect = true;
        }


        if (velocity.y < 0 && !landAnimStateInfo.IsName("APlayerJumpDown") && !isGround)//ジャンプの降りアニメーション再生
        {
            //animator.Play("APlayerJumpDown");

        }

        if (isEffect)
        {          
            isEffect = false;
        }
        if (isGround == false && isAnim == true)
        {

            isAnim = false;
        }
    }

    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void RPC_Jump()
    {
        audioSource.PlayOneShot(jumpSE);

        // �W�����v�̏����x��ݒ�
        velocity = new Vector3(velocity.x, jumpPower, velocity.z);
        isGround = false;  // �W�����v�����̂Œn�ʂɂ��Ȃ���Ԃɐݒ�       
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
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        // �n�ʂ��痣�ꂽ�ꍇ�̏���
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGround = false;
            isAnim = true;
        }
    }
}
