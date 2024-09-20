using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerParry : MonoBehaviour
{
    //�p���B�͈�
    [SerializeField] private GameObject ParryArea;

    //�q�b�g�X�g�b�v����
    [SerializeField] private float HitStop = 0.05f;

    Camera Maincamera;
    CinemaCharCamera cinemachar;

    [SerializeField] �@bool Parryflg = false;

    HitStop hitStop;

    private bool PressKey = false; 

    // Start is called before the first frame update
    void Start()
    {
        hitStop = GetComponent<HitStop>();
        Maincamera = Camera.main;
        cinemachar = Maincamera.GetComponent<CinemaCharCamera>();
    }

    // Update is called once per frame
    void Update()
    {
        //�L�[�{�[�h������
        if(Input.GetKey(KeyCode.K))
        {
            ParryArea.SetActive(true);
        }
        else
        {
            //ParryArea.SetActive(false);
        }

        if(Parryflg)
        {
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
        cinemachar.CameraZoom(Vector2.zero,5,2);
        
    }
}
