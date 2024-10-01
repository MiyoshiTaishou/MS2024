using Fusion;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerParry : IState
{
    private NetworkRunner runner;

    //�p���B�͈�
    private GameObject ParryArea;

    ////�p���B�̌��ʎ���
    private float ParryActivetimeFrame = 0; //�t���[���ɕϊ�����

    ////�q�b�g�X�g�b�v����
    private float HitStopFrame = 0; //�t���[���ɕϊ�����

    /// <summary>
    /// �G����̍U�����󂯂�������
    /// </summary>
   // public bool DamageReceive { get; set; } = false;

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
    public bool ParryCheck()
    {
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

    public void Enter()
    {
        hitStop = character.GetComponent<HitStop>();
        Maincamera = Camera.main;
        cinemachar = Maincamera.GetComponent<CinemaCharCamera>();
        back = character.GetComponent<Knockback>();
        Vector3 scale = new Vector3(character.parryradius, character.parryradius, character.parryradius);
        for (int i = 0; i < character.transform.childCount; i++)
        {
            if (character.transform.GetChild(i).gameObject.name == "ParryArea")
                ParryArea = character.transform.GetChild(i).gameObject;
        }

        //�t���[���ɒ���
        HitStopFrame = character.HitStop / 60;
        ParryActivetimeFrame = character.ParryActivetime / 60;

        ParryArea.transform.localScale = scale;

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
        back.ApplyKnockback(character.transform.forward, character.KnockbackPower);
        ParryArea.GetComponent<ParryDisplay>().Init();
    }

    [Rpc(RpcSources.InputAuthority, RpcTargets.All)]
    public void RPC_ParrySystem()
    {
        ParrySystem();
    }

    public void Update()
    {
        //�f�o�b�N�p
        if (ParryArea.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.L))
            {
                ParrySystem();
            }
        }

        if (runner != null)
        {

            // �z�X�g�p�̏���
            if (runner.IsServer)
            {
                Debug.Log("This instance is the Host (Server).");
                if (ParryArea.activeSelf)
                {
                    //�Ƃ肠�����L�[�{�[�h�ŉ�����
                    if (Input.GetKeyDown(KeyCode.L))
                    {
                        ParrySystem();
                    }
                }
            }
            else if (runner.IsClient)
            {
                Debug.Log("This instance is a Client.");
                // �N���C�A���g�p�̏���

                // �N���C�A���g���ł̓��͏���
                if (ParryArea.activeSelf)
                {
                    if (Input.GetKeyDown(KeyCode.L))
                    {
                        // RPC��ʂ��ăz�X�g�Ƀp���B��ʒm
                        RPC_ParrySystem();
                    }
                }
            }
        }
        else
        {
            Debug.LogError("NetworkRunner not found in the scene!");
        }

    }

    public void Exit()
    {
        // Idle��Ԃ𔲂���Ƃ��̏���
        Parryflg = false;
    }

}
