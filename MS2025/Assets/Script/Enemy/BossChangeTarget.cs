using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ChangeTargetAction", menuName = "Boss/Actions/TargetChange")]
public class BossChangeTarget : BossActionData
{
    [SerializeField, Header("どちらをターゲットにするか0が1P,1が2P")]
    private int taregt = 0;
    public override void InitializeAction(GameObject boss, Transform player)
    {
        if (boss.GetComponent<BossAI>().currentPlayerIndex == 0)
        {
            boss.GetComponent<BossAI>().currentPlayerIndex = 1;
        }
        else
        {
            boss.GetComponent<BossAI>().currentPlayerIndex = 0;
        }

        //boss.GetComponent<BossAI>().currentPlayerIndex = taregt;
    }

    public override bool ExecuteAction(GameObject boss, Transform player)
    {       
        return true;
    }
}
