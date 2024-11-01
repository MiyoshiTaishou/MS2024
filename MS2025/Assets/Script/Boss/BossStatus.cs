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

    [Tooltip("被ダメージエフェクト")]

    public ParticleSystem Damageparticle;

    [Tooltip("死亡時エフェクト")]

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
        sliderValue = nBossHP; // 初期値としてnBossHPをsliderValueに設定

        InitHP = nBossHP;

        sliderValue = nBossHP; // スライダー値を更新

    }



    /// <summary>
    /// ここのソースをしっかり設定しないと動作しない
    /// このオブジェクトがどの権限を持っているか考えてやること
    /// </summary>
    /// <param name="_damage"></param>
    [Rpc(RpcSources.All,RpcTargets.StateAuthority)]
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

    /// <summary>
    /// プレイヤーが死亡した際の処理
    /// </summary>
    private void HandleBossDeath()
    {

        isDeathEffect = true;

        // プレイヤーオブジェクトを削除する（StateAuthorityのみが行う）
        if (Object.HasStateAuthority)
        {
            //Runner.Despawn(Object); // オブジェクトを削除する

            Runner.Shutdown();

            // メインメニューシーンに戻る (シーン名 "MainMenu" を実際のシーン名に置き換えてください)
            UnityEngine.SceneManagement.SceneManager.LoadScene("MenuScene");
        }
    }

    public override void Render()
    {
        //被弾エフェクト再生
        if(isDamageEffect==true)
        {
            // パーティクルシステムのインスタンスを生成
            ParticleSystem DameParticle = Instantiate(Damageparticle);
            //最も近い場所にパーティクルを生成
            DameParticle.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z+1);
            // パーティクルを発生させる
            DameParticle.Play();
            // インスタンス化したパーティクルシステムのGameObjectを1秒後に削除
            Destroy(DameParticle.gameObject, 1.0f);

            isDamageEffect = false;
        }

        //死亡時エフェクト再生
        if(isDeathEffect==true)
        {
            // パーティクルシステムのインスタンスを生成
            ParticleSystem newParticle = Instantiate(Deathparticle);
            //最も近い場所にパーティクルを生成
            newParticle.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z);
            // パーティクルを発生させる
            newParticle.Play();
            // インスタンス化したパーティクルシステムのGameObjectを1秒後に削除
            Destroy(newParticle.gameObject, 1.0f);

            isDeathEffect = false;
        }

        

        // スライダーの値も同期させる
        slider.value = sliderValue;

    }

        private void OnSliderValueChanged(float value)
    {
        if (Object.HasInputAuthority)
        {
            // 入力権限を持つプレイヤーがスライダーを変更した場合、Networked Propertyに値を反映
            sliderValue = value;
        }
    }

    public override void FixedUpdateNetwork()
    {

        // Networked Propertyが変更された場合、UIに反映させる
        if (!Object.HasInputAuthority)
        {
            slider.value = sliderValue; // sliderValueをクライアントに反映
        }

       
        slider.value = 1 - (float)nBossHP / (float)InitHP;


        // HPが0以下の場合に削除処理を実行
        if (nBossHP <= 0 && Object.HasStateAuthority)
        {
            HandleBossDeath();
        }
    }
}
