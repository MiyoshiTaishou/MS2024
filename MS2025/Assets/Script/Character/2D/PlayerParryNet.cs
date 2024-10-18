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

    Animator animator;


    [SerializeField, Tooltip("�p���B�͈�")] float parryradius = 3;

    [Networked] public NetworkButtons ButtonsPrevious { get; set; }

    //�p���B�̌��ʎ���
    [SerializeField, Tooltip("�p���B���ʎ���")] float ParryActivetime = 3;
    private float ParryActivetimeFrame = 0; //�t���[���ɕϊ�����

    //�q�b�g�X�g�b�v����
    [SerializeField, Tooltip("�q�b�g�X�g�b�v����")] private int HitStop = 30;
    private float HitStopFrame = 0; //�t���[���ɕϊ�����

    //�m�b�N�o�b�N
    [SerializeField, Tooltip("�m�b�N�o�b�N��")] float KnockbackPower = 50;

    //�G����̍U�����󂯂�������
    public bool DamageReceive { get; set; } = false;

    //Camera Maincamera;
    //CinemaCharCamera cinemachar;

    [SerializeField, ReadOnly] bool Parryflg = false;

    //HitStop hitStop;

    [Networked] private bool PressKey { get; set; } = false;

    //Knockback back;

    private NetworkRunner runner;

    //�\�����Ԃ̃Q�b�^�[
    public float GetParryActiveTime() { return ParryActivetimeFrame; }

    //�p���B��Ԃ��ǂ���
    public void SetParryflg(bool flg) { Parryflg = flg; }

    public override void Spawned()
    {
        // NetworkRunner�̃C���X�^���X���擾
        runner = FindObjectOfType<NetworkRunner>();

        animator = GetComponent<Animator>();

        //hitStop = GetComponent<HitStop>();
        //Maincamera = Camera.main;
        //cinemachar = Maincamera.GetComponent<CinemaCharCamera>();
        //back = GetComponent<Knockback>();
        Vector3 scale = new Vector3(parryradius, parryradius, parryradius);
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).gameObject.name == "ParryArea")
                ParryArea = transform.GetChild(i).gameObject;
        }

        ParryArea.gameObject.SetActive(false);

        //�t���[���ɒ���
        Debug.Log(Application.targetFrameRate);
        HitStopFrame = HitStop / 60;
        ParryActivetimeFrame = ParryActivetime / 60;

        ParryArea.transform.localScale = scale;
    }

    public void Area()
    {
        ParryArea.SetActive(true);
        Parryflg = true;
    }

    [Rpc(RpcSources.InputAuthority, RpcTargets.All)]
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
        if (context.started)
        {
            RPC_ParryArea();
        }

    }

    /// <summary>
    /// �p���B�������̏���
    /// </summary>
    public void ParrySystem()
    {
        //hitStop.ApplyHitStop(HitStopFrame);
        //cinemachar.CameraZoom(this.transform,5,0.5f);
        //back.ApplyKnockback(transform.forward, KnockbackPower);
        ParryArea.GetComponent<ParryDisplay>().Init();
        DamageReceive = false;
        animator.SetTrigger("Parry");
    }

    [Rpc(RpcSources.InputAuthority, RpcTargets.All)]
    public void RPC_ParrySystem()
    {
        ParrySystem();
    }

    public override void FixedUpdateNetwork()
    {

        //if (runner != null)
        //{
         
        //    // �z�X�g�p�̏���
        //    if (runner.IsServer)
        //    {
        //        Debug.Log("This instance is the Host (Server).");
        //        if (Object.HasInputAuthority)
        //        {
        //            if (ParryArea.activeSelf)
        //            {
        //                //�Ƃ肠�����L�[�{�[�h�ŉ�����
        //                if (Input.GetKeyDown(KeyCode.L) || DamageReceive)
        //                {
        //                    RPC_ParrySystem();
        //                }
        //            }
        //        }
        //    }
        //    else if (runner.IsClient)
        //    {
        //        Debug.Log("This instance is a Client.");
        //        // �N���C�A���g�p�̏���

        //        // �N���C�A���g���ł̓��͏���
        //        if (ParryArea.activeSelf)
        //        {
        //            if (Input.GetKeyDown(KeyCode.L) || DamageReceive)
        //            {
        //                // RPC��ʂ��ăz�X�g�Ƀp���B��ʒm
        //                RPC_ParrySystem();
        //            }
        //        }
        //    }
        //}
        //else
        //{
        //    Debug.LogError("NetworkRunner not found in the scene!");
        //}

        if (Object.HasStateAuthority && GetInput(out NetworkInputData data))
        {
            var pressed = data.Buttons.GetPressed(ButtonsPrevious);
            ButtonsPrevious = data.Buttons;
           
            // �W�����v�{�^����������A���n�ʂɂ���Ƃ��W�����v����
            if (pressed.IsSet(NetworkInputButtons.Parry))
            {
                // RPC��ʂ��ăz�X�g�Ƀp���B��ʒm
                RPC_ParrySystem();
                RPC_ParryArea();
            }          
        }

    }   
}
