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
    public bool isAttack=false;
    private ParticleSystem newParticle;
    [Tooltip("�U���G�t�F�N�g")]
    public ParticleSystem AttackParticle;

    private GameObject Pare;

    public override void Spawned()
    {
        
        box = GameObject.Find("Networkbox");
        parent = transform.parent.gameObject;
        timer = deactivateTime;
        Pare = transform.parent.gameObject;
    }

    //SetActive(true)�̂��тɌĂяo��
    public void OnEnable()
    {
        Debug.Log("�U���G�t�F�N�g����");
        isAttack = true;
    }

    public override void Render()
    {

        if (isAttack)
        {
            if (Pare.transform.localScale.x >= 0)
            {
                // �p�[�e�B�N���V�X�e���̃C���X�^���X�𐶐�
                newParticle = Instantiate(AttackParticle);
                //�p�[�e�B�N���𐶐�
                newParticle.transform.position = new Vector3(this.transform.position.x - 4.0f, this.transform.position.y - 2.0f, this.transform.position.z);
                // �p�[�e�B�N���𔭐�������
                newParticle.Play();
                // �C���X�^���X�������p�[�e�B�N���V�X�e����GameObject��1�b��ɍ폜
                Destroy(newParticle.gameObject, 1f);
                isAttack = false;
            }
            else
            {
                // �p�[�e�B�N���V�X�e���̃C���X�^���X�𐶐�
                newParticle = Instantiate(AttackParticle);
                //�p�[�e�B�N���𐶐�
                newParticle.transform.position = new Vector3(this.transform.position.x + 4.0f, this.transform.position.y - 2.0f, this.transform.position.z);
                // �p�[�e�B�N���𔭐�������
                newParticle.Play();
                // �C���X�^���X�������p�[�e�B�N���V�X�e����GameObject��1�b��ɍ폜
                Destroy(newParticle.gameObject, 1f);
                isAttack = false;
            }

        }
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
                Render();
                return;
            }

            Debug.Log("�U���q�b�g");
            box.GetComponent<ShareNumbers>().RPC_Damage();
            other.GetComponent<PlayerHP>().RPC_DamageAnim();
            Render();
            gameObject.SetActive(false);
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
