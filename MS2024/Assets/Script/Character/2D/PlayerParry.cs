using Fusion;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerParry : IState
{
    private NetworkRunner runner;

    //パリィ範囲
    private GameObject ParryArea;

    ////パリィの効果時間
    private float ParryActivetimeFrame = 0; //フレームに変換する

    ////ヒットストップ時間
    private float HitStopFrame = 0; //フレームに変換する

    /// <summary>
    /// 敵からの攻撃を受けたか判定
    /// </summary>
   // public bool DamageReceive { get; set; } = false;

    Camera Maincamera;
    CinemaCharCamera cinemachar;

    [SerializeField, ReadOnly] bool Parryflg = false;

    HitStop hitStop;

    Knockback back;

    private PlayerState character;


    public PlayerParry(PlayerState character)
    {
        this.character = character;
    }

    /// <summary>
    /// パリィ状態かどうかのチェック(プレイヤーがダメージを受けたときに呼ぶ)
    /// </summary>
    public bool ParryCheck()
    {
        if (ParryArea.activeSelf)
        {
             ParrySystem();
            return true;
        }
        else
        {
            return false;
        }

    }

    //表示時間のゲッター
    public float GetParryActiveTime() { return ParryActivetimeFrame; }

    //パリィ状態かどうか
    public void SetParryflg(bool flg) { Parryflg = flg; }

    public void Enter()
    {
        hitStop = character.GetComponent<HitStop>();
        Maincamera = Camera.main;
        cinemachar = Maincamera.GetComponent<CinemaCharCamera>();
        back = character.GetComponent<Knockback>();
        Vector3 scale = new Vector3(character.parryradius, character.parryradius, character.parryradius);
        for (int i = 0; i < character.transform.childCount; i++)
        {
            if (character.transform.GetChild(i).gameObject.name == "ParryArea")
                ParryArea = character.transform.GetChild(i).gameObject;
        }

        //フレームに直す
        HitStopFrame = character.HitStop / 60;
        ParryActivetimeFrame = character.ParryActivetime / 60;

        ParryArea.transform.localScale = scale;

        ParryArea.SetActive(true);
        Parryflg = true;
    }

    /// <summary>
    /// コントローラー入力
    /// </summary>
    /// <param name="context"></param>
    public void ParryPress(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            ParryArea.SetActive(true);
            Parryflg = true;
        }

    }

    /// <summary>
    /// パリィ成功時の処理
    /// </summary>
    public void ParrySystem()
    {
        hitStop.ApplyHitStop(HitStopFrame);
       // cinemachar.CameraZoom(this.transform,5,0.5f);
        back.ApplyKnockback(character.transform.forward, character.KnockbackPower);
        ParryArea.GetComponent<ParryDisplay>().Init();
    }

    [Rpc(RpcSources.InputAuthority, RpcTargets.All)]
    public void RPC_ParrySystem()
    {
        ParrySystem();
    }

    public void Update()
    {
        //デバック用
        if (ParryArea.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.L))
            {
                ParrySystem();
            }
        }

        if (runner != null)
        {

            // ホスト用の処理
            if (runner.IsServer)
            {
                Debug.Log("This instance is the Host (Server).");
                if (ParryArea.activeSelf)
                {
                    //とりあえずキーボードで仮実装
                    if (Input.GetKeyDown(KeyCode.L))
                    {
                        ParrySystem();
                    }
                }
            }
            else if (runner.IsClient)
            {
                Debug.Log("This instance is a Client.");
                // クライアント用の処理

                // クライアント側での入力処理
                if (ParryArea.activeSelf)
                {
                    if (Input.GetKeyDown(KeyCode.L))
                    {
                        // RPCを通じてホストにパリィを通知
                        RPC_ParrySystem();
                    }
                }
            }
        }
        else
        {
            Debug.LogError("NetworkRunner not found in the scene!");
        }

    }

    public void Exit()
    {
        // Idle状態を抜けるときの処理
        Parryflg = false;
    }

}
