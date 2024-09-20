using Fusion;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerParry : NetworkBehaviour
{
    //�p���B�͈�
    [SerializeField,Tooltip("�p���B�����p")] private GameObject ParryArea;

    [SerializeField, Tooltip("�p���B�͈�")] float parryradius = 3;

    //�p���B�̌��ʎ���
    [SerializeField, Tooltip("�p���B���ʎ���")] float ParryActivetime = 3;

    //�q�b�g�X�g�b�v����
    [SerializeField, Tooltip("�q�b�g�X�g�b�v����")] private float HitStop = 0.05f;

    //�m�b�N�o�b�N
    [SerializeField, Tooltip("�m�b�N�o�b�N��")] float KnockbackPower = 50;


    Camera Maincamera;
    CinemaCharCamera cinemachar;

    [SerializeField,ReadOnly] bool Parryflg = false;

    HitStop hitStop;

    private bool PressKey = false;

    Knockback back;

    public float GetParryActiveTime() { return ParryActivetime; }

    // Start is called before the first frame update
    void Start()
    {
        hitStop = GetComponent<HitStop>();
        Maincamera = Camera.main;
        cinemachar = Maincamera.GetComponent<CinemaCharCamera>();
        back = GetComponent<Knockback>();
        Vector3 scale = new Vector3(parryradius, parryradius, parryradius);
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

        //if (context.canceled)
        //{
        //    ParryArea.SetActive(false);
        //   // Parryflg= false;
        //}
    }

    /// <summary>
    /// �p���B�������̏���
    /// </summary>
    public void ParrySystem()
    {
        hitStop.ApplyHitStop(HitStop);
        cinemachar.CameraZoom(this.transform,5,0.5f);
        back.ApplyKnockback(transform.forward, KnockbackPower);
        ParryArea.GetComponent<ParryDisplay>().Init();
    }

    public override void FixedUpdateNetwork()
    {
        if (ParryArea.activeSelf)
        {

            //�Ƃ肠�����L�[�{�[�h�ŉ�����
            if (Input.GetKeyDown(KeyCode.L))
            {
                ParrySystem();
            }
        }

    }


}
