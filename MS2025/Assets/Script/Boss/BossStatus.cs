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
    private int nBossHP { get; set; }

    //Slider
    public UnityEngine.UI.Slider slider;
    [Networked] private float sliderValue { get; set; }

    [Tooltip("被ダメージエフェクト")]

    public ParticleSystem Damageparticle;

    private void Start()
    {
        slider.onValueChanged.AddListener(OnSliderValueChanged);
    }

    public override void Spawned()
    {
        nBossHP = 100;
        slider.value = nBossHP;
        sliderValue = nBossHP; // 初期値としてnBossHPをsliderValueに設定
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

        sliderValue = nBossHP; // スライダー値を更新

        // スライダーの値も同期させる
        slider.value = sliderValue;

        // パーティクルシステムのインスタンスを生成
        ParticleSystem newParticle = Instantiate(Damageparticle);
        //最も近い場所にパーティクルを生成
        newParticle.transform.position= new Vector3(this.transform.position.x,this.transform.position.y,this.transform.position.z);
        // パーティクルを発生させる
        newParticle.Play();
        // インスタンス化したパーティクルシステムのGameObjectを1秒後に削除
        Destroy(newParticle.gameObject, 1.0f);

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
        // プレイヤーオブジェクトを削除する（StateAuthorityのみが行う）
        if (Object.HasStateAuthority)
        {
            //Runner.Despawn(Object); // オブジェクトを削除する

            Runner.Shutdown();

            // メインメニューシーンに戻る (シーン名 "MainMenu" を実際のシーン名に置き換えてください)
            UnityEngine.SceneManagement.SceneManager.LoadScene("MenuScene");
        }
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

        // HPが0以下の場合に削除処理を実行
        if (nBossHP <= 0 && Object.HasStateAuthority)
        {
            HandleBossDeath();
        }
    }
}
