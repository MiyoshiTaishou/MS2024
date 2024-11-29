using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

public class PlayerSpecialAttackNet : NetworkBehaviour
{
    [Networked] public NetworkButtons ButtonsPrevious { get; set; }
    [Networked] public int specialNum { get; set; }
    [Networked] public int specialDamage { get; set; }
   
    GameObject director;
    GameObject comboCountObject;

    private float SpecialTime = 0.0f;
    private float SpecialTime2 = 0.0f;

    [Header("�P�\����"), SerializeField]
    private float specialTimeWait = 0.2f;

    [SerializeField,Tooltip("�K�E�Z�g���鎞�̃R���{���̐F")] Color specialColor;
    [SerializeField,ReadOnly] private List<Image> ComboList;

    [SerializeField, Tooltip("�K�E�Z�g���鎞�̃v���C���[�p�[�e�B�N��")] private ParticleSystem Tanukiparticle;
    [SerializeField, Tooltip("�K�E�Z�g���鎞�̃v���C���[�p�[�e�B�N��")] private ParticleSystem Kituneparticle;


    public override void Spawned()
    {
        //�K�E�Z�Đ��p�I�u�W�F�N�g�T��
        director = GameObject.Find("Director");
        comboCountObject = GameObject.Find("Networkbox");
        for (int j = 0; j < GameObject.Find("MainGameUI/Combo").transform.childCount; j++)
        {
            if (GameObject.Find("MainGameUI/Combo").transform.GetChild(j).GetComponent<Image>())
            {
                ComboList.Add(GameObject.Find("MainGameUI/Combo").transform.GetChild(j).GetComponent<Image>());

            }
        }
    }

    public override void FixedUpdateNetwork()
    {
        if (Object.HasStateAuthority && GetInput(out NetworkInputData data))
        {
            var pressed = data.Buttons.GetPressed(ButtonsPrevious);
            ButtonsPrevious = data.Buttons;

            if(pressed.IsSet(NetworkInputButtons.Special))
            {
                SpecialTime = specialTimeWait;
            }
            
            if(pressed.IsSet(NetworkInputButtons.Attack))
            {
                SpecialTime2 = specialTimeWait;
            }

            //�U���{�^�����������Ƃ��ɃR���{�J�E���g���w��̐��𒴂��Ă�ꍇ�Đ�
            if (SpecialTime > 0.0f && SpecialTime2 > 0.0f && comboCountObject.GetComponent<ShareNumbers>().nCombo >= specialNum)
            {               
                RPC_SpecialAttack();
                SpecialTime = 0.0f;
                SpecialTime2 = 0.0f;
                GetComponent<PlayerMove>().isMove = false;
            }


            if(SpecialTime > 0.0f)
            {
                SpecialTime -= Time.deltaTime;
            }

            if (SpecialTime2 > 0.0f)
            {
                SpecialTime2 -= Time.deltaTime;
            }
        }

        if (comboCountObject.GetComponent<ShareNumbers>().nCombo >= specialNum)
        {
            for(int i = 0;i < ComboList.Count;i++)
            {
                ComboList[i].color = specialColor;
            }
        }
        else
        {
            for (int i = 0; i < ComboList.Count; i++)
            {
                Color col = ComboList[i].color;
                Color color = Color.white;
                color.a = ComboList[i].color.a;
                ComboList[i].color = color;
            }
        }
    }

    public override void Render()
    {
        if (Object.HasStateAuthority)
        {
            if (comboCountObject.GetComponent<ShareNumbers>().nCombo >= specialNum)
            {
                Tanukiparticle.Play();
                Debug.Log("�^�k�L���X�^�[�g");
            }
            else
            {
                Tanukiparticle.Stop();
                Kituneparticle.Stop();

                Debug.Log("�^�k�L���G���h");

            }
        }
        else
        {
            if (comboCountObject.GetComponent<ShareNumbers>().nCombo >= specialNum)
            {
                Kituneparticle.Play();
                Debug.Log("�L�c�l���X�^�[�g");

            }
            else
            {
                Tanukiparticle.Stop();
                Kituneparticle.Stop();
                Debug.Log("�L�c�l���G���h");

            }

        }
    }

    [Rpc(RpcSources.All, RpcTargets.All)]
    public void RPC_SpecialAttack()
    {
        director.GetComponent<PlayableDirector>().Play();       
    }
}
