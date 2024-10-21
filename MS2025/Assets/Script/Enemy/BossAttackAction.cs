using UnityEngine;

[CreateAssetMenu(fileName = "AttackAction", menuName = "Boss/Actions/Attack")]
public class AttackAction : BossActionData
{
    public float attackRange;
    public float attackDuration;

    private float attackStartTime;
    private Transform player;

    public override void InitializeAction(GameObject boss)
    {
        player = GameObject.FindWithTag("Player").transform;
        attackStartTime = Time.time;
    }

    public override bool ExecuteAction(GameObject boss)
    {
        // �v���C���[���U���͈͓����ǂ����𔻒�
        if (Vector3.Distance(boss.transform.position, player.position) <= attackRange)
        {
            if (Time.time - attackStartTime >= attackDuration)
            {
                // �U�������̊����i��F�v���C���[�Ƀ_���[�W��^����j
                Debug.Log("Attacking Player!");
                return true; // �A�N�V��������
            }
        }
        else
        {
            Debug.Log("Player is out of range");
        }
        return false; // �܂����s��
    }
}
