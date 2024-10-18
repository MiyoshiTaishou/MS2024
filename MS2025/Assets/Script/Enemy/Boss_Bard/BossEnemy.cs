using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossEnemy : MonoBehaviour
{
    [Header("HP(float)��HP�o�[�ݒ�")]
    //�ő�HP�ƌ��݂�HP�B
    public float maxHp = 10;
    float Hp;
    //Slider
    public Slider slider;

    [Space(15)]

    [Header("�e�G�t�F�N�g")]
    //Efect
  
    [Tooltip("��_���[�W�G�t�F�N�g")]
   public ParticleSystem Damageparticle;

    [Tooltip("���j�G�t�F�N�g")]
   public ParticleSystem Destroyparticle;

    [Space(15)]

    [Header("�e�T�E���h�G�t�F�N�g")]
    //SoundEfect
    //�Ȃ炷�T�E���h�G�t�F�N�g������ϐ�
    [Tooltip("��_���[�WSE")]
    public AudioClip Damagesound;
    AudioSource audioSource;

    private Animator m_Animator;
    private Vector3 location;

    void Start()
    {
        //Slider���ő�ɂ���B
        slider.value = 10;
        //HP���ő�HP�Ɠ����l�ɁB
        Hp = maxHp;
        //�R���|�[�l���g�擾
        audioSource = GetComponent<AudioSource>();

        m_Animator = GetComponent<Animator>();

    }

    void Update()
    {
        if (Input.GetKeyDown("down"))
        {


            ////Sound1��炷
            //audioSource.PlayOneShot(sound1);

            //m_Animator.SetTrigger("Hit");

            //�F��Ԃ�����
            //gameObject.GetComponent<SpriteRenderer>().color = new Color32(255, 0, 0, 255);
            //0.5�b���void back�����s
            //Invoke("back", 0.2f);

        }

        else if (Input.GetKeyDown("up"))
        {
            m_Animator.SetTrigger("Attack");
        }

        //���s�A�j���[�V����(��X�����Ƃ����₵�ē����L�[���͂ǂ����邩�Ƃ����߂����������Ǝv���܂�)
        if (Input.GetKey("w"))
        {
            m_Animator.SetBool("walkForward", true);

        }
        else
        {
            m_Animator.SetBool("walkForward", false);
        }

        if (Input.GetKey("a"))
        {
            m_Animator.SetBool("Left", true);

        }
        else
        {
            m_Animator.SetBool("Left", false);
        }

        if (Input.GetKey("d"))
        {
            m_Animator.SetBool("Right", true);

        }
        else
        {
            m_Animator.SetBool("Right", false);
        }

        if (Input.GetKey("s"))
        {
            m_Animator.SetBool("Back", true);

        }
        else
        {
            m_Animator.SetBool("Back", false);
        }

        if(Hp==0)
        {
            // �p�[�e�B�N���V�X�e���̃C���X�^���X�𐶐�
            ParticleSystem newParticle = Instantiate(Destroyparticle);
               newParticle.transform.position = new Vector3(this.transform.position.x,this.transform.position.y,this.transform.position.z);
            Hp = -1;
        }


    }
    void back()
    {
        //�F�����ɖ߂�
        gameObject.GetComponent<SpriteRenderer>().color = new Color32(255, 255, 255, 255);
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.name == "PlayerAttackArea")
        {
            //HP����1������
            Hp = Hp - 1;

            //HP��Slider�ɔ��f ��HP��value�̍ő�l����v���ĂȂ��Ƃ��܂�����Ȃ��̂Œ���
            slider.value = Hp;

            // �p�[�e�B�N���V�X�e���̃C���X�^���X�𐶐�
            ParticleSystem newParticle = Instantiate(Damageparticle);
            // ���������I�u�W�F�N�g�ƍł��߂��ʒu���擾����
            Vector3 closestPoint = collider.ClosestPoint(location);
            //�ł��߂��ꏊ�Ƀp�[�e�B�N���𐶐�
            newParticle.transform.position = closestPoint;
            // �p�[�e�B�N���𔭐�������
            newParticle.Play();
            // �C���X�^���X�������p�[�e�B�N���V�X�e����GameObject��1�b��ɍ폜
            Destroy(newParticle.gameObject, 1.0f);

            //Sound1��炷
            audioSource.PlayOneShot(Damagesound);

            m_Animator.SetTrigger("Hit");
        }

        Debug.Log(collider.gameObject.name);
    }

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.gameObject.name);

    }
}