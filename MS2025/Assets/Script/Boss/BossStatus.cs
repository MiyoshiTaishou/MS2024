using Fusion;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;

public class BossStatus : NetworkBehaviour
{
    [Networked, SerializeField]
    public int nBossHP { get; set; }

    public int InitHP;



    //Slider
    [SerializeField] private UnityEngine.UI.Slider slider;

    [SerializeField] private UnityEngine.UI.Slider Backslider;

    [SerializeField]private Image Sliderimage;

    [SerializeField] private Color HPBar2= new Color32(25, 176, 0, 255);
    [SerializeField] private Color HPBar3= new Color32(255, 221, 0, 255);

    [Tooltip("��_���[�W�G�t�F�N�g")]
   [SerializeField] private ParticleSystem Damageparticle;

    [Tooltip("���S���G�t�F�N�g")]
    [SerializeField] private ParticleSystem Deathparticle;

    //�̗͂�0�ɂȂ����񐔂𐔂���
    private int DeathCount = 0;

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

        transitionManager.TransitionStart();
        isDeathEffect = true;        
        hasTransitioned = true; // �V�[���J�ڃt���O��ݒ�       

        StartCoroutine(Load());

        //if (Object.HasStateAuthority)
        //{           
        //    RPC_ClientSceneTransition();
        //}
    }

    [Rpc(RpcSources.All, RpcTargets.All)]
    private void RPC_HandleBossDeath()
    {
        HandleBossDeath();
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

        if (DeathCount == 1)
        {
            Sliderimage.color = HPBar2;
        }
        else if (DeathCount == 2)
        {
            Sliderimage.color = HPBar3;
        }

    }

    public override void FixedUpdateNetwork()
    {      
        if (nBossHP <= 0 && Object.HasStateAuthority)
        {

            switch(DeathCount)
            {
                case 0:
                    nBossHP = InitHP;
                    slider.value = nBossHP;
                    Backslider.value = nBossHP;
                    DeathCount += 1;
                break;

                case 1:
                    nBossHP = InitHP;
                    slider.value = nBossHP;
                    Backslider.value = nBossHP;
                    DeathCount += 1;
                break;

                case 2:
                    Debug.Log("�{�X���S�ł�");
                    RPC_HandleBossDeath();                    
                    // �N���C�A���g�ɐ�ɃV�[���J�ڂ��w��
                    gameManager.RPC_EndBattle(10, 5);
                    DeathCount++;
                    break;
            }
     
        }
    }

    private IEnumerator Load()
    {
        yield return new WaitForSeconds(2f);
        networkRunner.LoadScene(ResultSceneName);
    }
}
