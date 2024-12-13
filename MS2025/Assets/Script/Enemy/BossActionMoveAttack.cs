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
    private float attackStartTime;
    private float moveStartTime;
    private Transform attackTarget;
    private Vector3 moveAttackEndPos;
    private Vector3 originalPosition;
    private bool isMoving;

    private bool isAttack = false;
    private bool isComp = false;  

    private Vector3 linkedImageOriginalPosition; // �摜�̌��̈ʒu

    private IEnumerator resetCoroutine; // ���Z�b�g�����p�R���[�`��

    public override void InitializeAction(GameObject boss, Transform player)
    {       
        attackTarget = boss.GetComponent<BossAI>().players[taregt];
        attackStartTime = Time.time;
        moveAttackEndPos = attackTarget.transform.position + deviate;      
      
        attackArea = GameObject.Find(attackAreaName)?.gameObject;
        originalPosition = attackArea.transform.position;
        attackArea.GetComponent<BoxCollider>().size = attackScale;
        attackArea.SetActive(true);
       
        isMoving = false;

        // �������� (�����̏���)
        float dis = Vector3.Distance(moveAttackEndPos, boss.transform.position);
        isAttack = (distance > dis);        

        // �{�X�̃A�j���[�V�����ݒ�
        boss.GetComponent<Animator>().speed = attackAnimSpeed;
        boss.GetComponent<BossAI>().isKnockBack = canKnockBack;
        boss.GetComponent<BossAI>().isParry = canParry;

        isComp = false;

        attackArea.GetComponent<BoxCollider>().enabled = true;    

        attackArea.GetComponent<MoveToBossObject>().RPC_SetToMove(false); 
        
        resetCoroutine = null;

        //�����ύX����
        if (boss.transform.position.x > attackTarget.position.x)
        {
            attackArea.GetComponent<MoveToBossObject>().RPC_SetDir(true);
        }
        else if (boss.transform.position.x < attackTarget.position.x)
        {
            attackArea.GetComponent<MoveToBossObject>().RPC_SetDir(false);
        }
    }   

    public override bool ExecuteAction(GameObject boss, Transform player)
    {
        if (Time.time - attackStartTime < attackDuration)
        {
            return false; // �U���ҋ@��
        }

        if (isComp)
        {
            return true; // �A�N�V��������
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

            Vector3 targetPosition = Vector3.Lerp(originalPosition, moveAttackEndPos, curveValue);
            attackArea.transform.position = targetPosition;

            if (progress >= 1.0f && resetCoroutine == null)
            {
                // ���Z�b�g�����J�n
                resetCoroutine = ResetToOriginalPosition();
                boss.GetComponent<MonoBehaviour>().StartCoroutine(resetCoroutine);
            }

            // ���Z�b�g��������������̂�҂�
            if (resetCoroutine != null && !isMoving)
            {
                resetCoroutine = null; // ������ɃR���[�`�������Z�b�g
                return true; // ���������ꍇ
            }

            return false; // ���Z�b�g���������Ă��Ȃ��ꍇ
        }
    }

    private IEnumerator ResetToOriginalPosition()
    {
        float resetStartTime = Time.time;
        float resetDuration = 0.5f;

        Vector3 attackAreaStartPosition = attackArea.transform.position;

        while (Time.time - resetStartTime < resetDuration)
        {
            float progress = (Time.time - resetStartTime) / resetDuration;
            attackArea.transform.position = Vector3.Lerp(attackAreaStartPosition, originalPosition, progress);
            yield return null;
        }

        attackArea.transform.position = originalPosition;
        attackArea.GetComponent<BoxCollider>().enabled = false;

        attackArea.GetComponent<MoveToBossObject>().RPC_SetToMove(true);

        isMoving = false;
        isComp = true;
    }

}
