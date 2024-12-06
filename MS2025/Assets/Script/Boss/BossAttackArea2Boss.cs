using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttackArea2Boss : NetworkBehaviour
{
    GameObject box;
    GameObject parent;

    [SerializeField]
    public float deactivateTime = 0.5f; // �U���G���A�𖳌�������܂ł̎���
    [SerializeField]
    private float returnDuration = 1.0f; // ���̈ʒu�ɖ߂�̂ɂ����鎞��

    [Networked] private float timer { get; set; }
    [Networked] private bool isAttackActive { get; set; } // �U�����L�����ǂ���

    private ParticleSystem newParticle;
    [Tooltip("�U���G�t�F�N�g")]
    public ParticleSystem AttackParticle;

    private GameObject Pare;
    private Vector3 originalPosition;

    [Networked] public PARRYTYPE Type { get; set; }
    [Networked] public bool isTanuki { get; set; }
    [Networked] public bool isKitune { get; set; }

    private bool isReturningToPosition = false;
    private float returnTimer = 0f; // ���̈ʒu�ɖ߂�ۂ̃^�C�}�[
    private Vector3 startPosition; // ���݂̈ʒu���L�^

    public override void Spawned()
    {
        box = GameObject.Find("Networkbox");
        parent = transform.parent.gameObject;
        timer = deactivateTime;
        Pare = transform.parent.gameObject;
        isTanuki = false;
        isKitune = false;
        isAttackActive = false; // ������Ԃ͖�����
        originalPosition = transform.position;
    }

    public void OnEnable()
    {
        Debug.Log("�U���G���A�L����");
        SetAttackActive(true);
    }

    public override void Render()
    {
        if (isAttackActive)
        {
            // �p�[�e�B�N���V�X�e���̃C���X�^���X�𐶐�
            newParticle = Instantiate(AttackParticle);
            if (Pare.transform.localScale.x >= 0)
            {
                newParticle.transform.position = new Vector3(transform.position.x - 4.0f, transform.position.y - 2.0f, transform.position.z);
            }
            else
            {
                newParticle.transform.position = new Vector3(transform.position.x + 4.0f, transform.position.y - 2.0f, transform.position.z);
            }

            newParticle.Play();
            Destroy(newParticle.gameObject, 1f);
        }
    }

    public void SetAttackActive(bool isActive)
    {
        isAttackActive = isActive;
        if (isActive)
        {
            timer = deactivateTime; // �^�C�}�[�����Z�b�g
        }
        else
        {
            StartReturningToPosition(); // ���������Ɍ��̈ʒu�ɖ߂鏈�����J�n
        }
    }

    private void StartReturningToPosition()
    {
        isReturningToPosition = true;
        returnTimer = 0f;
        startPosition = transform.position;
    }

    private void ReturnToOriginalPosition()
    {
        if (!isReturningToPosition) return;

        returnTimer += Runner.DeltaTime;
        float t = returnTimer / returnDuration;
        transform.position = Vector3.Lerp(startPosition, originalPosition, t);

        if (t >= 1f)
        {
            isReturningToPosition = false; // ����������t���O�����Z�b�g
            transform.position = originalPosition; // �ŏI�I�ɐ��m�Ȍ��̈ʒu�ɃZ�b�g
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isAttackActive) return; // �U���������Ȃ珈�����X�L�b�v

        if (other.CompareTag("Player"))
        {
            timer = deactivateTime;

            // �p���B����
            if (!parent.GetComponent<BossAI>().isParry)
            {
                if (other.GetComponent<PlayerParryNet>().ParryCheck() &&
                    ((other.GetComponent<PlayerParryNet>().isTanuki && Type == PARRYTYPE.TANUKI) ||
                    (!other.GetComponent<PlayerParryNet>().isTanuki && Type == PARRYTYPE.KITUNE) ||
                    Type == PARRYTYPE.ALL))
                {
                    Debug.Log("�p���B����");
                    other.GetComponent<PlayerParryNet>().RPC_ParrySystem();

                    if (parent.GetComponent<BossAI>().isKnockBack)
                    {
                        parent.GetComponent<BossAI>().RPC_AnimName();
                    }

                    SetAttackActive(false);
                    return;
                }
                else if (other.GetComponent<PlayerParryNet>().ParryCheck() && Type == PARRYTYPE.DOUBLE)
                {
                    if (other.GetComponent<PlayerParryNet>().isTanuki)
                    {
                        isTanuki = true;
                    }
                    else if (!other.GetComponent<PlayerParryNet>().isTanuki)
                    {
                        isKitune = true;
                    }
                    if (isTanuki && isKitune)
                    {
                        Debug.Log("�p���B����");
                        other.GetComponent<PlayerParryNet>().RPC_ParrySystem();

                        if (parent.GetComponent<BossAI>().isKnockBack)
                        {
                            parent.GetComponent<BossAI>().RPC_AnimName();
                        }

                        SetAttackActive(false);
                        return;
                    }
                }
            }

            // �_���[�W����
            Debug.Log("�U���q�b�g");
            if (other.GetComponent<PlayerHP>().inbisibleFrame == 0)
            {
                box.GetComponent<ShareNumbers>().CurrentHP--;
                box.GetComponent<ShareNumbers>().RPC_Damage();
                other.GetComponent<PlayerHP>().RPC_DamageAnim();
            }

            SetAttackActive(false);
        }
    }

    public override void FixedUpdateNetwork()
    {
        if (isAttackActive)
        {
            timer -= Runner.DeltaTime;
            if (timer <= 0)
            {
                SetAttackActive(false);
            }
        }

        ReturnToOriginalPosition();
    }
}
