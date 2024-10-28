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

    [Tooltip("被ダメージエフェクト")]
    public ParticleSystem Damageparticle;

    public override void Spawned()
    {
        nBossHP = 100;
        InitHP = 100;
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

        slider.value = nBossHP;

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

    public override void FixedUpdateNetwork()
    {
       
        slider.value = 1 - (float)nBossHP / (float)InitHP;

        // HPが0以下の場合に削除処理を実行
        if (nBossHP <= 0 && Object.HasStateAuthority)
        {
            HandleBossDeath();
        }
    }
}
