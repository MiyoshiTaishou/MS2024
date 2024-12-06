using Fusion;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

public class PlayerSpecialAttackNet : NetworkBehaviour
{
    [Networked] public NetworkButtons ButtonsPrevious { get; set; }
    [Networked] public int specialNum { get; set; }
    [Networked] public int specialDamage { get; set; }

    private GameObject director;
    private GameObject comboCountObject;

    private float SpecialTime = 0.0f;
    private float SpecialTime2 = 0.0f;

    [Networked]
    private bool isSpecialSound { get; set; }

    /// <summary>
    /// 発動待ち時間
    /// </summary>
    private float SpecialActTime = 0.0f;

    [Header("入力猶予時間"), SerializeField]
    private float specialTimeWait = 0.2f;

    [Header("発動猶予時間"), SerializeField]
    private float specialActTimeWait = 10.0f;

    [SerializeField, Tooltip("必殺技使える時のコンボ数の色")]
    private Color specialColor;



    [SerializeField, Tooltip("必殺技使える時のプレイヤーパーティクル")]
    private ParticleSystem Tanukiparticle;

    [SerializeField, Tooltip("必殺技使える時のプレイヤーパーティクル")]
    private ParticleSystem Kituneparticle;

    private AudioSource source;
    [SerializeField, Tooltip("必殺技使えるタイミングで鳴らす音")]
    private AudioClip clip;

    [SerializeField, Tooltip("必殺技ボタンを押した時に鳴らす音")]
    private AudioClip clipSpecial;

    private PlayerParryNet parry;

    [SerializeField, ReadOnly]
    private List<Image> ComboList = new List<Image>();

    [SerializeField, ReadOnly]
    private List<Image> PlayerFireList = new List<Image>();

    GameObject change;

    public override void Spawned()
    {
        // 必殺技再生用オブジェクト探索
        director = GameObject.Find("Director");
        comboCountObject = GameObject.Find("Networkbox");
        change = GameObject.Find("ChangeAction");
        // コンボUIのリストを取得
        var comboUI = GameObject.Find("MainGameUI/Combo");
        for (int j = 0; j < comboUI.transform.childCount; j++)
        {
            var childImage = comboUI.transform.GetChild(j).GetComponent<Image>();
            if (childImage != null)
            {
                ComboList.Add(childImage);
            }
        }

        // コンボUIのリストを取得
        var playerUI = GameObject.Find("MainGameUI/Icon");
        for (int j = 0; j < playerUI.transform.childCount; j++)
        {
            var childImage = playerUI.transform.GetChild(j).GetComponent<Image>();
            if (childImage != null)
            {
                PlayerFireList.Add(childImage);
            }
        }


        source = GetComponent<AudioSource>();
        parry = GetComponent<PlayerParryNet>();      
    }

    public override void FixedUpdateNetwork()
    {
        if (Object.HasStateAuthority && GetInput(out NetworkInputData data))
        {
            var pressed = data.Buttons.GetPressed(ButtonsPrevious);
            ButtonsPrevious = data.Buttons;

            if (pressed.IsSet(NetworkInputButtons.Special))
            {
                SpecialTime = specialTimeWait;
            }

            if (pressed.IsSet(NetworkInputButtons.Attack))
            {
                SpecialTime2 = specialTimeWait;
            }

            // 攻撃ボタンとスペシャルボタンが押され、コンボ数が指定数以上の場合かつ必殺技待ち時間が0な場合
            if (SpecialTime > 0.0f && SpecialTime2 > 0.0f && SpecialActTime == 0.0f && comboCountObject.GetComponent<ShareNumbers>().nCombo >= specialNum)
            {                
                Debug.Log("必殺技押した");
                SpecialTime = 0.0f;
                SpecialTime2 = 0.0f;
                GetComponent<PlayerMove>().isMove = false; // 移動を一時停止

                isSpecialSound = true;

                //二人が押していたら発動可能
                if(comboCountObject.GetComponent<ShareNumbers>().AddSpecialNum())
                {
                    Debug.Log("必殺技出すぞ！");
                    RPC_SpecialAttack();

                    if(change.GetComponent<ChangeBossAction>().TextNo == 1)
                     {
                        change.GetComponent<ChangeBossAction>().TextNo = 2;
                    }
                    
                    //カウントを0にする
                    comboCountObject.GetComponent<ShareNumbers>().ResetSpecialNUm();
                }
                else
                {
                    //発動できなかった場合相方待ち時間を追加する
                    SpecialActTime = specialActTimeWait;
                }
            }

            // タイマー減少処理
            if (SpecialTime > 0.0f)
            {
                SpecialTime -= Time.deltaTime;
            }

            if (SpecialTime2 > 0.0f)
            {
                SpecialTime2 -= Time.deltaTime;
            }

            if (SpecialActTime > 0.0f)
            {
                SpecialActTime -= Time.deltaTime;
            }
            else
            {
                GetComponent<PlayerMove>().isMove = true; // 移動を一時停止
            }
        }

        // コンボUIの色変更処理
        UpdateComboUI();
    }

    private void UpdateComboUI()
    {
        if (comboCountObject.GetComponent<ShareNumbers>().nCombo >= specialNum)
        {
            foreach (var combo in ComboList)
            {
                combo.color = specialColor;
            }

            foreach (var player in PlayerFireList)
            {
                player.color= Color.white;
            }
        }
        else
        {
            foreach (var combo in ComboList)
            {
                var color = Color.white;
                color.a = combo.color.a;
                combo.color = color;
            }

            foreach (var player in PlayerFireList)
            {
                Color color= Color.white;
                color.a = 0;
                player.color = color;
            }
        }
    }

    public override void Render()
    {
        if (comboCountObject.GetComponent<ShareNumbers>().nCombo >= specialNum)
        {
            PlaySpecialParticles();
        }
        else
        {
            StopSpecialParticles();
        }        

        if(isSpecialSound)
        {
            source.PlayOneShot(clipSpecial);
            isSpecialSound = false;
        }
    }

    private void PlaySpecialParticles()
    {
        if (parry.isTanuki && !Tanukiparticle.isPlaying)
        {
            Tanukiparticle.Play();
            source.PlayOneShot(clip);
        }
        else if (!parry.isTanuki && !Kituneparticle.isPlaying)
        {
            Kituneparticle.Play();
            source.PlayOneShot(clip);
        }
    }

    private void StopSpecialParticles()
    {
        if (Tanukiparticle.isPlaying) Tanukiparticle.Stop();
        if (Kituneparticle.isPlaying) Kituneparticle.Stop();
    }

    [Rpc(RpcSources.All, RpcTargets.All)]
    public void RPC_SpecialAttack()
    {
        director.GetComponent<PlayableDirector>().Play();
    }
}
