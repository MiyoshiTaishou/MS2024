using Fusion;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class BossStatus : NetworkBehaviour
{
    [Networked,SerializeField]
    private int nBossHP { get; set; }

    int InitHP;

    //Slider
    public Slider slider;

    [Tooltip("��_���[�W�G�t�F�N�g")]
    public ParticleSystem Damageparticle;

    public override void Spawned()
    {
        nBossHP = 100;
        InitHP = 100;
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

        slider.value = nBossHP;

        // �p�[�e�B�N���V�X�e���̃C���X�^���X�𐶐�
        ParticleSystem newParticle = Instantiate(Damageparticle);
        //�ł��߂��ꏊ�Ƀp�[�e�B�N���𐶐�
        newParticle.transform.position= new Vector3(this.transform.position.x,this.transform.position.y,this.transform.position.z);
        // �p�[�e�B�N���𔭐�������
        newParticle.Play();
        // �C���X�^���X�������p�[�e�B�N���V�X�e����GameObject��1�b��ɍ폜
        Destroy(newParticle.gameObject, 1.0f);

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
        // �v���C���[�I�u�W�F�N�g���폜����iStateAuthority�݂̂��s���j
        if (Object.HasStateAuthority)
        {
            //Runner.Despawn(Object); // �I�u�W�F�N�g���폜����

            Runner.Shutdown();

            // ���C�����j���[�V�[���ɖ߂� (�V�[���� "MainMenu" �����ۂ̃V�[�����ɒu�������Ă�������)
            UnityEngine.SceneManagement.SceneManager.LoadScene("MenuScene");
        }
    }

    public override void FixedUpdateNetwork()
    {
       
        slider.value = 1 - (float)nBossHP / (float)InitHP;

        // HP��0�ȉ��̏ꍇ�ɍ폜���������s
        if (nBossHP <= 0 && Object.HasStateAuthority)
        {
            HandleBossDeath();
        }
    }
}
