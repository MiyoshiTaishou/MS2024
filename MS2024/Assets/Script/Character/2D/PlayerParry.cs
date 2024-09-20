using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerParry : MonoBehaviour
{
    //�p���B�͈�
    [SerializeField,Tooltip("�p���B�����p")] private GameObject ParryArea;

    [SerializeField, Tooltip("�p���B�͈�")] float parryradius = 3;

    //�q�b�g�X�g�b�v����
    [SerializeField, Tooltip("�q�b�g�X�g�b�v����")] private float HitStop = 0.05f;

    //�m�b�N�o�b�N
    [SerializeField, Tooltip("�m�b�N�o�b�N��")] float KnockbackPower = 10;


    Camera Maincamera;
    CinemaCharCamera cinemachar;

    [SerializeField,ReadOnly] bool Parryflg = false;

    HitStop hitStop;

    private bool PressKey = false;

    Knockback back;

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

    // Update is called once per frame
    void Update()
    {

        if(Parryflg)
        {

            //�Ƃ肠�����L�[�{�[�h�ŉ�����
            if(Input.GetKeyDown(KeyCode.L))
            {
                ParrySystem();
            }
        }

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

        if (context.canceled)
        {
            ParryArea.SetActive(false);
           // Parryflg= false;
        }
    }



    /// <summary>
    /// �p���B�������̏���
    /// </summary>
    public void ParrySystem()
    {
        Debug.Log("�Y�[��");
        hitStop.ApplyHitStop(HitStop);
        cinemachar.CameraZoom(this.transform,5,0.5f);
        back.ApplyKnockback(transform.forward, KnockbackPower);
    }
}
