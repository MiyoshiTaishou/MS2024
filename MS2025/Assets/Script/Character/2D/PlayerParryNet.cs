using Fusion;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

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

    //Camera Maincamera;
    //CinemaCharCamera cinemachar;

    [SerializeField,Networked] bool Parryflg { get; set; } = false;

    HitStop hitStop;

    [Networked] private bool PressKey { get; set; } = false;

    Knockback back;

    private NetworkRunner runner;

    //�\�����Ԃ̃Q�b�^�[
    public float GetParryActiveTime() { return ParryActivetimeFrame; }

    //�p���B��Ԃ��ǂ���
    public void SetParryflg(bool flg) { Parryflg = flg; }

    // �A�j���[�V���������l�b�g���[�N����������
    [Networked]
    private NetworkString<_16> networkedAnimationName { get; set; }

    /// <summary>
    /// �p���B��Ԃ��ǂ����̃`�F�b�N(�v���C���[���_���[�W���󂯂��Ƃ��ɌĂ�)
    /// </summary>
    public bool ParryCheck()
    {
        // Debug.Log("�p���B!!!");

        if (ParryArea.activeSelf)
        {
            ParrySystem();
            return true;
        }
        else
        {
            return false;
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
        //Maincamera = Camera.main;
        //cinemachar = Maincamera.GetComponent<CinemaCharCamera>();
        back = GetComponent<Knockback>();
        Vector3 scale = new Vector3(parryradius, parryradius, parryradius);
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).gameObject.name == "ParryArea")
                ParryArea = transform.GetChild(i).gameObject;
        }

        ParryArea.gameObject.SetActive(false);

        //�t���[���ɒ���
        //Debug.Log(Application.targetFrameRate);
        HitStopFrame = HitStop / 60;
        ParryActivetimeFrame = ParryActivetime / 60;

        ParryArea.transform.localScale = scale;


    }

    public void Area()
    {
        ParryArea.SetActive(true);
        Parryflg = true;
    }

    [Rpc(RpcSources.All, RpcTargets.All)]
    public void RPC_ParryArea()
    {
        Area();
    }

    /// <summary>
    /// �R���g���[���[����
    /// </summary>
    /// <param name="context"></param>
    public void ParryPress(InputAction.CallbackContext context)
    {
        AnimatorStateInfo landAnimStateInfo2 = GetComponent<Animator>().GetCurrentAnimatorStateInfo(0);

        //�p���B���͓������Ȃ��悤�ɂ���
        if (landAnimStateInfo2.IsName("APlayerAtack1") || landAnimStateInfo2.IsName("APlayerAtack2") || landAnimStateInfo2.IsName("APlayerAtack3"))
        {
            return;
        }

        if (context.started)
        {
            ParryStart();
        }

    }

    /// <summary>
    /// �p���B�������̏���
    /// </summary>
    public void ParrySystem()
    {
        audioSource.PlayOneShot(ParrySuccessSE);

        animator.Play("APlayerCounter");

        hitStop.ApplyHitStop(HitStopFrame);
        //cinemachar.CameraZoom(this.character.transform, 5,0.5f);
        back.ApplyKnockback(transform.forward, KnockbackPower);
        ParryArea.GetComponent<ParryDisplayNet>().Init();
    }

    [Rpc(RpcSources.All, RpcTargets.All)]
    public void RPC_ParrySystem()
    {
        ParrySystem();
    }

    public void ParryStart()
    {
        audioSource.PlayOneShot(ParrySE);

        animator.Play("APlayerParry");
        ParryArea.SetActive(true);
        Parryflg = true;
    }


    public override void FixedUpdateNetwork()
    {
        AnimatorStateInfo landAnimStateInfo = GetComponent<Animator>().GetCurrentAnimatorStateInfo(0);

        if (Object.HasStateAuthority && GetInput(out NetworkInputData data))
        {
            //�p���B���͓������Ȃ��悤�ɂ���
            if (landAnimStateInfo.IsName("APlayerAtack1") || landAnimStateInfo.IsName("APlayerAtack2") || landAnimStateInfo.IsName("APlayerAtack3"))
            {
                return;
            }

            var pressed = data.Buttons.GetPressed(ButtonsPrevious);
            ButtonsPrevious = data.Buttons;

            // Attack�{�^���������ꂽ���A���A�j���[�V�������Đ����łȂ����`�F�b�N
            if (pressed.IsSet(NetworkInputButtons.Parry) && !Parryflg)
            {
                Debug.Log("�p���B�J�n");
                ParryStart();
                RPC_ParryArea();
            }

        }

    }

    public override void Render()
    {
        if (Object.HasStateAuthority)
        {
            return;
        }

        // �N���C�A���g���ł��A�j���[�V�������Đ��i�l�b�g���[�N�ϐ����ς�����Ƃ��Ɏ��s�j
        // ���݂̃A�j���[�V�����̏�Ԃ��擾
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        // �U���t���O�������Ă���ꍇ�ɃA�j���[�V�������g���K�[
        if (Parryflg && Animator.StringToHash("Parry") != stateInfo.shortNameHash) //�p���B�A�j���[�V���������ǂ���
        {
            animator.SetTrigger("Parry"); // �A�j���[�V�����̃g���K�[
            //Parryflg = false; // �t���O�����Z�b�g
        }
    }
}
