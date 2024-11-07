using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttackArea : NetworkBehaviour
{
    GameObject box;
    GameObject parent;
    private float deactivateTime = 0.5f; // �U���G���A�̔�\���ɂ���܂ł̎���
    private float timer;
    private Vector3 PlayerPos;
    private bool isAttack=false;
    [Tooltip("�U���G�t�F�N�g")]
    public ParticleSystem AttackParticle;

    public override void Spawned()
    {
        box = GameObject.Find("Networkbox");
        parent = transform.parent.gameObject;
        timer = deactivateTime;
       
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (other.GetComponent<PlayerParryNet>().ParryCheck())
            {
                Debug.Log("�p���B����");
                other.GetComponent<PlayerParryNet>().RPC_ParrySystem();
                parent.GetComponent<BossAI>().RPC_AnimName();
                gameObject.SetActive(false);

                return;
            }

            Debug.Log("�U���q�b�g");
            box.GetComponent<ShareNumbers>().RPC_Damage();
            other.GetComponent<PlayerHP>().RPC_DamageAnim();
            isAttack = true;
            PlayerPos = other.transform.position;
            gameObject.SetActive(false);
        }
    }

    public override void Render()
    {
     

        if(isAttack)
        {
            // �p�[�e�B�N���V�X�e���̃C���X�^���X�𐶐�
            ParticleSystem newParticle = Instantiate(AttackParticle);
            //�p�[�e�B�N���𐶐�
            newParticle.transform.position = (this.transform.position + PlayerPos)/2;
            // �p�[�e�B�N���𔭐�������
            newParticle.Play();
            // �C���X�^���X�������p�[�e�B�N���V�X�e����GameObject��1�b��ɍ폜
            Destroy(newParticle.gameObject, 0.4f);
            isAttack = false;
        }
    }

    public override void FixedUpdateNetwork()
    {
        // �^�C�}�[�����炵�A��莞�Ԍ�ɔ�\���ɂ���
        if (timer > 0)
        {
            timer -= Runner.DeltaTime;
            if (timer <= 0)
            {
                gameObject.SetActive(false);
                timer = deactivateTime;
            }
        }
    }
}
