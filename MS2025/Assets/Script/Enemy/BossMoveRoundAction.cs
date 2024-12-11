using Fusion;
using UnityEngine;

[CreateAssetMenu(fileName = "MoveRoundAction", menuName = "Boss/Actions/MoveRound")]
public class BossMoveRoundAction : BossActionData
{
    public float moveSpeed; // �ړ����x
    public float range; // �����_���ړ��͈�

    public Vector3 minBounds; // �ړ��͈͂̍ŏ����W
    public Vector3 maxBounds; // �ړ��͈͂̍ő���W

    private Vector3 targetPosition; // ���̈ړ���
    private float elapsedTime; // �o�ߎ���
    public float moveTime = 10.0f; // �s���̎��s����

    public override void InitializeAction(GameObject boss, Transform player)
    {
        // �o�ߎ��Ԃ����Z�b�g
        elapsedTime = 0f;

        // �ŏ��̃����_���ȖڕW�n�_��ݒ�
        targetPosition = GetRandomPosition(boss.transform.position);
    }

    public override bool ExecuteAction(GameObject boss, Transform player)
    {
        // �o�ߎ��Ԃ��X�V
        elapsedTime += Time.deltaTime;

        // �{�X��ڕW�n�_�Ɉړ�������
        boss.transform.position = Vector3.MoveTowards(boss.transform.position, targetPosition, moveSpeed * Time.deltaTime);

        // �͈͐�����K�p
        boss.transform.position = ApplyBounds(boss.transform.position);

        // �ڕW�n�_�ɓ��B�����ꍇ�A�V�����ڕW�n�_��ݒ�
        if (Vector3.Distance(boss.transform.position, targetPosition) < 0.1f)
        {
            targetPosition = GetRandomPosition(boss.transform.position);
        }

        // ���̍s���Ɉڂ邩�ǂ����̔���
        if (elapsedTime >= moveTime)
        {
            return true; // �A�N�V��������
        }

        return false; // ���s��
    }

    private Vector3 GetRandomPosition(Vector3 currentPosition)
    {
        // �����_���ȃI�t�Z�b�g���v�Z
        float randomX = Random.Range(-range, range);
        float randomZ = Random.Range(-range, range);

        // �V�����ڕW�n�_��ݒ肵�A�͈͓��Ɏ��߂�
        Vector3 newPosition = new Vector3(currentPosition.x + randomX, currentPosition.y, currentPosition.z + randomZ);
        return ApplyBounds(newPosition);
    }

    private Vector3 ApplyBounds(Vector3 position)
    {
        // �͈͐�����K�p���A���W���C��
        if (position.x < minBounds.x)
        {
            position.x = maxBounds.x;
        }
        else if (position.x > maxBounds.x)
        {
            position.x = minBounds.x;
        }

        if (position.z < minBounds.z)
        {
            position.z = maxBounds.z;
        }
        else if (position.z > maxBounds.z)
        {
            position.z = minBounds.z;
        }

        return position;
    }
}
