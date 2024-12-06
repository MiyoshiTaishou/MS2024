using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public  enum PARRYTYPE
{
    ALL,
    TANUKI,
    KITUNE,
    DOUBLE,
};

public class BossAttackArea : NetworkBehaviour
{
    GameObject box;
    GameObject parent;

    [SerializeField]
    public float deactivateTime = 0.5f; // �U���G���A�̔�\���ɂ���܂ł̎���

    [Networked]private float timer { get; set; }

    [Networked] private bool isAttack { get; set; }
    private ParticleSystem newParticle;
    [Tooltip("�U���G�t�F�N�g")]
    public ParticleSystem AttackParticle;

    private GameObject Pare;

    // ���̈ʒu��ێ�����
    private Vector3 originalPosition;

    [Networked] public  PARRYTYPE Type { get; set; }

    [Networked] public bool isTanuki { get; set; }
    [Networked] public bool isKitune { get; set; }

    [SerializeField, Header("�`���[�g���A�����[�h")]
    private bool isTutorial = false;

    public override void Spawned()
    {
        box = GameObject.Find("Networkbox");
        parent = transform.parent.gameObject;
        timer = deactivateTime;
        Pare = transform.parent.gameObject;
        isTanuki= false;
        isKitune= false;
        // ���̈ʒu���L�^
        originalPosition = transform.position;
    }

    // SetActive(true)�̂��тɌĂяo��
    public void OnEnable()
    {
        Debug.Log("�U���G�t�F�N�g����");
        isAttack = true;
        isTanuki = false;
        isKitune = false;
    }

    public override void Render()
    {
        if (isAttack)
        {
            // �p�[�e�B�N���V�X�e���̃C���X�^���X�𐶐�
            newParticle = Instantiate(AttackParticle);
            // �U�������Ɋ�Â��Ĉʒu��ݒ�
            if (Pare.transform.localScale.x >= 0)
            {
                newParticle.transform.position = new Vector3(transform.position.x - 4.0f, transform.position.y - 2.0f, transform.position.z);
            }
            else
            {
                newParticle.transform.position = new Vector3(transform.position.x + 4.0f, transform.position.y - 2.0f, transform.position.z);
            }

            // �p�[�e�B�N���𔭐�������
            newParticle.Play();
            // �C���X�^���X�������p�[�e�B�N���V�X�e����GameObject��1�b��ɍ폜
            Destroy(newParticle.gameObject, 1f);
            isAttack = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //��������
        if (other.CompareTag("Player"))
        {
            timer = deactivateTime;
            // �p���B�s�U�����ǂ���
            if (!parent.GetComponent<BossAI>().isParry)
            {
                if (other.GetComponent<PlayerParryNet>().ParryCheck()&&
                    ((other.GetComponent<PlayerParryNet>().isTanuki&&Type==PARRYTYPE.TANUKI)||
                    (!other.GetComponent<PlayerParryNet>().isTanuki&&Type==PARRYTYPE.KITUNE)||
                     Type==PARRYTYPE.ALL))
                {
                    Debug.Log("�p���B����");
                    other.GetComponent<PlayerParryNet>().RPC_ParrySystem();

                    // �m�b�N�o�b�N�\���ǂ���
                    if (parent.GetComponent<BossAI>().isKnockBack)
                    {
                        parent.GetComponent<BossAI>().RPC_AnimName();
                    }

                    ResetToOriginalPosition(); // ���̈ʒu�ɖ߂�
                    gameObject.SetActive(false);
                    return;
                }
                else if(other.GetComponent<PlayerParryNet>().ParryCheck() &&Type == PARRYTYPE.DOUBLE)
                {
                    if(other.GetComponent<PlayerParryNet>().isTanuki) 
                    {
                        isTanuki = true;
                    }
                    else if(other.GetComponent<PlayerParryNet>().isTanuki==false) 
                    {
                        isKitune = true;
                    }
                    if(isTanuki&&isKitune)
                    {
                        Debug.Log("�p���B����");
                        other.GetComponent<PlayerParryNet>().RPC_ParrySystem();

                        // �m�b�N�o�b�N�\���ǂ���
                        if (parent.GetComponent<BossAI>().isKnockBack)
                        {
                            parent.GetComponent<BossAI>().RPC_AnimName();
                        }

                        ResetToOriginalPosition(); // ���̈ʒu�ɖ߂�
                        gameObject.SetActive(false);
                        return;
                    }
                }
            }
            //�����܂�TriggerStay
            Debug.Log("�U���q�b�g");
            if (other.GetComponent<PlayerHP>().inbisibleFrame == 0)
            {
                if(!isTutorial)
                {
                    box.GetComponent<ShareNumbers>().CurrentHP--;
                    box.GetComponent<ShareNumbers>().RPC_Damage();
                }
                other.GetComponent<PlayerHP>().RPC_DamageAnim();
            }
            Render();
            ResetToOriginalPosition(); // ���̈ʒu�ɖ߂�
            gameObject.SetActive(false);
        }
    }

    // ���̈ʒu�ɖ߂����\�b�h
    private void ResetToOriginalPosition()
    {
        transform.position = originalPosition;
    }

    public override void FixedUpdateNetwork()
    {
        // �^�C�}�[�����炵�A��莞�Ԍ�ɔ�\���ɂ���
        if (timer > 0)
        {
            timer -= Runner.DeltaTime;
            if (timer <= 0)
            {
                ResetToOriginalPosition(); // �^�C���A�E�g���ɂ����̈ʒu�ɖ߂�
                gameObject.SetActive(false);
                timer = deactivateTime;
            }
        }
    }
}
