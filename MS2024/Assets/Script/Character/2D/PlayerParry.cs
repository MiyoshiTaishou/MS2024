using Fusion;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.TextCore.Text;

public class PlayerParry : MonoBehaviour
{
    //パリィ範囲
    [SerializeField, ReadOnly] private GameObject ParryArea;

    ////パリィの効果時間
    private float ParryActivetimeFrame = 0; //フレームに変換する

    ////ヒットストップ時間
    private float HitStopFrame = 0; //フレームに変換する

    // パリィ範囲
    [SerializeField, Tooltip("パリィ範囲")] public float parryradius = 3;

    // パリィの効果時間
    [SerializeField, Tooltip("パリィ効果時間")] public float ParryActivetime = 30;

    // ヒットストップ時間
    [SerializeField, Tooltip("ヒットストップ時間")] public int HitStop = 3;

    // ノックバック
    [SerializeField, Tooltip("ノックバック力")] public float KnockbackPower = 10;

    Camera Maincamera;
    CinemaCharCamera cinemachar;

    [SerializeField, ReadOnly] bool Parryflg = false;

    HitStop hitStop;

    Knockback back;

    private Animator animator;

    AudioSource audioSource;

    AudioManager audioManager;

    /// <summary>
    /// パリィ状態かどうかのチェック(プレイヤーがダメージを受けたときに呼ぶ)
    /// </summary>
    public bool ParryCheck()
    {
       // Debug.Log("パリィ!!!");

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

    /// <summary>
    /// パリィ状態かどうかのチェック(パリィジャンプ用)
    /// </summary>
    public bool ParryJumpCheck()
    {
        // Debug.Log("パリィ!!!");

        if (ParryArea.activeSelf)
        {
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

    void Start()
    {
        //SE読み込み
        audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        audioSource = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();//アニメーター
        hitStop = GetComponent<HitStop>();//ヒットストップ
        Maincamera = Camera.main;//カメラ読み込み
        cinemachar = Maincamera.GetComponent<CinemaCharCamera>();
        back = GetComponent<Knockback>();//ノックバック
        //子オブジェクトからパリィエリアを取得
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).gameObject.name == "ParryArea")
            {
                ParryArea = transform.GetChild(i).gameObject;
            }
        }



        //フレームに直す
        HitStopFrame = HitStop / 60;
        ParryActivetimeFrame = ParryActivetime / 60;
        //パリィ範囲
        Vector3 scale = new Vector3(parryradius, parryradius, parryradius);
        ParryArea.transform.localScale = scale;

    }

    public void ParryStart()
    {
        audioManager.PlaySE(audioSource, AudioManager.SESoundData.SE.Parry);

        animator.Play("APlayerParry");
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
            ParryStart();
        }

    }

    /// <summary>
    /// パリィ成功時の処理
    /// </summary>
    public void ParrySystem()
    {
        audioManager.PlaySE(audioSource,AudioManager.SESoundData.SE.ParrySuccess);

        animator.Play("APlayerCounter");

        hitStop.ApplyHitStop(HitStopFrame);
        //cinemachar.CameraZoom(this.character.transform, 5,0.5f);
        back.ApplyKnockback(transform.forward, KnockbackPower);
        ParryArea.GetComponent<ParryDisplay>().Init();
    }

    [Rpc(RpcSources.InputAuthority, RpcTargets.All)]
    public void RPC_ParrySystem()
    {
        ParrySystem();
    }

    void Update()
    {
        //デバック用-----------------------
        if (Input.GetKeyDown(KeyCode.O))
        {
            ParryStart();
        }

        //デバック用
        if (ParryArea.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.L))
            {
                ParrySystem();
            }
        }
        //----------------------------------

        //アニメーション終了
        AnimatorStateInfo landAnimStateInfo = GetComponent<Animator>().GetCurrentAnimatorStateInfo(0);
        if (landAnimStateInfo.IsName("APlayerParry") && landAnimStateInfo.normalizedTime >= 1.0f)
            animator.Play("APlayerIdle");

        if (landAnimStateInfo.IsName("APlayerCounter") && landAnimStateInfo.normalizedTime >= 1.0f)
            animator.Play("APlayerIdle");

    }

    //public void Exit()
    //{
    //    // Idle状態を抜けるときの処理
    //    Parryflg = false;
    //}

}
