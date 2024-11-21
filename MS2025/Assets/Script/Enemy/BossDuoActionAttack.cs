using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DuoAction", menuName = "Boss/Actions/DuoAttack")]
public class BossDuoActionAttack : BossActionData
{
    // �z�񂩂烊�X�g�ɕύX
    [SerializeField,Header("�A�N�V�������X�g")]
    private List<BossActionData> actionDatas = new List<BossActionData>();

    public override void InitializeAction(GameObject boss, Transform player)
    {
        foreach (var actionData in actionDatas)
        {
            actionData.InitializeAction(boss, player);
        }
    }

    public override bool ExecuteAction(GameObject boss)
    {
        // ���s���ɍ폜���邽�߁A�ꎞ���X�g�Ɉړ�
        List<BossActionData> completedActions = new List<BossActionData>();

        foreach (var actionData in actionDatas)
        {
            if (actionData.ExecuteAction(boss))
            {
                Debug.Log(actionData + "�A�N�V��������");
                // ���������A�N�V�������L�^
                completedActions.Add(actionData);
            }
        }

        // ���������A�N�V���������X�g����폜
        foreach (var completedAction in completedActions)
        {
            int num = 0;
            num++;

            Debug.Log(num + "�A�N�V����������");

            return num == completedActions.Count;
        }
       
        return false;
    }
}
