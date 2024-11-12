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

    [Tooltip("被ダメージエフェクト")]
    public ParticleSystem Damageparticle;

    [Tooltip("死亡時エフェクト")]
    public ParticleSystem Deathparticle;

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
    }

    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void RPC_Damage(int _damage)
    {
        nBossHP -= _damage;
        HPCount = 0;
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

        isDeathEffect = true;

        transitionManager.TransitionStart();
        hasTransitioned = true; // シーン遷移フラグを設定       

        StartCoroutine(Load());

        //if (Object.HasStateAuthority)
        //{           
        //    RPC_ClientSceneTransition();
        //}
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

       

    }

    public override void FixedUpdateNetwork()
    {
        if (nBossHP <= 0 && !hasTransitioned)
        {
            // クライアントに先にシーン遷移を指示
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
