using Fusion;
using UnityEngine;

[CreateAssetMenu(fileName = "MoveRoundAction", menuName = "Boss/Actions/MoveRound")]
public class BossMoveRoundAction : BossActionData
{
    public float moveSpeed; // �~������鑬�x
    public float radius; // �~�̔��a
    public float angularSpeed; // �p���x (���W�A�����b)
    
    public Vector3 centerPoint; // �~�̒��S
    private float currentAngle; // ���݂̊p�x
    private float elapsedTime; // �o�ߎ���

    public float moveTime = 10.0f;

    public override void InitializeAction(GameObject boss, Transform player)
    {      
        // ���݂̊p�x�ƌo�ߎ��Ԃ����Z�b�g
        currentAngle = 0f;
        elapsedTime = 0f;
    }

    public override bool ExecuteAction(GameObject boss, Transform player)
    {
        // �o�ߎ��Ԃ��X�V
        elapsedTime += Time.deltaTime;

        // ���Ԍo�߂ɉ����Ċp�x��i�߂�
        currentAngle += angularSpeed * Time.deltaTime;

        // �~����̎��̈ʒu���v�Z
        float x = centerPoint.x + radius * Mathf.Cos(currentAngle);
        float z = centerPoint.z + radius * Mathf.Sin(currentAngle);

        Vector3 targetPosition = new Vector3(x, boss.transform.position.y, z);

        // �{�X�̈ʒu�𒼐ڐݒ肵�Ă��ꂢ�ȉ~��`��
        boss.transform.position = Vector3.Lerp(boss.transform.position, targetPosition, moveSpeed * Time.deltaTime);

        // ���̍s���Ɉڂ邩�ǂ����̔���i��: 5�b��Ɏ��̍s���j
        if (elapsedTime >= moveTime)
        {
            return true; // �A�N�V��������
        }

        return false; // ���s��
    }
}
