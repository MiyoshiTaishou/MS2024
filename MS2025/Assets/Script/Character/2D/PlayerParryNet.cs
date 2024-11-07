using Fusion;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using static UnityEngine.ParticleSystem;

public class PlayerParryNet : NetworkBehaviour
{
    //�p���B�͈�
    private GameObject ParryArea;

    private Animator animator;

    AudioSource audioSource;

    AudioManager audioManager;

    [Header("�p���BSE"), SerializeField] private AudioClip ParrySE;
    [Header("�p���B����SE"), SerializeField] private AudioClip ParrySuccessSE;

    [SerializeField, Tooltip("�p���B�͈�")] float parryradius = 3;

    [Networked] public NetworkButtons ButtonsPrevious { get; set; }

    //�p���B�̌��ʎ���
    [SerializeField, Tooltip("�p���B���ʎ���")] float ParryActivetime = 3;
    [Networked] private float ParryActivetimeFrame { get; set; } = 0; //�t���[���ɕϊ�����

    //�q�b�g�X�g�b�v����
    [SerializeField, Tooltip("�q�b�g�X�g�b�v����")] private int HitStop = 30;
    private float HitStopFrame = 0; //�t���[���ɕϊ�����

    //�m�b�N�o�b�N
    [SerializeField, Tooltip("�m�b�N�o�b�N��")] float KnockbackPower = 50;

    //�G����̍U�����󂯂�������
    public bool DamageReceive { get; set; } = false;

    /// <summary>
    /// �p���B��Ԃ��ǂ���
    /// </summary>
    [Networked] bool isParry { get; set; } = false;

    /// <summary>
    /// �p���B��Ԃ��ǂ���
    /// </summary>
    [Networked] bool isParrySuccess { get; set; } = false;

    [Networked] bool isParryAnimation { get; set; } = false;

    HitStop hitStop;

    Knockback back;

    private NetworkRunner runner;
    private NetworkObject networkobject;

    [SerializeField,Networked] bool isHost { get; set; } = false;
    //[SerializeField,ReadOnly] private bool _isHost => isHost;

    private GameObject playerhost;

    [SerializeField, Tooltip("�G�t�F�N�g�p���B")]
    GameObject Parryeffect;

    ParticleSystem particle;

    [SerializeField, Tooltip("�G�t�F�N�g�J�E���^�[")]
    GameObject Countereffect;

    ParticleSystem counterparticle;

    //�\�����Ԃ̃Q�b�^�[
    public float GetParryActiveTime() { return ParryActivetimeFrame; }

    //�p���B��Ԃ��ǂ���
    public void SetParryflg(bool flg) { isParry = flg; }

    // �A�j���[�V���������l�b�g���[�N����������
    [Networked]
    private NetworkString<_16> networkedAnimationName { get; set; }

    private bool isGround = false;

    [Networked] bool NetParryeffect { get; set; } = false;

    [Networked] bool NetCountereffect { get; set; } = false;

    /// <summary>
    /// �p���B��Ԃ��ǂ����̃`�F�b�N(�v���C���[���_���[�W���󂯂��Ƃ��ɌĂ�)
    /// </summary>
    public bool ParryCheck()
    {
        //Debug.Log("�p���B!!!");

        if (isParry)
        {
            return true;
        }
        else
        {
            return false;
        }

    }
    public void Counter()
    {

        if (ParryArea.GetComponent<ParryDisplayNet>().Hit)
        {
            RPC_ParrySystem();
        }
    }


    /// <summary>
    /// �p���B��Ԃ��ǂ����̃`�F�b�N(�p���B�W�����v�p)
    /// </summary>
    public bool ParryJumpCheck()
    {
        // Debug.Log("�p���B!!!");

        if (ParryArea.activeSelf)
        {
            return true;
        }
        else
        {
            return false;
        }

    }

