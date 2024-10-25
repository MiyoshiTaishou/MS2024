using Fusion;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

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

    /// <summary>
    /// �p���B��Ԃ��ǂ���
    /// </summary>
    [SerializeField,Networked] public bool isParry { get; private set; } = false;

    /// <summary>
    /// �p���B��Ԃ��ǂ���
    /// </summary>
    [SerializeField,Networked] bool isParrySuccess { get; set; } = false;

    [SerializeField,Networked] bool isParryAnimation { get; set; } = false;

    HitStop hitStop;

    Knockback back;

    private NetworkRunner runner;
    private NetworkObject networkobject;

    [SerializeField,Networked] bool isHost { get; set; } = false;
    //[SerializeField,ReadOnly] private bool _isHost => isHost;

    private GameObject playerhost;

    //�\�����Ԃ̃Q�b�^�[
    public float GetParryActiveTime() { return ParryActivetimeFrame; }

    //�p���B��Ԃ��ǂ���
    public void SetParryflg(bool flg) { isParry = flg; }

    // �A�j���[�V���������l�b�g���[�N����������
    [Networked]
    private NetworkString<_16> networkedAnimationName { get; set; }

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
        HitStopFrame = HitStop / 60;
        ParryActivetimeFrame = ParryActivetime / 60;

        ParryArea.transform.localScale = scale;
        networkobject = FindObjectOfType<NetworkObject>();

        if (Object.HasStateAuthority)
        {
            Debug.Log("�z�X�g�ł�");
           isHost= true;
        }


        // "Player(Clone)"�Ƃ������O�̃I�u�W�F�N�g��S�Ď擾
        GameObject[] players = FindObjectsOfType<GameObject>();

        foreach (GameObject player in players)
        {
            if (player.name == "Player(Clone)")
            {

               if( player.GetComponent<PlayerParryNet>().isHost)
                {
                    playerhost = player;
                }
            }
        }
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

        Debug.Log("�p���B�V�X�e��");
        audioSource.PlayOneShot(ParrySuccessSE);

        animator.Play("APlayerCounter");
       // animator.SetTrigger("ParrySuccess"); // �A�j���[�V�����̃g���K�[

        //hitStop.ApplyHitStop(HitStopFrame);
        //cinemachar.CameraZoom(this.character.transform, 5,0.5f);
       // back.ApplyKnockback(transform.forward, KnockbackPower);
        ParryArea.GetComponent<ParryDisplayNet>().Init();

        isParrySuccess = true;
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
            if (pressed.IsSet(NetworkInputButtons.Parry) && !isParry)
            {
                
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

        if(playerhost)
        {
            // "Player(Clone)"�Ƃ������O�̃I�u�W�F�N�g��S�Ď擾
            GameObject[] players = FindObjectsOfType<GameObject>();

            foreach (GameObject player in players)
            {
                if (player.name == "Player(Clone)")
                {

                    if (player.GetComponent<PlayerParryNet>().isHost)
                    {
                        playerhost = player;
                    }
                }
            }

        }

        if (playerhost.GetComponent<PlayerParryNet>().isParrySuccess ) //�p���B�A�j���[�V���������ǂ���
        {
            Debug.Log("�p���B�J�E���^�[");
            animator.Play("APlayerCounter");
            playerhost.GetComponent<PlayerParryNet>().isParrySuccess = false;
        }
        
        else if (isParry && isParryAnimation) //�p���B�A�j���[�V���������ǂ���
        {
            Debug.Log("�p���B");

            animator.SetTrigger("Parry"); // �A�j���[�V�����̃g���K�[
            isParryAnimation = false; // �t���O�����Z�b�g
        }
    }
}
