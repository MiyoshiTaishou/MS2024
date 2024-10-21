using UnityEngine;

[CreateAssetMenu(fileName = "AttackAction", menuName = "Boss/Actions/Attack")]
public class AttackAction : BossActionData
{
    public float attackRange;
    public float attackDuration;   

    public AudioClip attackClip;
  
    private float attackStartTime;
    private Transform player;

    public override void InitializeAction(GameObject boss)
    {
        player = GameObject.FindWithTag("Player").transform;
        attackStartTime = Time.time;        
    }

    public override bool ExecuteAction(GameObject boss)
    {
        //�v���C���[�����������O�Ɏ��s����邱�Ƃ�����̂Ŗ����ꍇ�͂Ƃ肠�����s�����I��点��
        if(player == null)
        {
            return true;
        }

        // �v���C���[���U���͈͓����ǂ����𔻒�
        if (Vector3.Distance(boss.transform.position, player.position) <= attackRange)
        {
            if (Time.time - attackStartTime >= attackDuration)
            {
                // �U�������̊����i��F�v���C���[�Ƀ_���[�W��^����j
                Debug.Log("Attacking Player!");
                boss.GetComponent<AudioSource>().clip = attackClip;
                boss.GetComponent<AudioSource>().Play();
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
