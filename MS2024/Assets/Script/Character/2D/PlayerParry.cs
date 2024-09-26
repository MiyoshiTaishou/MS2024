using Fusion;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerParry : NetworkBehaviour
{
    //�p���B�͈�
    private GameObject ParryArea;

    [SerializeField, Tooltip("�p���B�͈�")] float parryradius = 3;

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

    HitStop hitStop;

    [Networked] private bool PressKey { get; set; } = false;

    Knockback back;

    public float GetParryActiveTime() { return ParryActivetimeFrame; }

    public void SetParryflg(bool flg) { Parryflg = flg; }

    public override void Spawned()
    {
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
        hitStop.ApplyHitStop(HitStopFrame);
        //cinemachar.CameraZoom(this.transform,5,0.5f);
        back.ApplyKnockback(transform.forward, KnockbackPower);
        ParryArea.GetComponent<ParryDisplay>().Init();
        DamageReceive = false;
    }

    [Rpc(RpcSources.InputAuthority, RpcTargets.All)]
    public void RPC_ParrySystem()
    {
        ParrySystem();
    }

    public override void FixedUpdateNetwork()
    {

        if(Object.HasInputAuthority)
        {
            if (ParryArea.activeSelf)
            {

                //�Ƃ肠�����L�[�{�[�h�ŉ�����
                if (Input.GetKeyDown(KeyCode.L) || DamageReceive)
                {
                    RPC_ParrySystem();
                }
            }
        }
    }


}
