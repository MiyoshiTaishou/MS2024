using UnityEngine;

[CreateAssetMenu(fileName = "JumpAction", menuName = "Boss/Actions/Jump")]
public class BossJumpAction : BossActionData
{
    public float jumpDuration;  // �󒆂ŗ��܂鎞��
    public float jumpPower;     // �W�����v��
    public float jumpHeight;    // �W�����v�̍�������
    public AudioClip attackClip;

    private float jumpStartTime;
    private bool isJumping;     // �W�����v�����ǂ���
    private bool atPeak;        // �ō����B�_�ɒB�������ǂ���
    private Vector3 startPos;

    private Rigidbody rb;

    private GameObject attackAreaView; // �����̍U���G���A�̎Q��

    [SerializeField,Header("���̍U��������")]
    private AttackAction attack;

    [SerializeField, Header("�A�j���[�V�����̑��x�@�ʏ킪2")]
    private float attackAnimSpeed;

    public override void InitializeAction(GameObject boss, Transform player)
    {
        rb = boss.GetComponent<Rigidbody>();

        // �W�����v�J�n�������L�^
        jumpStartTime = Time.time;
        isJumping = true;
        atPeak = false;

        // �W�����v�͂��u���ɓK�p
        Vector3 jumpForce = Vector3.up * jumpPower;
        rb.AddForce(jumpForce, ForceMode.Impulse);

        // �J�n�n�_�̋L�^
        startPos = boss.transform.position;

        boss.GetComponent<AudioSource>().clip = attackClip;
        boss.GetComponent<AudioSource>().Play();
        boss.GetComponent<BossAI>().isAir = true;

        attackAreaView = boss.transform.Find("Area")?.gameObject;
        // �U���G���A���v���C���[�����ɔz�u
        Vector3 directionToPlayer = (player.position - boss.transform.position).normalized; // �v���C���[�ւ̕����𐳋K��
        Vector3 attackPosition = boss.transform.position + directionToPlayer * attack.attackRange;      // �U���G���A�̐V�����ʒu       
        attackAreaView.transform.position = new Vector3(attackPosition.x, 2f, attackPosition.z);
        attackAreaView.GetComponent<PulsatingCircle>().SetMaxScale(attack.attackScale.x);
        attackAreaView.GetComponent<PulsatingCircle>().SetSpeed(attackAnimSpeed);
        attackAreaView.GetComponent<PulsatingCircle>().RPC_Active(true);
    }

    public override bool ExecuteAction(GameObject boss, Transform player)
    {
        // �U���G���A���v���C���[�����ɔz�u
        Vector3 directionToPlayer = (player.position - boss.transform.position).normalized; // �v���C���[�ւ̕����𐳋K��
        Vector3 attackPosition = boss.transform.position + directionToPlayer * attack.attackRange;      // �U���G���A�̐V�����ʒu       
        attackAreaView.transform.position = new Vector3(attackPosition.x, 2f, attackPosition.z);
        attackAreaView.GetComponent<PulsatingCircle>().SetMaxScale(attack.attackScale.x);

        if (isJumping)
        {
            Vector3 nowPos = boss.transform.position;

            // Y�̋������v��
            float currentHeight = nowPos.y - startPos.y;

            //Debug.Log(currentHeight + "���̍���" + jumpHeight + "�ڕW�̍���");

            // �w�肵�������ɒB������
            if (currentHeight >= jumpHeight && !atPeak)
            {                
                // �ō����B�_�ɒB�����̂ő��x���[���ɂ��A�t���t���Ƌ󒆂ŗ��܂�
                rb.velocity = Vector3.zero;
                rb.useGravity = false; // �d�͂𖳌������ċ󒆂ɗ��܂�
                boss.transform.position = new Vector3(nowPos.x, startPos.y + jumpHeight, nowPos.z); // �������Œ�

                atPeak = true;         // �ō����B�_�ɒB�������Ƃ��L�^
                jumpStartTime = Time.time; // �����Ă��鎞�Ԃ̌v�������Z�b�g              
            }

            // ��莞�Ԃ��o�߂�����A�d�͂����ɖ߂��ė���������
            if (atPeak && Time.time - jumpStartTime >= jumpDuration)
            {
                rb.useGravity = true; // �d�͂�L���ɂ��čĂї���
                isJumping = false;    // �W�����v�I��
            }

            return false;
        }
        else
        {
            if(rb.velocity.y == 0.0f)
            {
                boss.GetComponent<BossAI>().isAir = false;
                return true; // �W�����v���I�������� true ��Ԃ�
            }
            else
            {
                return false;
            }
        }       
    }
}
