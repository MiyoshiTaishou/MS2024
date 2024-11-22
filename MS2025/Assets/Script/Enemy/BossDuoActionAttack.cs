using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DuoAction", menuName = "Boss/Actions/DuoAttack")]
public class BossDuoActionAttack : BossActionData
{
    [SerializeField, Header("�A�N�V�������X�g")]
    private List<BossActionData> actionDatas = new List<BossActionData>();

    // �A�N�V�����̊�����Ԃ�ǐՂ��郊�X�g
    private HashSet<BossActionData> completedActions = new HashSet<BossActionData>();

    public override void InitializeAction(GameObject boss, Transform player)
    {
        // �A�N�V���������������A������Ԃ����Z�b�g
        completedActions.Clear();
        foreach (var actionData in actionDatas)
        {
            actionData.InitializeAction(boss, player);
        }
    }

    public override bool ExecuteAction(GameObject boss, Transform player)
    {
        bool allCompleted = true;

        foreach (var actionData in actionDatas)
        {
            // �������Ă��Ȃ��A�N�V�������������s
            if (!completedActions.Contains(actionData))
            {
                if (actionData.ExecuteAction(boss,player))
                {
                    Debug.Log(actionData + " �A�N�V��������");
                    completedActions.Add(actionData); // ������Ԃ��L�^
                }
                else
                {
                    allCompleted = false; // �܂��������̃A�N�V����������
                }
            }
        }

        // �S�ẴA�N�V�������������Ă���� true ��Ԃ�
        return allCompleted;
    }
}
