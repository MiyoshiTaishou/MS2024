using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossStatus : NetworkBehaviour
{
    [Networked,SerializeField]
    private int nBossHP { get; set; }

    //Slider
    public Slider slider;

    public override void Spawned()
    {
        nBossHP = 100;

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
        slider.value = nBossHP;

        // HPが0以下の場合に削除処理を実行
        if (nBossHP <= 0 && Object.HasStateAuthority)
        {
            HandleBossDeath();
        }
    }
}
