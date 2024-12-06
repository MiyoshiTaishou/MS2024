using Fusion;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
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

    [SerializeField]private Image Fill;

    [SerializeField] private Color HPBar2= new Color32(25, 176, 0, 255);
    [SerializeField] private Color HPBar3= new Color32(255, 221, 0, 255);

    [Tooltip("被ダメージエフェクト")]
   [SerializeField] private ParticleSystem Damageparticle;

    [Tooltip("死亡時エフェクト")]
    [SerializeField] private ParticleSystem Deathparticle;

<<<<<<< HEAD
    //�̗͂�0�ɂȂ����񐔂𐔂���
    [SerializeField,Networked] private int DeathCount{ get; set; };
=======
    //体力が0になった回数を数える
    [SerializeField] private int DeathCount = 0;
>>>>>>> main

    [SerializeField,Header("ゲームマネージャー")]
    private GameManager gameManager;

    [Networked] private bool isDamageEffect { get; set; }

    [Networked] private bool isDeathEffect { get; set; }


    [Header("リザルトシーン名"), SerializeField]
    private String ResultSceneName;

    //HPの減少が止まったら赤ゲージを減らすためのカウント
    [Networked] private int HPCount  { get; set; }

    private NetworkRunner networkRunner;

    [SerializeField]
    private TransitionManager transitionManager;

    // シーン遷移が一度だけ実行されるようにするためのフラグ
    private bool hasTransitioned = false;

    public override void Spawned()
    {
        networkRunner = FindObjectOfType<NetworkRunner>();
        slider.maxValue = nBossHP;
        slider.value = nBossHP;
        Backslider.maxValue = nBossHP;
        Backslider.value = nBossHP;
        InitHP = nBossHP;
        DeathCount = 0;
    }

    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void RPC_Damage(int _damage)
    {
        if (SceneManager.GetActiveScene().name != "Game")
        {
            nBossHP -= _damage;
            HPCount = 0;
        }

        isDamageEffect = true;

        //// HPが0以下なら削除処理を呼ぶ
        //if (nBossHP <= 0)
        //{
        //    HandleBossDeath();
        //}
    }

    private void HandleBossDeath()
    {
        // シーン遷移が一度だけ行われるようにチェック
        if (hasTransitioned) return;

        transitionManager.TransitionStart();
        isDeathEffect = true;
        hasTransitioned = true; // シーン遷移フラグを設定
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
        // クライアントは先にシーン遷移を実行
        if (!Object.HasStateAuthority)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(ResultSceneName);
        }
        else
        {
            // ホスト側はクライアントの遷移が完了した後にシーン遷移
            StartCoroutine(HostSceneTransition());
        }
    }

    private IEnumerator HostSceneTransition()
    {
        yield return new WaitForSeconds(2); // クライアント側がシーン遷移するまでの時間を調整
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

<<<<<<< HEAD
=======
        if (nBossHP <= 0 && Object.HasStateAuthority)
        {

            switch (DeathCount)
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
                    DeathCount++;
                    break;
            }

        }

>>>>>>> main
        if (DeathCount == 1)
        {
            Fill.color = HPBar2;
            Destroy(GameObject.Find("BossHPBarP"));
        }
        else if (DeathCount == 2)
        {
            Fill.color = HPBar3;
            Destroy(GameObject.Find("BossHPBarG"));
        }
        else if(DeathCount==3)
        {
            Destroy(GameObject.Find("BossHPBarY"));
        }

    }

    public override void FixedUpdateNetwork()
    {
        if (nBossHP <= 0 && Object.HasStateAuthority)
        {

            switch (DeathCount)
            {
<<<<<<< HEAD
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
                    DeathCount++;
                    break;
=======
>>>>>>> main
                case 3:
                    Debug.Log("ボス死亡です");
                    RPC_HandleBossDeath();
                    // クライアントに先にシーン遷移を指示
                    gameManager.RPC_EndBattle(10, 5);
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
