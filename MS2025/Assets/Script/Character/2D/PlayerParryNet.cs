using Fusion;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using static UnityEngine.ParticleSystem;

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

    /// <summary>
    /// パリィ状態かどうか
    /// </summary>
    [Networked] bool isParry { get; set; } = false;

    /// <summary>
    /// パリィ状態かどうか
    /// </summary>
    [Networked] bool isParrySuccess { get; set; } = false;

    [Networked] bool isParryAnimation { get; set; } = false;

    HitStop hitStop;

    Knockback back;

    private NetworkRunner runner;
    private NetworkObject networkobject;

    [SerializeField,Networked] bool isHost { get; set; } = false;
    //[SerializeField,ReadOnly] private bool _isHost => isHost;

    private GameObject playerhost;

    [SerializeField, Tooltip("エフェクトパリィ")]
    GameObject Parryeffect;

    ParticleSystem particle;

    [SerializeField, Tooltip("エフェクトカウンター")]
    GameObject Countereffect;

    ParticleSystem counterparticle;

    //表示時間のゲッター
    public float GetParryActiveTime() { return ParryActivetimeFrame; }

    //パリィ状態かどうか
    public void SetParryflg(bool flg) { isParry = flg; }

    // アニメーション名をネットワーク同期させる
    [Networked]
    private NetworkString<_16> networkedAnimationName { get; set; }

    private bool isGround = false;

    [Networked] bool NetParryeffect { get; set; } = false;

    [Networked] bool NetCountereffect { get; set; } = false;

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

        if (Object.HasInputAuthority)
        {
           isHost= true;
        }

        particle = Parryeffect.GetComponent<ParticleSystem>();

        counterparticle = Countereffect.GetComponent<ParticleSystem>();

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
    /// パリィ成功時の処理
    /// </summary>
    public void ParrySystem()
    {

        Debug.Log("パリィシステム");
        audioSource.PlayOneShot(ParrySuccessSE);
        animator.Play("APlayerCounter");
        // animator.SetTrigger("ParrySuccess"); // アニメーションのトリガー
        NetCountereffect = true;
        hitStop.ApplyHitStop(HitStopFrame);
        //cinemachar.CameraZoom(this.character.transform, 5,0.5f);
        back.ApplyKnockback(transform.forward, KnockbackPower);
        ParryArea.GetComponent<ParryDisplayNet>().Init();

        isParrySuccess = true;
        isParry = false;
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
        NetParryeffect = true;
    }


    public override void FixedUpdateNetwork()
    {
        AnimatorStateInfo landAnimStateInfo = GetComponent<Animator>().GetCurrentAnimatorStateInfo(0);

        if (GetInput(out NetworkInputData data))
        {
            //パリィ中は動かせないようにする
            if (landAnimStateInfo.IsName("APlayerAtack1") || landAnimStateInfo.IsName("APlayerAtack2") || landAnimStateInfo.IsName("APlayerAtack3"))
            {
                return;
            }

            var pressed = data.Buttons.GetPressed(ButtonsPrevious);
            ButtonsPrevious = data.Buttons;

            // Attackボタンが押されたか、かつアニメーションが再生中でないかチェック
            if (pressed.IsSet(NetworkInputButtons.Parry) && !isParry && isGround /*地上にいるかの判定*/)
            {
                
                ParryStart();
                RPC_ParryArea();
            }

        }

    }

    private void OnCollisionEnter(Collision collision)
    {
        // 地上にいるか
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGround = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGround = false;
        }
    }

    public override void Render()
    {
        if (isParry && isParryAnimation) //パリィアニメーション中かどうか
        {
            Debug.Log("パリィクライアント");
            NetParryeffect = true;
            animator.Play("APlayerParry");
            isParryAnimation = false; // フラグをリセット
        }

        if (isParrySuccess) //パリィアニメーション中かどうか
        {
            Debug.Log("パリィカウンタークライアント");
            NetCountereffect = true;
            animator.Play("APlayerCounter");
            isParrySuccess = false;
        }

        if (NetParryeffect)
        {
            Vector3 pos = transform.position;
            pos.y -= this.gameObject.transform.localScale.y / 2;//直打ちだけどプレイヤーの足元まで下げる
            pos.y += 0.5f;//地面と重ならないように少し浮かす
            Instantiate(particle,pos, Quaternion.identity);
            NetParryeffect = false;
        }

        if (NetCountereffect)
        {
            counterparticle.Play();
            NetCountereffect = false;
        }

        //ホストなら終了
        //if (Object.HasInputAuthority)
        //{
        //    return;
        //}


    }
}
