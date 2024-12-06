using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �U���̔��肪�ړ�����U��
/// </summary>
[CreateAssetMenu(fileName = "MoveAttackAction", menuName = "Boss/Actions/MoveAttack")]
public class BossActionMoveAttack : BossActionData
{
    [SerializeField, Header("�ǂ�����^�[�Q�b�g�ɂ��邩0��1P,1��2P")]
    private int taregt = 0;

    [SerializeField, Header("�U�����ړ����鎞��")]
    private float moveAttackEndPosTime = 3.0f;

    [SerializeField, Header("�ǂꂮ�炢����Ă���U�����邩")]
    private float distance = 5.0f;

    [SerializeField, Header("�U�����ǂꂾ������邩")]
    private Vector3 deviate;

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
    private GameObject attackAreaImage;
    private float attackStartTime;
    private float moveStartTime;
    private Transform attackTarget;
    private Vector3 moveAttackEndPos;
    private Vector3 originalPosition;
    private bool isMoving;

    private bool isAttack = false;

    [SerializeField, Header("�U���G���A�ɘA������摜�I�u�W�F�N�g")]
    private string linkedImage; // �����������摜�I�u�W�F�N�g

    private Vector3 linkedImageOriginalPosition; // �摜�̌��̈ʒu

    public override void InitializeAction(GameObject boss, Transform player)
    {
        // (�����̏���)
        attackTarget = boss.GetComponent<BossAI>().players[taregt];
        attackStartTime = Time.time;
        moveAttackEndPos = attackTarget.transform.position + deviate;

        // �U���G���A�̐ݒ�
        attackArea = boss.transform.Find(attackAreaName)?.gameObject;
        attackAreaImage = boss.transform.Find(linkedImage)?.gameObject;
        originalPosition = attackArea.transform.position;
        attackArea.transform.localScale = attackScale;
        attackArea.SetActive(true);

        // �摜�I�u�W�F�N�g�̌��̈ʒu���L�^
        if (linkedImage != null)
        {
            linkedImageOriginalPosition = attackAreaImage.transform.position;
        }

        isMoving = false;

        // �������� (�����̏���)
        float dis = Vector3.Distance(moveAttackEndPos, boss.transform.position);
        isAttack = (distance > dis);

        if (isAttack)
        {
            attackArea.GetComponent<BossAttackArea>().deactivateTime = 0.5f;
        }
        else
        {
            attackArea.GetComponent<BossAttackArea>().deactivateTime = moveAttackEndPosTime;
        }

        // �{�X�̃A�j���[�V�����ݒ�
        boss.GetComponent<Animator>().speed = attackAnimSpeed;
        boss.GetComponent<BossAI>().isKnockBack = canKnockBack;
        boss.GetComponent<BossAI>().isParry = canParry;
    }

    public override bool ExecuteAction(GameObject boss, Transform player)
    {
        if (Time.time - attackStartTime < attackDuration)
        {
            return false; // �U���ҋ@��
        }

        if (isAttack)
        {
            attackArea.SetActive(true);

            if (boss.GetComponent<AudioSource>() != null && attackClip != null)
            {
                boss.GetComponent<AudioSource>().clip = attackClip;
                boss.GetComponent<AudioSource>().Play();
            }

            boss.GetComponent<Animator>().speed = 1;
            return true;
        }
        else
        {
            if (!isMoving)
            {
                isMoving = true;
                moveStartTime = Time.time;
                attackArea.SetActive(true);

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

            // �U���G���A�Ɖ摜�I�u�W�F�N�g�̈ړ�
            attackArea.transform.position = Vector3.Lerp(originalPosition, moveAttackEndPos, curveValue);
            if (linkedImage != null)
            {
                attackAreaImage.transform.position = Vector3.Lerp(linkedImageOriginalPosition, moveAttackEndPos, curveValue);
            }

            // �����蔻��`�F�b�N
            if (CheckForHit(attackArea))
            {
                if (isCameraShake)
                {
                    boss.GetComponent<HitStop>().ApplyHitStop(60000);
                    Camera.main.GetComponent<CameraShake>().RPC_CameraShake(cameraDuration, magnitude);
                }
            }

            if (progress >= 1.0f)
            {
                boss.GetComponent<MonoBehaviour>().StartCoroutine(ResetToOriginalPosition());
                return false; // ���Z�b�g����������܂ł�false��Ԃ�
            }

            return false;
        }
    }


    // �񓯊��ōU���G���A�Ɖ摜�����̈ʒu�ɖ߂�
    private IEnumerator ResetToOriginalPosition()
    {
        float resetStartTime = Time.time;
        float resetDuration = 0.5f; // ���̈ʒu�ɖ߂�܂ł̎���

        Vector3 attackAreaStartPosition = attackArea.transform.position;
        Vector3 linkedImageStartPosition = linkedImage != null ? attackAreaImage.transform.position : Vector3.zero;

        while (Time.time - resetStartTime < resetDuration)
        {
            float progress = (Time.time - resetStartTime) / resetDuration;

            // �U���G���A�����[�v�Ō��̈ʒu�ɖ߂�
            attackArea.transform.position = Vector3.Lerp(attackAreaStartPosition, originalPosition, progress);

            // �摜�I�u�W�F�N�g�����[�v�Ō��̈ʒu�ɖ߂�
            if (linkedImage != null)
            {
                attackAreaImage.transform.position = Vector3.Lerp(linkedImageStartPosition, linkedImageOriginalPosition, progress);
            }

            yield return null;
        }

        // �Ō�Ɋm���Ɉʒu�����ɖ߂�
        attackArea.transform.position = originalPosition;
        attackArea.SetActive(false);

        if (linkedImage != null)
        {
            attackAreaImage.transform.position = linkedImageOriginalPosition;
        }

        isMoving = false;

        yield return true; // ������ʒm
    }


    // �U���q�b�g����
    private bool CheckForHit(GameObject attackArea)
    {
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