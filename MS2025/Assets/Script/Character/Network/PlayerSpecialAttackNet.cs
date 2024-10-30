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

    public override void Spawned()
    {
        //必殺技再生用オブジェクト探索
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
                SpecialTime = 5.0f;
            }
            
            if(pressed.IsSet(NetworkInputButtons.Attack))
            {
                SpecialTime2 = 5.0f;
            }

            //攻撃ボタンを押したときにコンボカウントが指定の数を超えてる場合再生
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
