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

    int magnification = 2;

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

            //攻撃ボタンを押したときにコンボカウントが指定の数を超えてる場合再生
            if (pressed.IsSet(NetworkInputButtons.Attack) && comboCountObject.GetComponent<ShareNumbers>().nCombo == specialNum)
            {
                //倍率計算式
                magnification = (comboCountObject.GetComponent<ShareNumbers>().nCombo - 10) / 5 * 2 + 2;

                RPC_SpecialAttack();
            }

        }
    }

    [Rpc(RpcSources.All, RpcTargets.All)]
    public void RPC_SpecialAttack()
    {
        director.GetComponent<PlayableDirector>().Play();
        comboCountObject.GetComponent<ShareNumbers>().nCombo = 0;
    }
}
