using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerParry : IState
{
    private bool buttonInput;

    //�p���B�͈�
    private GameObject ParryArea;

    [SerializeField, Tooltip("�p���B�͈�")] float parryradius = 3;

    //�p���B�̌��ʎ���
    [SerializeField, Tooltip("�p���B���ʎ���")] float ParryActivetime = 30;
    private float ParryActivetimeFrame = 0; //�t���[���ɕϊ�����

    //�q�b�g�X�g�b�v����
    [SerializeField, Tooltip("�q�b�g�X�g�b�v����")] private int HitStop = 3;
    private float HitStopFrame = 0; //�t���[���ɕϊ�����

    //�m�b�N�o�b�N
    [SerializeField, Tooltip("�m�b�N�o�b�N��")] float KnockbackPower = 10;

    /// <summary>
    /// �G����̍U�����󂯂�������
    /// </summary>
    public bool DamageReceive { get; set; } = false;

    Camera Maincamera;
    CinemaCharCamera cinemachar;

    [SerializeField, ReadOnly] bool Parryflg = false;

    HitStop hitStop;

    Knockback back;

    private PlayerState character;


    public PlayerParry(PlayerState character)
    {
        this.character = character;
    }

    /// <summary>
    /// �p���B��Ԃ��ǂ����̃`�F�b�N(�v���C���[���_���[�W���󂯂��Ƃ��ɌĂ�)
    /// </summary>
    public void ParryCheck()
    {
        if (ParryArea.activeSelf)
        {
            if (DamageReceive)
            {
                ParrySystem();
            }
        }
    }

    //�\�����Ԃ̃Q�b�^�[
    public float GetParryActiveTime() { return ParryActivetimeFrame; }

    //�p���B��Ԃ��ǂ���
    public void SetParryflg(bool flg) { Parryflg = flg; }

    public void Enter()
    {
        

        hitStop = character.GetComponent<HitStop>();
        Maincamera = Camera.main;
        cinemachar = Maincamera.GetComponent<CinemaCharCamera>();
        back = character.GetComponent<Knockback>();
        Vector3 scale = new Vector3(parryradius, parryradius, parryradius);
        for (int i = 0; i < character.transform.childCount; i++)
        {
            if (character.transform.GetChild(i).gameObject.name == "ParryArea")
                ParryArea = character.transform.GetChild(i).gameObject;
        }

        //�t���[���ɒ���
        HitStopFrame = HitStop / 60;
        ParryActivetimeFrame = ParryActivetime / 60;

        ParryArea.transform.localScale = scale;
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
       // cinemachar.CameraZoom(this.transform,5,0.5f);
        back.ApplyKnockback(character.transform.forward, KnockbackPower);
        ParryArea.GetComponent<ParryDisplay>().Init();
        DamageReceive = false;
    }

    public void Update()
    {

        if (ParryArea.activeSelf)
        {
            //�Ƃ肠�����L�[�{�[�h�ŉ�����
            if (Input.GetKeyDown(KeyCode.L) || DamageReceive)
            {
                ParrySystem();
            }
        }

    }

    public void Exit()
    {
        // Idle��Ԃ𔲂���Ƃ��̏���
    }

}
