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

    private GameObject attackAreaView; // �����̍U���G���A�̎Q��

    [SerializeField, Header("���̍U��������")]
    private AttackAction attack;

    [SerializeField, Header("�A�j���[�V�����̑��x�@�ʏ킪2")]
    private float attackAnimSpeed;

    public override void InitializeAction(GameObject boss, Transform player)
    {
        // �W�����v�J�n�������L�^
        jumpStartTime = Time.time;
        isJumping = true;
        atPeak = false;

        // �J�n�n�_�̋L�^
        startPos = boss.transform.position;

        // �W�����v�͂��v�Z
        float targetY = startPos.y + jumpHeight;

        boss.GetComponent<AudioSource>().clip = attackClip;
        boss.GetComponent<AudioSource>().Play();
        boss.GetComponent<BossAI>().isAir = true;

        attackAreaView = boss.transform.Find("Area")?.gameObject;

        // �U���G���A���v���C���[�����ɔz�u
        Vector3 directionToPlayer = (player.position - boss.transform.position).normalized; // �v���C���[�ւ̕����𐳋K��
        Vector3 attackPosition = boss.transform.position + directionToPlayer * attack.attackRange;      // �U���G���A�̐V�����ʒu       
        attackAreaView.transform.position = new Vector3(attackPosition.x, 2f, attackPosition.z);
        attackAreaView.GetComponent<PulsatingCircle>().RPC_Scale(attack.attackScale.x);
        attackAreaView.GetComponent<PulsatingCircle>().RPC_Spedd(attackAnimSpeed);
        attackAreaView.GetComponent<PulsatingCircle>().RPC_Active(true);
    }

    public override bool ExecuteAction(GameObject boss, Transform player)
    {
        Vector3 position = boss.transform.position;

        // �U���G���A���v���C���[�����ɔz�u
        Vector3 directionToPlayer = (player.position - position).normalized; // �v���C���[�ւ̕����𐳋K��
        Vector3 attackPosition = position + directionToPlayer * attack.attackRange;      // �U���G���A�̐V�����ʒu       
        attackAreaView.transform.position = new Vector3(attackPosition.x, 2f, attackPosition.z);
        attackAreaView.GetComponent<PulsatingCircle>().RPC_Scale(attack.attackScale.x);

        if (isJumping)
        {
            float currentHeight = position.y - startPos.y;

            if (!atPeak)
            {
                // �㏸���̏���
                position.y += jumpPower * Time.deltaTime;

                if (currentHeight >= jumpHeight)
                {
                    position.y = startPos.y + jumpHeight; // �����𐧌�
                    atPeak = true;
                    jumpStartTime = Time.time; // �󒆑؍ݎ��Ԃ̌v�����J�n
                }
            }
            else
            {
                // �󒆑؍ݒ��̏���
                if (Time.time - jumpStartTime >= jumpDuration)
                {
                    isJumping = false; // �����J�n
                }
            }
        }
        else
        {
            // �������̏���
            position.y -= jumpPower * Time.deltaTime;

            if (position.y <= startPos.y)
            {
                position.y = startPos.y; // �n�ʂŒ�~
                boss.GetComponent<BossAI>().isAir = false;
                return true; // �W�����v����
            }
        }

        // �{�X�̈ʒu���X�V
        boss.transform.position = position;

        return false;
    }
}
