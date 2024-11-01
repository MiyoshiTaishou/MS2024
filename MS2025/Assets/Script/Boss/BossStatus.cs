using Fusion;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class BossStatus : NetworkBehaviour
{
    [Networked, SerializeField]
    public int nBossHP { get; set; }

    public int InitHP;

    //Slider
    public UnityEngine.UI.Slider slider;
    [Networked] private float sliderValue { get; set; }

    [Tooltip("��_���[�W�G�t�F�N�g")]

    public ParticleSystem Damageparticle;

    [Tooltip("���S���G�t�F�N�g")]

    public ParticleSystem Deathparticle;

    [Networked] private bool isDamageEffect { get; set; }

    [Networked] private bool isDeathEffect { get; set; }

    private void Start()
    {
        slider.onValueChanged.AddListener(OnSliderValueChanged);
    }

    public override void Spawned()
    {

        slider.value = nBossHP;
        sliderValue = nBossHP; // �����l�Ƃ���nBossHP��sliderValue�ɐݒ�

        InitHP = nBossHP;

        sliderValue = nBossHP; // �X���C�_�[�l���X�V

    }



    /// <summary>
    /// �����̃\�[�X����������ݒ肵�Ȃ��Ɠ��삵�Ȃ�
    /// ���̃I�u�W�F�N�g���ǂ̌����������Ă��邩�l���Ă�邱��
    /// </summary>
    /// <param name="_damage"></param>
    [Rpc(RpcSources.All,RpcTargets.StateAuthority)]
    public void RPC_Damage(int _damage)
    {
        nBossHP -= _damage;

        isDamageEffect = true;

        // HP��0�ȉ��Ȃ�폜�������Ă�
        if (nBossHP <= 0)
        {
            HandleBossDeath();
        }
    }

    /// <summary>
    /// �v���C���[�����S�����ۂ̏���
    /// </summary>
    private void HandleBossDeath()
    {

        isDeathEffect = true;

        // �v���C���[�I�u�W�F�N�g���폜����iStateAuthority�݂̂��s���j
        if (Object.HasStateAuthority)
        {
            //Runner.Despawn(Object); // �I�u�W�F�N�g���폜����

            Runner.Shutdown();

            // ���C�����j���[�V�[���ɖ߂� (�V�[���� "MainMenu" �����ۂ̃V�[�����ɒu�������Ă�������)
            UnityEngine.SceneManagement.SceneManager.LoadScene("MenuScene");
        }
    }

    public override void Render()
    {
        //��e�G�t�F�N�g�Đ�
        if(isDamageEffect==true)
        {
            // �p�[�e�B�N���V�X�e���̃C���X�^���X�𐶐�
            ParticleSystem DameParticle = Instantiate(Damageparticle);
            //�ł��߂��ꏊ�Ƀp�[�e�B�N���𐶐�
            DameParticle.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z+1);
            // �p�[�e�B�N���𔭐�������
            DameParticle.Play();
            // �C���X�^���X�������p�[�e�B�N���V�X�e����GameObject��1�b��ɍ폜
            Destroy(DameParticle.gameObject, 1.0f);

            isDamageEffect = false;
        }

        //���S���G�t�F�N�g�Đ�
        if(isDeathEffect==true)
        {
            // �p�[�e�B�N���V�X�e���̃C���X�^���X�𐶐�
            ParticleSystem newParticle = Instantiate(Deathparticle);
            //�ł��߂��ꏊ�Ƀp�[�e�B�N���𐶐�
            newParticle.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z);
            // �p�[�e�B�N���𔭐�������
            newParticle.Play();
            // �C���X�^���X�������p�[�e�B�N���V�X�e����GameObject��1�b��ɍ폜
            Destroy(newParticle.gameObject, 1.0f);

            isDeathEffect = false;
        }

        

        // �X���C�_�[�̒l������������
        slider.value = sliderValue;

    }

        private void OnSliderValueChanged(float value)
    {
        if (Object.HasInputAuthority)
        {
            // ���͌��������v���C���[���X���C�_�[��ύX�����ꍇ�ANetworked Property�ɒl�𔽉f
            sliderValue = value;
        }
    }

    public override void FixedUpdateNetwork()
    {

        // Networked Property���ύX���ꂽ�ꍇ�AUI�ɔ��f������
        if (!Object.HasInputAuthority)
        {
            slider.value = sliderValue; // sliderValue���N���C�A���g�ɔ��f
        }

       
        slider.value = 1 - (float)nBossHP / (float)InitHP;


        // HP��0�ȉ��̏ꍇ�ɍ폜���������s
        if (nBossHP <= 0 && Object.HasStateAuthority)
        {
            HandleBossDeath();
        }
    }
}
