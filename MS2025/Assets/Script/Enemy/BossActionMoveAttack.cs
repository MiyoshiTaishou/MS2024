using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �U���̔��肪�ړ�����U��
/// </summary>
[CreateAssetMenu(fileName = "MoveAttackAction", menuName = "Boss/Actions/MoveAttack")]
public class BossActionMoveAttack : BossActionData
{
    [SerializeField, Header("�U�����ړ����鎞��")]
    private float moveAttackEndPosTime = 3.0f;

    [SerializeField, Header("�A�j���[�V�����J�[�u�ňړ������b�`�ɂ���")]
    private AnimationCurve curve;

    [SerializeField, Header("�U���J�n����܂ł̑ҋ@����")]
    private float attackDuration;

    [SerializeField, Header("�A�j���[�V�����̑��x")]
    private float attackAnimSpeed;

    [SerializeField, Header("�U���̓����蔻��̑傫��")]
    private Vector3 attackScale;

    [SerializeField, Header("�J������h�炷������K�p���邩")]
    private bool isCameraShake;

    [SerializeField, Header("�J�����h��̎���")]
    private float cameraDuration;

    [SerializeField, Header("�h��̋���")]
    private float magnitude;

    [SerializeField, Header("�p���B�s�\���ǂ���")]
    private bool canParry;

    [SerializeField, Header("�m�b�N�o�b�N�\���ǂ���")]
    private bool canKnockBack;

    [SerializeField, Header("�U���G���A�̖��O")]
    public string attackAreaName;

    public AudioClip attackClip;

    private GameObject attackArea;
    private float attackStartTime;
    private float moveStartTime;
    private Transform attackTarget;
    private Vector3 moveAttackEndPos;
    private Vector3 originalPosition;
    private bool isMoving;

    public override void InitializeAction(GameObject boss, Transform player)
    {
        attackTarget = player;
        attackStartTime = Time.time;
        moveAttackEndPos = player.transform.position;

        // �U���G���A�̐ݒ�
        attackArea = boss.transform.Find(attackAreaName)?.gameObject;
        originalPosition = attackArea.transform.position;
        attackArea.transform.localScale = attackScale;
        attackArea.SetActive(false);
        isMoving = false;

        // �{�X�̃A�j���[�V�����ݒ�
        boss.GetComponent<Animator>().speed = attackAnimSpeed;
        boss.GetComponent<BossAI>().isKnockBack = canKnockBack;
        boss.GetComponent<BossAI>().isParry = canParry;
    }

    public override bool ExecuteAction(GameObject boss)
    {
        // �U���J�n�܂ł̑ҋ@
        if (Time.time - attackStartTime < attackDuration)
        {
            return false;
        }

        // �ړ��J�n���̏�����
        if (!isMoving)
        {
            isMoving = true;
            moveStartTime = Time.time;
            attackArea.SetActive(true);

            // �����Đ�
            if (boss.GetComponent<AudioSource>() != null && attackClip != null)
            {
                boss.GetComponent<AudioSource>().clip = attackClip;
                boss.GetComponent<AudioSource>().Play();
            }
        }

        // �ړ�����
        float elapsed = Time.time - moveStartTime;
        float progress = elapsed / moveAttackEndPosTime;
        float curveValue = curve.Evaluate(progress);

        attackArea.transform.position = Vector3.Lerp(originalPosition, moveAttackEndPos, curveValue);

        // �v���C���[�ւ̍U�����q�b�g�������A�ړ������������ꍇ
        if (progress >= 1.0f || CheckForHit(attackArea))
        {
            ResetAttackArea();
            return true; // �A�N�V��������
        }

        return false; // �A�N�V�����p����
    }

    // �U���G���A�����̈ʒu�ɖ߂��Ĕ�A�N�e�B�u��
    private void ResetAttackArea()
    {
        attackArea.transform.position = originalPosition;
        attackArea.SetActive(false);
        isMoving = false;
    }

    // �U���q�b�g����̗�i���ۂ̃q�b�g���菈���ɉ����ĕύX���邱�Ɓj
    private bool CheckForHit(GameObject attackArea)
    {
        // �v���C���[���U���G���A���ɂ��邩�ǂ����̔���
        Collider[] hits = Physics.OverlapBox(attackArea.transform.position, attackArea.transform.localScale / 2);
        foreach (var hit in hits)
        {
            if (hit.CompareTag("Player"))
            {
                Debug.Log("�U�����q�b�g���܂����I");
                return true;
            }
        }
        return false;
    }
}
