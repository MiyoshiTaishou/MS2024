using UnityEngine;

[CreateAssetMenu(fileName = "AttackAction", menuName = "Boss/Actions/Attack")]
public class AttackAction : BossActionData
{
    public float attackRange;
    public float attackDuration;  // �U�����s���܂ł̑ҋ@����
    public float attackAnimSpeed;  // �A�j���[�V�����̑��x

    public string attackAreaName; // �U���G���A�̖��O�i�{�X�̎q�I�u�W�F�N�g�̖��O�j
    private GameObject attackArea; // �����̍U���G���A�̎Q��

    public AudioClip attackClip;

    private float attackStartTime;   

    private Transform attackTarget;

    public override void InitializeAction(GameObject boss, Transform player)
    {
        attackTarget = player;
        attackStartTime = Time.time;

        Debug.Log(player);

        // �{�X�̎q�I�u�W�F�N�g����U���G���A���擾
        attackArea = boss.transform.Find(attackAreaName)?.gameObject;
        boss.GetComponent<Animator>().speed = attackAnimSpeed;
    }

    public override bool ExecuteAction(GameObject boss)
    {       
        // �U���J�n�܂ł̎��Ԃ�ҋ@
        if (Time.time - attackStartTime < attackDuration)
        {
            // �U���ҋ@���ɉ�������̓�����������ꍇ�i��F�A�j���[�V�����Ȃǁj�A�����ɏ��������邱�Ƃ��ł��܂�
            return false; // �܂����s��
        }
        
        // �v���C���[���U���͈͓����ǂ������m�F
        if (Vector3.Distance(boss.transform.position, attackTarget.position) <= attackRange)
        {
            // �v���C���[���U���͈͓��Ȃ�U��
            Debug.Log("�U��");
            attackArea.SetActive(true);
        }
        else
        {
            // �͈͊O�ł���U��̍U�����s��
            Debug.Log("��U��");
            attackArea.SetActive(true);
        }

        // �����Đ�
        if (boss.GetComponent<AudioSource>() != null && attackClip != null)
        {
            boss.GetComponent<AudioSource>().clip = attackClip;
            boss.GetComponent<AudioSource>().Play();
        }

        boss.GetComponent<Animator>().speed = 1;

        return true; // �A�N�V��������
    }
}
