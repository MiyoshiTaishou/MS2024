using UnityEngine;

[CreateAssetMenu(fileName = "JumpAction", menuName = "Boss/Actions/Jump")]
public class BossJumpAction : BossActionData
{
    public float jumpDuration;  // �󒆂ŗ��܂鎞��
    public float jumpPower;     // �W�����v��
    public float jumpHeight;    // �W�����v�̍�������

    private float jumpStartTime;
    private bool isJumping;     // �W�����v�����ǂ���
    private bool atPeak;        // �ō����B�_�ɒB�������ǂ���
    private Vector3 startPos;

    private Rigidbody rb;

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
    }

    public override bool ExecuteAction(GameObject boss)
    {
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
                return true; // �W�����v���I�������� true ��Ԃ�
            }
            else
            {
                return false;
            }
        }       
    }
}
