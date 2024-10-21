using Fusion;
using UnityEngine;

[CreateAssetMenu(fileName = "MoveAction", menuName = "Boss/Actions/Move")]
public class BossMoveAction : BossActionData
{
    public float moveSpeed;
    public float stoppingDistance;   

    private Transform player;
    private Rigidbody bossRigidbody;

    public override void InitializeAction(GameObject boss)
    {
        // Rigidbody �̎Q�Ƃ��擾
        bossRigidbody = boss.GetComponent<Rigidbody>();
        player = GameObject.FindWithTag("Player").transform;       
    }

    public override bool ExecuteAction(GameObject boss)
    {
        if (player == null || bossRigidbody == null)
            return false;

        // �v���C���[�܂ł̋������v�Z
        Vector3 directionToPlayer = player.position - boss.transform.position;
        float distanceToPlayer = directionToPlayer.magnitude;

        // �w�苗���܂ŋ߂Â��Ă�����A�ړ����I��
        if (distanceToPlayer <= stoppingDistance)
        {
            bossRigidbody.velocity = Vector3.zero; // �{�X���~������
            return true; // �A�N�V��������
        }

        // �v���C���[�̕��ֈړ��i���K�����ꂽ�����x�N�g���ɃX�s�[�h���|����j
        Vector3 moveDirection = directionToPlayer.normalized * moveSpeed;
        bossRigidbody.velocity = new Vector3(moveDirection.x, bossRigidbody.velocity.y, moveDirection.z); // Y���̑��x�͈ێ�

        return false; // �܂����s��
    }
}
