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

    [Tooltip("被ダメージエフェクト")]
    public ParticleSystem Damageparticle;

    [Tooltip("死亡時エフェクト")]
    public ParticleSystem Deathparticle;

    [Networked] private bool isDamageEffect { get; set; }

    [Networked] private bool isDeathEffect { get; set; }

    [Header("リザルトシーン名"), SerializeField]
    private String ResultSceneName;

    private void Start()
    {
        slider.onValueChanged.AddListener(OnSliderValueChanged);
    }

    public override void Spawned()
    {
        slider.value = nBossHP;
        Backslider.value = nBossHP;
        sliderValue = nBossHP; // 初期値としてnBossHPをsliderValueに設定

        InitHP = nBossHP;
        sliderValue = nBossHP; // スライダー値を更新
    }

    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void RPC_Damage(int _damage)
    {
        nBossHP -= _damage;
        isDamageEffect = true;

        // HPが0以下なら削除処理を呼ぶ
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
            // クライアントに先にシーン遷移を指示
            RPC_ClientSceneTransition();
        }
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
            slider.value = sliderValue; // sliderValueをクライアントに反映
        }


        if (nBossHP <= 0 && Object.HasStateAuthority)
        {
            HandleBossDeath();
        }
    }
}
