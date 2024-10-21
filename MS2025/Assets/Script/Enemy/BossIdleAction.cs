using UnityEngine;

[CreateAssetMenu(fileName = "WaitAction", menuName = "Boss/Actions/Wait")]
public class WaitAction : BossActionData
{
    public float waitDuration;
    private float waitStartTime;

    public override void InitializeAction(GameObject boss)
    {
        waitStartTime = Time.time;
    }

    public override bool ExecuteAction(GameObject boss)
    {
        if (Time.time - waitStartTime >= waitDuration)
        {
            return true; // �A�N�V��������
        }
        return false; // �܂����s��
    }
}
