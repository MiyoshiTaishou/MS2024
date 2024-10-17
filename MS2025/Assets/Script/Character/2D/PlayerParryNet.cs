using Fusion;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerParryNet : NetworkBehaviour
{
    //パリィ範囲
    private GameObject ParryArea;

    Animator animator;


    [SerializeField, Tooltip("パリィ範囲")] float parryradius = 3;

    [Networked] public NetworkButtons ButtonsPrevious { get; set; }

    //パリィの効果時間
    [SerializeField, Tooltip("パリィ効果時間")] float ParryActivetime = 3;
    private float ParryActivetimeFrame = 0; //フレームに変換する

    //ヒットストップ時間
    [SerializeField, Tooltip("ヒットストップ時間")] private int HitStop = 30;
    private float HitStopFrame = 0; //フレームに変換する

    //ノックバック
    [SerializeField, Tooltip("ノックバック力")] float KnockbackPower = 50;

    //敵からの攻撃を受けたか判定
    public bool DamageReceive { get; set; } = false;

    //Camera Maincamera;
    //CinemaCharCamera cinemachar;

    [SerializeField, ReadOnly] bool Parryflg = false;

    //HitStop hitStop;

    [Networked] private bool PressKey { get; set; } = false;

    //Knockback back;

    private NetworkRunner runner;

    //表示時間のゲッター
    public float GetParryActiveTime() { return ParryActivetimeFrame; }

    //パリィ状態かどうか
    public void SetParryflg(bool flg) { Parryflg = flg; }

    public override void Spawned()
    {
        // NetworkRunnerのインスタンスを取得
        runner = FindObjectOfType<NetworkRunner>();

        animator = GetComponent<Animator>();

        //hitStop = GetComponent<HitStop>();
        //Maincamera = Camera.main;
        //cinemachar = Maincamera.GetComponent<CinemaCharCamera>();
        //back = GetComponent<Knockback>();
        Vector3 scale = new Vector3(parryradius, parryradius, parryradius);
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).gameObject.name == "ParryArea")
                ParryArea = transform.GetChild(i).gameObject;
        }

        ParryArea.gameObject.SetActive(false);

        //フレームに直す
        Debug.Log(Application.targetFrameRate);
        HitStopFrame = HitStop / 60;
        ParryActivetimeFrame = ParryActivetime / 60;

        ParryArea.transform.localScale = scale;
    }

    public void Area()
    {
        ParryArea.SetActive(true);
        Parryflg = true;
    }

    [Rpc(RpcSources.InputAuthority, RpcTargets.All)]
    public void RPC_ParryArea()
    {
        Area();
    }

    /// <summary>
    /// コントローラー入力
    /// </summary>
    /// <param name="context"></param>
    public void ParryPress(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            RPC_ParryArea();
        }

    }

    /// <summary>
    /// パリィ成功時の処理
    /// </summary>
    public void ParrySystem()
    {
        //hitStop.ApplyHitStop(HitStopFrame);
        //cinemachar.CameraZoom(this.transform,5,0.5f);
        //back.ApplyKnockback(transform.forward, KnockbackPower);
        ParryArea.GetComponent<ParryDisplay>().Init();
        DamageReceive = false;
        animator.SetTrigger("Parry");
    }

    [Rpc(RpcSources.InputAuthority, RpcTargets.All)]
    public void RPC_ParrySystem()
    {
        ParrySystem();
    }

    public override void FixedUpdateNetwork()
    {

        //if (runner != null)
        //{
         
        //    // ホスト用の処理
        //    if (runner.IsServer)
        //    {
        //        Debug.Log("This instance is the Host (Server).");
        //        if (Object.HasInputAuthority)
        //        {
        //            if (ParryArea.activeSelf)
        //            {
        //                //とりあえずキーボードで仮実装
        //                if (Input.GetKeyDown(KeyCode.L) || DamageReceive)
        //                {
        //                    RPC_ParrySystem();
        //                }
        //            }
        //        }
        //    }
        //    else if (runner.IsClient)
        //    {
        //        Debug.Log("This instance is a Client.");
        //        // クライアント用の処理

        //        // クライアント側での入力処理
        //        if (ParryArea.activeSelf)
        //        {
        //            if (Input.GetKeyDown(KeyCode.L) || DamageReceive)
        //            {
        //                // RPCを通じてホストにパリィを通知
        //                RPC_ParrySystem();
        //            }
        //        }
        //    }
        //}
        //else
        //{
        //    Debug.LogError("NetworkRunner not found in the scene!");
        //}

        if (Object.HasStateAuthority && GetInput(out NetworkInputData data))
        {
            var pressed = data.Buttons.GetPressed(ButtonsPrevious);
            ButtonsPrevious = data.Buttons;
           
            // ジャンプボタンが押され、かつ地面にいるときジャンプする
            if (pressed.IsSet(NetworkInputButtons.Parry))
            {
                // RPCを通じてホストにパリィを通知
                RPC_ParrySystem();
                RPC_ParryArea();
            }          
        }

    }   
}
