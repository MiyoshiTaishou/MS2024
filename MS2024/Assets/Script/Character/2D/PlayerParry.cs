using Fusion;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerParry : MonoBehaviour
{
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
    [SerializeField, Tooltip("�m�b�N�o�b�N��")] float KnockbackPower = 50;

    //�G����̍U�����󂯂�������
    public bool DamageReceive { get; set; } = false;

    Camera Maincamera;
    CinemaCharCamera cinemachar;

    [SerializeField, ReadOnly] bool Parryflg = false;

    HitStop hitStop;

    //private bool PressKey { get; set; } = false;

    Knockback back;

    //�\�����Ԃ̃Q�b�^�[
    public float GetParryActiveTime() { return ParryActivetimeFrame; }

    //�p���B��Ԃ��ǂ���
    public void SetParryflg(bool flg) { Parryflg = flg; }

    private void Start()
    {
        hitStop = GetComponent<HitStop>();
        Maincamera = Camera.main;
        cinemachar = Maincamera.GetComponent<CinemaCharCamera>();
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

    /// <summary>
    /// �R���g���[���[����
    /// </summary>
    /// <param name="context"></param>
    public void ParryPress(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            Area();
        }

    }

    //public void ParryHit(InputAction.CallbackContext context)
    //{

    //    if (ParryArea.activeSelf)
    //    {
    //        ParrySystem();
    //    }

    //}

    /// <summary>
    /// �p���B�������̏���
    /// </summary>
    public void ParrySystem()
    {
        Debug.Log("�p���B");
        hitStop.ApplyHitStop(HitStopFrame);
       // cinemachar.CameraZoom(this.transform,5,0.5f);
        back.ApplyKnockback(transform.forward, KnockbackPower);
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


}
