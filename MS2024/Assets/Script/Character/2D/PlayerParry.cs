using Fusion;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.TextCore.Text;

public class PlayerParry : MonoBehaviour
{
    //�p���B�͈�
    [SerializeField, ReadOnly] private GameObject ParryArea;

    ////�p���B�̌��ʎ���
    private float ParryActivetimeFrame = 0; //�t���[���ɕϊ�����

    ////�q�b�g�X�g�b�v����
    private float HitStopFrame = 0; //�t���[���ɕϊ�����

    // �p���B�͈�
    [SerializeField, Tooltip("�p���B�͈�")] public float parryradius = 3;

    // �p���B�̌��ʎ���
    [SerializeField, Tooltip("�p���B���ʎ���")] public float ParryActivetime = 30;

    // �q�b�g�X�g�b�v����
    [SerializeField, Tooltip("�q�b�g�X�g�b�v����")] public int HitStop = 3;

    // �m�b�N�o�b�N
    [SerializeField, Tooltip("�m�b�N�o�b�N��")] public float KnockbackPower = 10;

    /// <summary>
    /// �G����̍U�����󂯂�������
    /// </summary>
   // public bool DamageReceive { get; set; } = false;

    Camera Maincamera;
    CinemaCharCamera cinemachar;

    [SerializeField, ReadOnly] bool Parryflg = false;

    HitStop hitStop;

    Knockback back;

    private Animator animator;

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

    //�\�����Ԃ̃Q�b�^�[
    public float GetParryActiveTime() { return ParryActivetimeFrame; }

    //�p���B��Ԃ��ǂ���
    public void SetParryflg(bool flg) { Parryflg = flg; }

    void Start()
    {
        animator = GetComponent<Animator>();
        hitStop = GetComponent<HitStop>();
        Maincamera = Camera.main;
        cinemachar = Maincamera.GetComponent<CinemaCharCamera>();
        back = GetComponent<Knockback>();
        Vector3 scale = new Vector3(parryradius, parryradius, parryradius);
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).gameObject.name == "ParryArea")
            {
                ParryArea = transform.GetChild(i).gameObject;
            }
        }

        //�t���[���ɒ���
        HitStopFrame = HitStop / 60;
        ParryActivetimeFrame = ParryActivetime / 60;

        ParryArea.transform.localScale = scale;

        ////�p���B����
        //ParryArea.SetActive(true);
        //Parryflg = true;
    }

    /// <summary>
    /// �R���g���[���[����
    /// </summary>
    /// <param name="context"></param>
    public void ParryPress(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            ParryArea.SetActive(true);
            Parryflg = true;
        }

    }

    /// <summary>
    /// �p���B�������̏���
    /// </summary>
    public void ParrySystem()
    {
        hitStop.ApplyHitStop(HitStopFrame);
        //cinemachar.CameraZoom(this.character.transform, 5,0.5f);
        back.ApplyKnockback(transform.forward, KnockbackPower);
        ParryArea.GetComponent<ParryDisplay>().Init();
        animator.Play("APlayerParry");
    }

    [Rpc(RpcSources.InputAuthority, RpcTargets.All)]
    public void RPC_ParrySystem()
    {
        ParrySystem();
    }

    void Update()
    {
        //�f�o�b�N�p
        if (ParryArea.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.L))
            {
                ParrySystem();
            }
        }

        AnimatorStateInfo landAnimStateInfo = GetComponent<Animator>().GetCurrentAnimatorStateInfo(0);
        if (landAnimStateInfo.IsName("APlayerParry") && landAnimStateInfo.normalizedTime >= 1.0f)
            animator.Play("APlayerIdle");


    }

    //public void Exit()
    //{
    //    // Idle��Ԃ𔲂���Ƃ��̏���
    //    Parryflg = false;
    //}

}
