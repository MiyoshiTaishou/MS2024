using Fusion;
using System;
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

    public UnityEngine.UI.Slider Backslider;

    [Networked] private float sliderValue { get; set; }

    [Tooltip("��_���[�W�G�t�F�N�g")]
    public ParticleSystem Damageparticle;

    [Tooltip("���S���G�t�F�N�g")]
    public ParticleSystem Deathparticle;

    [Networked] private bool isDamageEffect { get; set; }

    [Networked] private bool isDeathEffect { get; set; }

    [Header("���U���g�V�[����"), SerializeField]
    private String ResultSceneName;

    private void Start()
    {
        slider.onValueChanged.AddListener(OnSliderValueChanged);
    }

    public override void Spawned()
    {
        slider.value = nBossHP;
        Backslider.value = nBossHP;
        sliderValue = nBossHP; // �����l�Ƃ���nBossHP��sliderValue�ɐݒ�

        InitHP = nBossHP;
        sliderValue = nBossHP; // �X���C�_�[�l���X�V
    }

    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
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

    private void HandleBossDeath()
    {
        isDeathEffect = true;

        if (Object.HasStateAuthority)
        {
            // �N���C�A���g�ɐ�ɃV�[���J�ڂ��w��
            RPC_ClientSceneTransition();
        }
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    private void RPC_ClientSceneTransition()
    {
        // �N���C�A���g�͐�ɃV�[���J�ڂ����s
        if (!Object.HasStateAuthority)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(ResultSceneName);
        }
        else
        {
            // �z�X�g���̓N���C�A���g�̑J�ڂ�����������ɃV�[���J��
            StartCoroutine(HostSceneTransition());
        }
    }

    private IEnumerator HostSceneTransition()
    {
        yield return new WaitForSeconds(2); // �N���C�A���g�����V�[���J�ڂ���܂ł̎��Ԃ𒲐�
        Runner.Shutdown();
        UnityEngine.SceneManagement.SceneManager.LoadScene(ResultSceneName);
    }

    public override void Render()
    {
        if (isDamageEffect == true)
        {
            ParticleSystem DameParticle = Instantiate(Damageparticle);
            DameParticle.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z + 1);
            DameParticle.Play();
            Destroy(DameParticle.gameObject, 1.0f);
            isDamageEffect = false;
        }

        if (isDeathEffect == true)
        {
            ParticleSystem newParticle = Instantiate(Deathparticle);
            newParticle.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z);
            newParticle.Play();
            Destroy(newParticle.gameObject, 1.0f);
            isDeathEffect = false;
        }

        slider.value = nBossHP;

        if(Backslider.value>nBossHP)
        {
            Backslider.value -= 0.5f;
        }

    }

    private void OnSliderValueChanged(float value)
    {
        if (Object.HasInputAuthority)
        {
            sliderValue = value;
        }
    }

    public override void FixedUpdateNetwork()
    {
        if (!Object.HasInputAuthority)
        {
            slider.value = sliderValue; // sliderValue���N���C�A���g�ɔ��f
        }


        if (nBossHP <= 0 && Object.HasStateAuthority)
        {
            HandleBossDeath();
        }
    }
}
