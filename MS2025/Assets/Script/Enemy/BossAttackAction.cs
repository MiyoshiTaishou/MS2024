using UnityEngine;

[CreateAssetMenu(fileName = "AttackAction", menuName = "Boss/Actions/Attack")]
public class AttackAction : BossActionData
{
    [SerializeField,Header("�U�����͂��͈͕ʂɂȂ��Ă���������")]
    private float attackRange;

    [SerializeField, Header("�U���J�n����܂ł̑ҋ@���ԁ@�A�j���[�V�����̑��x��1�Ȃ�1.4")]
    private float attackDuration;

    [SerializeField, Header("�A�j���[�V�����̑��x�@�ʏ킪1")]
    private float attackAnimSpeed;

    [SerializeField, Header("�U���̓����蔻��̑傫��")]
    private Vector3 attackScale;

    [SerializeField, Header("�J������h�炷������K�p���邩�ǂ���")]
    private bool isCameraShake;

    [SerializeField, Header("�����܂ŗh�炷��")]
    private float cameraDuration;

    [SerializeField, Header("�h��̋���")]
    private float magnitude;

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
        attackArea.transform.localScale = attackScale;
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

        //�J������h�炷����
        if(isCameraShake)
        {
            boss.GetComponent<HitStop>().ApplyHitStop(60000);
            Camera.main.GetComponent<CameraShake>().RPC_CameraShake(cameraDuration, magnitude);           
        }

        boss.GetComponent<Animator>().speed = 1;

        return true; // �A�N�V��������
    }
}
