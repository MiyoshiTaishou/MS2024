using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossEnemy : MonoBehaviour
{
    //�ő�HP�ƌ��݂�HP�B
    public float maxHp=10;
    float Hp;
    //Slider
    public Slider slider;

    //Efect
    [SerializeField]
    [Tooltip("����������G�t�F�N�g(�p�[�e�B�N��)")]
    private ParticleSystem particle;

    //SoundEfect
    //�Ȃ炷�T�E���h�G�t�F�N�g������ϐ�
    public AudioClip sound1;
    AudioSource audioSource;

    void Start()
    {
        //Slider���ő�ɂ���B
        slider.value = 10;
        //HP���ő�HP�Ɠ����l�ɁB
        Hp = maxHp;
        //�R���|�[�l���g�擾
        audioSource = GetComponent<AudioSource>();

    }

   void Update()
    {
        if (Input.GetKeyDown("down"))
        {
            //HP����1������
            Hp = Hp - 1;

            //HP��Slider�ɔ��f ��HP��value�̍ő�l����v���ĂȂ��Ƃ��܂�����Ȃ��̂Œ���
            slider.value = Hp;

            // �p�[�e�B�N���V�X�e���̃C���X�^���X�𐶐�
            ParticleSystem newParticle = Instantiate(particle);
            // �p�[�e�B�N���̔����ꏊ�����̃X�N���v�g���A�^�b�`���Ă���GameObject�̏ꏊ�ɂ���
            newParticle.transform.position = this.transform.position;
            // �p�[�e�B�N���𔭐�������
            newParticle.Play();
            // �C���X�^���X�������p�[�e�B�N���V�X�e����GameObject��1�b��ɍ폜
            Destroy(newParticle.gameObject, 1.0f);

            //Sound1��炷
            audioSource.PlayOneShot(sound1);

            //�F��Ԃ�����
            gameObject.GetComponent<SpriteRenderer>().color = new Color32(255, 0, 0, 255);
            //0.5�b���void back�����s
            Invoke("back", 0.2f);

        }
    }

    void back()
    {
        //�F�����ɖ߂�
        gameObject.GetComponent<SpriteRenderer>().color = new Color32(255, 255, 255, 255);
    }
}