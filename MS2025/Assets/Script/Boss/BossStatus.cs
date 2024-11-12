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

    [Tooltip("��_���[�W�G�t�F�N�g")]
    public ParticleSystem Damageparticle;

    [Tooltip("���S���G�t�F�N�g")]
    public ParticleSystem Deathparticle;

    [SerializeField,Header("�Q�[���}�l�[�W���[")]
    private GameManager gameManager;

    [Networked] private bool isDamageEffect { get; set; }

    [Networked] private bool isDeathEffect { get; set; }


    [Header("���U���g�V�[����"), SerializeField]
    private String ResultSceneName;

    //HP�̌������~�܂�����ԃQ�[�W�����炷���߂̃J�E���g
    [Networked] private int HPCount  { get; set; }

    private NetworkRunner networkRunner;

    [SerializeField]
    private TransitionManager transitionManager;

    // �V�[���J�ڂ���x�������s�����悤�ɂ��邽�߂̃t���O
    private bool hasTransitioned = false;

    public override void Spawned()
    {
        networkRunner = FindObjectOfType<NetworkRunner>();
        slider.maxValue = nBossHP;
        slider.value = nBossHP;
        Backslider.maxValue = nBossHP;
        Backslider.value = nBossHP;
        InitHP = nBossHP;
    }

    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void RPC_Damage(int _damage)
    {
        nBossHP -= _damage;
        HPCount = 0;
        isDamageEffect = true;

        //// HP��0�ȉ��Ȃ�폜�������Ă�
        //if (nBossHP <= 0)
        //{
        //    HandleBossDeath();
        //}
    }

    private void HandleBossDeath()
    {
        // �V�[���J�ڂ���x�����s����悤�Ƀ`�F�b�N
        if (hasTransitioned) return;

        isDeathEffect = true;

        transitionManager.TransitionStart();
        hasTransitioned = true; // �V�[���J�ڃt���O��ݒ�       

        StartCoroutine(Load());

        //if (Object.HasStateAuthority)
        //{           
        //    RPC_ClientSceneTransition();
        //}
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
            Destroy(DameParticle.gameObject, 0.5f);
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

        HPCount++;

        if (Backslider.value > nBossHP && HPCount > 50)
        {
            Backslider.value -= 1f;
        }
        else if (Backslider.value == nBossHP)
        {
            HPCount = 0;
        }

       

    }

    public override void FixedUpdateNetwork()
    {
        if (nBossHP <= 0 && !hasTransitioned)
        {
            // �N���C�A���g�ɐ�ɃV�[���J�ڂ��w��
            gameManager.RPC_EndBattle(10, 5);
        }

        if (nBossHP <= 0 && Object.HasStateAuthority)
        {
            HandleBossDeath();
        }
    }

    private IEnumerator Load()
    {
        yield return new WaitForSeconds(2f);
        networkRunner.LoadScene(ResultSceneName);
    }
}
