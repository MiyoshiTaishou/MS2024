using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

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

    public override void Spawned()
    {
        //�K�E�Z�Đ��p�I�u�W�F�N�g�T��
        director = GameObject.Find("Director");
        comboCountObject = GameObject.Find("Networkbox");
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
    }

    [Rpc(RpcSources.All, RpcTargets.All)]
    public void RPC_SpecialAttack()
    {
        director.GetComponent<PlayableDirector>().Play();       
    }
}