    public override void Spawned()
    {
        // NetworkRunner�̃C���X�^���X���擾
        runner = FindObjectOfType<NetworkRunner>();

        //SE�ǂݍ���
        //audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        audioSource = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();//�A�j���[�^�[
        hitStop = GetComponent<HitStop>();
        back = GetComponent<Knockback>();
        Vector3 scale = new Vector3(parryradius, parryradius, parryradius);
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).gameObject.name == "ParryArea")
                ParryArea = transform.GetChild(i).gameObject;
        }

        ParryArea.gameObject.SetActive(false);

        //�t���[���ɒ���
        HitStopFrame = HitStop / 60;
        ParryActivetimeFrame = ParryActivetime / 60;

        ParryArea.transform.localScale = scale;
        networkobject = FindObjectOfType<NetworkObject>();

        if (Object.HasInputAuthority)
        {
           isHost= true;
        }

        particle = Parryeffect.GetComponent<ParticleSystem>();

        counterparticle = Countereffect.GetComponent<ParticleSystem>();

    }

    public void Area()
    {
        ParryArea.SetActive(true);
        isParry = true;
        isParryAnimation = true;

    }

    [Rpc(RpcSources.All, RpcTargets.All)]
    public void RPC_ParryArea()
    {
        Area();
    }

    /// <summary>
    /// �p���B�������̏���
    /// </summary>
    public void ParrySystem()
    {

        Debug.Log("�p���B�V�X�e��");
        audioSource.PlayOneShot(ParrySuccessSE);
        animator.Play("APlayerCounter");
        // animator.SetTrigger("ParrySuccess"); // �A�j���[�V�����̃g���K�[
        NetCountereffect = true;
        hitStop.ApplyHitStop(HitStopFrame);
        //cinemachar.CameraZoom(this.character.transform, 5,0.5f);
        back.ApplyKnockback(transform.forward, KnockbackPower);
        ParryArea.GetComponent<ParryDisplayNet>().Init();

        isParrySuccess = true;
        isParry = false;
    }

    [Rpc(RpcSources.All, RpcTargets.All)]
    public void RPC_ParrySystem()
    {
        if (ParryArea.GetComponent<ParryDisplayNet>().Hit)
            ParrySystem();
    }

    public void ParryStart()
    {
        audioSource.PlayOneShot(ParrySE);
        // animator.SetTrigger("Parry"); // �A�j���[�V�����̃g���K�[
       
        animator.Play("APlayerParry");
        ParryArea.SetActive(true);
        NetParryeffect = true;
    }


    public override void FixedUpdateNetwork()
    {
        AnimatorStateInfo landAnimStateInfo = GetComponent<Animator>().GetCurrentAnimatorStateInfo(0);

        if (GetInput(out NetworkInputData data))
        {
            //�p���B���͓������Ȃ��悤�ɂ���
            if (landAnimStateInfo.IsName("APlayerAtack1") || landAnimStateInfo.IsName("APlayerAtack2") || landAnimStateInfo.IsName("APlayerAtack3"))
            {
                return;
            }

            var pressed = data.Buttons.GetPressed(ButtonsPrevious);
            ButtonsPrevious = data.Buttons;

            // Attack�{�^���������ꂽ���A���A�j���[�V�������Đ����łȂ����`�F�b�N
            if (pressed.IsSet(NetworkInputButtons.Parry) && !isParry && isGround /*�n��ɂ��邩�̔���*/)
            {
                
                ParryStart();
                RPC_ParryArea();
            }

        }

    }

    private void OnCollisionEnter(Collision collision)
    {
        // �n��ɂ��邩
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGround = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGround = false;
        }
    }

    public override void Render()
    {
        if (isParry && isParryAnimation) //�p���B�A�j���[�V���������ǂ���
        {
            Debug.Log("�p���B�N���C�A���g");
            NetParryeffect = true;
            animator.Play("APlayerParry");
            isParryAnimation = false; // �t���O�����Z�b�g
        }

        if (isParrySuccess) //�p���B�A�j���[�V���������ǂ���
        {
            Debug.Log("�p���B�J�E���^�[�N���C�A���g");
            NetCountereffect = true;
            animator.Play("APlayerCounter");
            isParrySuccess = false;
        }

        if (NetParryeffect)
        {
            Vector3 pos = transform.position;
            pos.y -= this.gameObject.transform.localScale.y / 2;//���ł������ǃv���C���[�̑����܂ŉ�����
            pos.y += 0.5f;//�n�ʂƏd�Ȃ�Ȃ��悤�ɏ���������
            Instantiate(particle,pos, Quaternion.identity);
            NetParryeffect = false;
        }

        if (NetCountereffect)
        {
            counterparticle.Play();
            NetCountereffect = false;
        }

        //�z�X�g�Ȃ�I��
        //if (Object.HasInputAuthority)
        //{
        //    return;
        //}


    }
}
