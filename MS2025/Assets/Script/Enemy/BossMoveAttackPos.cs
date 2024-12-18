using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �U���̔��肪�ړ�����U��
/// </summary>
[CreateAssetMenu(fileName = "MoveAttackPos", menuName = "Boss/Actions/MoveAttackPos")]
public class BossMoveAttackPos : BossActionData
{  
    [SerializeField, Header("�ړ����鎞��")]
    private float moveAttackEndPosTime = 3.0f;

    [SerializeField, Header("��������")]
    private Vector3 EndPosition = Vector3.zero;

    [SerializeField, Header("������")]
    private float rotPunch;

    [SerializeField, Header("�A�j���[�V�����J�[�u�ňړ������b�`�ɂ���")]
    private AnimationCurve curve; 

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

    [SerializeField, Header("�U���G���A�̖��O")]
    public string attackAreaName;

    [SerializeField, Header("�p���B�s�\���ǂ���")]
    private bool canParry;

    [SerializeField, Header("�m�b�N�o�b�N�\���ǂ���")]
    private bool canKnockBack;

    [SerializeField, Header("�ړ��s�ɂ���")]
    private bool canMove;

    [SerializeField, Header("�U���̔���")]
    private PARRYTYPE parryType;

    public AudioClip attackClip;

    private GameObject attackArea;
    private float attackStartTime;
    private float moveStartTime;    
    private bool isMoving;
    private Vector3 originalPosition;

    private bool isAttack = false;
    private bool isComp = false;

    private Vector3 linkedImageOriginalPosition; // �摜�̌��̈ʒu

    private IEnumerator resetCoroutine; // ���Z�b�g�����p�R���[�`��

    private GameObject attackAreaView; // �����̍U���G���A�̎Q��

    public override void InitializeAction(GameObject boss, Transform player)
    {        
        attackStartTime = Time.time;       

        attackArea = GameObject.Find(attackAreaName)?.gameObject;
        originalPosition = attackArea.transform.position;
        attackArea.GetComponent<BoxCollider>().size = attackScale;
        attackArea.SetActive(true);

        attackArea.transform.rotation = Quaternion.identity;             
        attackArea.transform.rotation = Quaternion.Euler(0, 0, rotPunch);

        //attackArea.transform.localRotation = rotPunch2;

        isMoving = false;

        attackArea.GetComponent<BossAttackArea2Boss>().Type = parryType;

        // �{�X�̃A�j���[�V�����ݒ�
        boss.GetComponent<Animator>().speed = attackAnimSpeed;
        boss.GetComponent<BossAI>().isKnockBack = canKnockBack;
        boss.GetComponent<BossAI>().isParry = canParry;

        attackAreaView = boss.transform.Find("Area")?.gameObject;

        isComp = false;

        attackArea.GetComponent<BoxCollider>().enabled = true;

        attackArea.GetComponent<MoveToBossObject>().RPC_SetToMove(false);

        resetCoroutine = null;

        Camera.main.GetComponent<CameraShake>().RPC_CameraShake(cameraDuration, magnitude);
        attackAreaView.transform.position = new Vector3(EndPosition.x, 2f, EndPosition.z);
        attackAreaView.GetComponent<PulsatingCircle>().RPC_Scale(attackScale.z);
        attackAreaView.GetComponent<PulsatingCircle>().RPC_Spedd(attackAnimSpeed);
        attackAreaView.GetComponent<PulsatingCircle>().RPC_Active(true);
    }

    public override bool ExecuteAction(GameObject boss, Transform player)
    {       
        if (isComp)
        {
            return true; // �A�N�V��������
        }
      
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

            Vector3 targetPosition = Vector3.Lerp(originalPosition, EndPosition, curveValue);
            attackArea.transform.position = targetPosition;

            if (progress >= 1.0f && resetCoroutine == null && canMove)
            {
                // ���Z�b�g�����J�n
                //resetCoroutine = ResetToOriginalPosition();
                //boss.GetComponent<MonoBehaviour>().StartCoroutine(resetCoroutine);
                attackArea.GetComponent<MoveToBossObject>().RPC_SetToMove(true);
                attackAreaView.GetComponent<PulsatingCircle>().RPC_Active(false);
                return true;
            }
            else if(progress >= 1.0f && resetCoroutine == null && !canMove)
            {
                return true;
            }

            // ���Z�b�g��������������̂�҂�
            if (resetCoroutine != null && !isMoving)
            {
                resetCoroutine = null; // ������ɃR���[�`�������Z�b�g
                return true; // ���������ꍇ
            }

            return false; // ���Z�b�g���������Ă��Ȃ��ꍇ        
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
        attackAreaView.GetComponent<PulsatingCircle>().RPC_Active(false);

        isMoving = false;
        isComp = true;
    }

}
