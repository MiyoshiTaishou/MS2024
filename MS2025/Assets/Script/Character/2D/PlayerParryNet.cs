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

    private Animator animator;

    AudioSource audioSource;

    AudioManager audioManager;

    [Header("パリィSE"), SerializeField] private AudioClip ParrySE;
    [Header("パリィ成功SE"), SerializeField] private AudioClip ParrySuccessSE;

    [SerializeField, Tooltip("パリィ範囲")] float parryradius = 3;

    [Networked] public NetworkButtons ButtonsPrevious { get; set; }

    //パリィの効果時間
    [SerializeField, Tooltip("パリィ効果時間")] float ParryActivetime = 3;
    [Networked] private float ParryActivetimeFrame { get; set; } = 0; //フレームに変換する

    //ヒットストップ時間
    [SerializeField, Tooltip("ヒットストップ時間")] private int HitStop = 30;
    private float HitStopFrame = 0; //フレームに変換する

    //ノックバック
    [SerializeField, Tooltip("ノックバック力")] float KnockbackPower = 50;

    //敵からの攻撃を受けたか判定
    public bool DamageReceive { get; set; } = false;

    //Camera Maincamera;
    //CinemaCharCamera cinemachar;

    [SerializeField,Networked] bool Parryflg { get; set; } = false;

    HitStop hitStop;

    [Networked] private bool PressKey { get; set; } = false;

    Knockback back;

    private NetworkRunner runner;

    //表示時間のゲッター
    public float GetParryActiveTime() { return ParryActivetimeFrame; }

    //パリィ状態かどうか
    public void SetParryflg(bool flg) { Parryflg = flg; }

    // アニメーション名をネットワーク同期させる
    [Networked]
    private NetworkString<_16> networkedAnimationName { get; set; }

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

    public override void Spawned()
    {
        // NetworkRunnerのインスタンスを取得
        runner = FindObjectOfType<NetworkRunner>();

        //SE読み込み
        //audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        audioSource = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();//アニメーター
        hitStop = GetComponent<HitStop>();
        //Maincamera = Camera.main;
        //cinemachar = Maincamera.GetComponent<CinemaCharCamera>();
        back = GetComponent<Knockback>();
        Vector3 scale = new Vector3(parryradius, parryradius, parryradius);
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).gameObject.name == "ParryArea")
                ParryArea = transform.GetChild(i).gameObject;
        }

        ParryArea.gameObject.SetActive(false);

        //フレームに直す
        //Debug.Log(Application.targetFrameRate);
        HitStopFrame = HitStop / 60;
        ParryActivetimeFrame = ParryActivetime / 60;

        ParryArea.transform.localScale = scale;


    }

    public void Area()
    {
        ParryArea.SetActive(true);
        Parryflg = true;
    }

    [Rpc(RpcSources.All, RpcTargets.All)]
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
        AnimatorStateInfo landAnimStateInfo2 = GetComponent<Animator>().GetCurrentAnimatorStateInfo(0);

        //パリィ中は動かせないようにする
        if (landAnimStateInfo2.IsName("APlayerAtack1") || landAnimStateInfo2.IsName("APlayerAtack2") || landAnimStateInfo2.IsName("APlayerAtack3"))
        {
            return;
        }

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
        audioSource.PlayOneShot(ParrySuccessSE);

        animator.Play("APlayerCounter");

        hitStop.ApplyHitStop(HitStopFrame);
        //cinemachar.CameraZoom(this.character.transform, 5,0.5f);
        back.ApplyKnockback(transform.forward, KnockbackPower);
        ParryArea.GetComponent<ParryDisplayNet>().Init();
    }

    [Rpc(RpcSources.All, RpcTargets.All)]
    public void RPC_ParrySystem()
    {
        ParrySystem();
    }

    public void ParryStart()
    {
        audioSource.PlayOneShot(ParrySE);

        animator.Play("APlayerParry");
        ParryArea.SetActive(true);
        Parryflg = true;
    }


    public override void FixedUpdateNetwork()
    {
        AnimatorStateInfo landAnimStateInfo = GetComponent<Animator>().GetCurrentAnimatorStateInfo(0);

        if (Object.HasStateAuthority && GetInput(out NetworkInputData data))
        {
            //パリィ中は動かせないようにする
            if (landAnimStateInfo.IsName("APlayerAtack1") || landAnimStateInfo.IsName("APlayerAtack2") || landAnimStateInfo.IsName("APlayerAtack3"))
            {
                return;
            }

            var pressed = data.Buttons.GetPressed(ButtonsPrevious);
            ButtonsPrevious = data.Buttons;

            // Attackボタンが押されたか、かつアニメーションが再生中でないかチェック
            if (pressed.IsSet(NetworkInputButtons.Parry) && !Parryflg)
            {
                Debug.Log("パリィ開始");
                ParryStart();
                RPC_ParryArea();
            }

        }

    }

    public override void Render()
    {
        if (Object.HasStateAuthority)
        {
            return;
        }

        // クライアント側でもアニメーションを再生（ネットワーク変数が変わったときに実行）
        // 現在のアニメーションの状態を取得
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        // 攻撃フラグが立っている場合にアニメーションをトリガー
        if (Parryflg && Animator.StringToHash("Parry") != stateInfo.shortNameHash) //パリィアニメーション中かどうか
        {
            animator.SetTrigger("Parry"); // アニメーションのトリガー
            //Parryflg = false; // フラグをリセット
        }
    }
}
