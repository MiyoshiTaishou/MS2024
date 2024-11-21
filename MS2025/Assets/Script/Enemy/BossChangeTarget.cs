using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ChangeTargetAction", menuName = "Boss/Actions/TargetChange")]
public class BossChangeTarget : BossActionData
{
    [SerializeField, Header("�ǂ�����^�[�Q�b�g�ɂ��邩0��1P,1��2P")]
    private int taregt = 0;
    public override void InitializeAction(GameObject boss, Transform player)
    {
        boss.GetComponent<BossAI>().currentPlayerIndex = taregt;
    }

    public override bool ExecuteAction(GameObject boss, Transform player)
    {       
        return true;
    }
}
