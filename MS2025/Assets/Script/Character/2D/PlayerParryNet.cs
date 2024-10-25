using Fusion;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

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

    /// <summary>
    /// パリィ状態かどうか
    /// </summary>
    [SerializeField,Networked] public bool isParry { get; private set; } = false;

    /// <summary>
    /// パリィ状態かどうか
    /// </summary>
    [SerializeField,Networked] bool isParrySuccess { get; set; } = false;

    [SerializeField,Networked] bool isParryAnimation { get; set; } = false;

    HitStop hitStop;

    Knockback back;

    private NetworkRunner runner;
    private NetworkObject networkobject;

    [SerializeField,Networked] bool isHost { get; set; } = false;
    //[SerializeField,ReadOnly] private bool _isHost => isHost;

    private GameObject playerhost;

    //表示時間のゲッター
    public float GetParryActiveTime() { return ParryActivetimeFrame; }

    //パリィ状態かどうか
    public void SetParryflg(bool flg) { isParry = flg; }

    // アニメーション名をネットワーク同期させる
    [Networked]
    private NetworkString<_16> networkedAnimationName { get; set; }

    /// <summary>
    /// パリィ状態かどうかのチェック(プレイヤーがダメージを受けたときに呼ぶ)
    /// </summary>
    public bool ParryCheck()
    {
        //Debug.Log("パリィ!!!");

        if (isParry)
        {
            return true;
        }
        else
        {
            return false;
        }

    }
    public void Counter()
    {

        if (ParryArea.GetComponent<ParryDisplayNet>().Hit)
        {
            RPC_ParrySystem();
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
        HitStopFrame = HitStop / 60;
        ParryActivetimeFrame = ParryActivetime / 60;

        ParryArea.transform.localScale = scale;
        networkobject = FindObjectOfType<NetworkObject>();

        if (Object.HasStateAuthority)
        {
            Debug.Log("ホストです");
           isHost= true;
        }


        // "Player(Clone)"という名前のオブジェクトを全て取得
        GameObject[] players = FindObjectsOfType<GameObject>();

        foreach (GameObject player in players)
        {
            if (player.name == "Player(Clone)")
            {

               if( player.GetComponent<PlayerParryNet>().isHost)
                {
                    playerhost = player;
                }
            }
        }
    }

    public void Area()
    {
        ParryArea.SetActive(true);
        isParry = true;
        isParryAnimation = true;

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

        Debug.Log("パリィシステム");
        audioSource.PlayOneShot(ParrySuccessSE);

        animator.Play("APlayerCounter");
       // animator.SetTrigger("ParrySuccess"); // アニメーションのトリガー

        //hitStop.ApplyHitStop(HitStopFrame);
        //cinemachar.CameraZoom(this.character.transform, 5,0.5f);
       // back.ApplyKnockback(transform.forward, KnockbackPower);
        ParryArea.GetComponent<ParryDisplayNet>().Init();

        isParrySuccess = true;
    }

    [Rpc(RpcSources.All, RpcTargets.All)]
    public void RPC_ParrySystem()
    {
        if (ParryArea.GetComponent<ParryDisplayNet>().Hit)
            ParrySystem();
    }

    public void ParryStart()
    {
        audioSource.PlayOneShot(ParrySE);
       // animator.SetTrigger("Parry"); // アニメーションのトリガー

        animator.Play("APlayerParry");
        ParryArea.SetActive(true);
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
            if (pressed.IsSet(NetworkInputButtons.Parry) && !isParry)
            {
                
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

        if(playerhost)
        {
            // "Player(Clone)"という名前のオブジェクトを全て取得
            GameObject[] players = FindObjectsOfType<GameObject>();

            foreach (GameObject player in players)
            {
                if (player.name == "Player(Clone)")
                {

                    if (player.GetComponent<PlayerParryNet>().isHost)
                    {
                        playerhost = player;
                    }
                }
            }

        }

        if (playerhost.GetComponent<PlayerParryNet>().isParrySuccess ) //パリィアニメーション中かどうか
        {
            Debug.Log("パリィカウンター");
            animator.Play("APlayerCounter");
            playerhost.GetComponent<PlayerParryNet>().isParrySuccess = false;
        }
        
        else if (isParry && isParryAnimation) //パリィアニメーション中かどうか
        {
            Debug.Log("パリィ");

            animator.SetTrigger("Parry"); // アニメーションのトリガー
            isParryAnimation = false; // フラグをリセット
        }
    }
}
