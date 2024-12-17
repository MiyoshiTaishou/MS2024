using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttackArea2Boss : NetworkBehaviour
{
    GameObject box;
    GameObject parent;  

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
        parent = GameObject.Find("Boss2D");       
        Pare = GameObject.Find("Boss2D");
        isTanuki = false;
        isKitune = false;        
        originalPosition = transform.position;
    }  

    public override void Render()
    {
       
            //// �p�[�e�B�N���V�X�e���̃C���X�^���X�𐶐�
            //newParticle = Instantiate(AttackParticle);
            //if (Pare.transform.localScale.x >= 0)
            //{
            //    newParticle.transform.position = new Vector3(transform.position.x - 4.0f, transform.position.y - 2.0f, transform.position.z);
            //}
            //else
            //{
            //    newParticle.transform.position = new Vector3(transform.position.x + 4.0f, transform.position.y - 2.0f, transform.position.z);
            //}

            //newParticle.Play();
            //Destroy(newParticle.gameObject, 1f);
        
    }
    
    private void OnTriggerEnter(Collider other)
    {        
        if (other.CompareTag("Player"))
        {
            // �p���B����
            if (!parent.GetComponent<BossAI>().isParry)
            {
                // �p���B�s�U�����ǂ���
                if (!parent.GetComponent<BossAI>().isParry)
                {
                    //�p���B�����������ǂ���
                    if (other.GetComponent<PlayerParryNet>().ParryCheck())
                    {
                        //�K�ōU�����K�Ȃ�p���B����
                        if (other.GetComponent<PlayerParryNet>().isTanuki && Type == PARRYTYPE.TANUKI)
                        {
                            Debug.Log("�p���B����");
                            other.GetComponent<PlayerParryNet>().RPC_ParrySystem();

                            // �m�b�N�o�b�N�\���ǂ���
                            if (parent.GetComponent<BossAI>().isKnockBack)
                            {
                                parent.GetComponent<BossAI>().RPC_AnimName();
                            }
                           
                            //gameObject.SetActive(false);
                            return;
                        }

                        //�ςōU�����ςȂ�p���B����
                        if (!other.GetComponent<PlayerParryNet>().isTanuki && Type == PARRYTYPE.KITUNE)
                        {
                            Debug.Log("�p���B����");
                            other.GetComponent<PlayerParryNet>().RPC_ParrySystem();

                            // �m�b�N�o�b�N�\���ǂ���
                            if (parent.GetComponent<BossAI>().isKnockBack)
                            {
                                parent.GetComponent<BossAI>().RPC_AnimName();
                            }
                           
                            //gameObject.SetActive(false);
                            return;
                        }

                        //�ǂ�ł��p���B�\�U��
                        if (Type == PARRYTYPE.ALL)
                        {
                            Debug.Log("�p���B����");
                            other.GetComponent<PlayerParryNet>().RPC_ParrySystem();

                            // �m�b�N�o�b�N�\���ǂ���
                            if (parent.GetComponent<BossAI>().isKnockBack)
                            {
                                parent.GetComponent<BossAI>().RPC_AnimName();
                            }
                          
                            //gameObject.SetActive(false);
                            return;
                        }

                        //�_�u���p���B
                        if (Type == PARRYTYPE.DOUBLE)
                        {
                            if (other.GetComponent<PlayerParryNet>().isTanuki)
                            {
                                isTanuki = true;
                            }
                            else if (other.GetComponent<PlayerParryNet>().isTanuki == false)
                            {
                                isKitune = true;
                            }
                            if (isTanuki && isKitune)
                            {
                                Debug.Log("�p���B����");
                                other.GetComponent<PlayerParryNet>().RPC_ParrySystem();

                                // �m�b�N�o�b�N�\���ǂ���
                                if (parent.GetComponent<BossAI>().isKnockBack)
                                {
                                    parent.GetComponent<BossAI>().RPC_AnimName();
                                }
                                
                                //gameObject.SetActive(false);
                                return;
                            }
                        }
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
        }
    }   
}
