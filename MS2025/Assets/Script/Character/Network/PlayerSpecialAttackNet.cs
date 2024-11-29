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

    [Header("猶予時間"), SerializeField]
    private float specialTimeWait = 0.2f;

    [SerializeField, Tooltip("必殺技使える時のコンボ数の色")]
    private Color specialColor;

    [SerializeField, ReadOnly]
    private List<Image> ComboList = new List<Image>();

    [SerializeField, Tooltip("必殺技使える時のプレイヤーパーティクル")]
    private ParticleSystem Tanukiparticle;

    [SerializeField, Tooltip("必殺技使える時のプレイヤーパーティクル")]
    private ParticleSystem Kituneparticle;

    private AudioSource source;
    [SerializeField, Tooltip("必殺技使えるタイミングで鳴らす音")]
    private AudioClip clip;

    private PlayerParryNet parry;

    [Networked]
    private bool SpecialWait1P { get; set; }

    [Networked]
    private bool SpecialWait2P { get; set; }

    private bool isHost = false;

    public override void Spawned()
    {
        // 必殺技再生用オブジェクト探索
        director = GameObject.Find("Director");
        comboCountObject = GameObject.Find("Networkbox");

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

        source = GetComponent<AudioSource>();
        parry = GetComponent<PlayerParryNet>();

        if (Object.HasInputAuthority)
        {
            isHost = true;
        }
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

            // 攻撃ボタンとスペシャルボタンが押され、コンボ数が指定数以上の場合
            if (SpecialTime > 0.0f && SpecialTime2 > 0.0f && comboCountObject.GetComponent<ShareNumbers>().nCombo >= specialNum)
            {
                if (isHost)
                {
                    SpecialWait1P = true;
                }
                else
                {
                    SpecialWait2P = true;
                }
                Debug.Log("必殺技押した");                
            }

            // 両プレイヤーが必殺技ボタンを押した場合
            if (SpecialWait1P && SpecialWait2P)
            {
                SpecialTime = 0.0f;
                SpecialTime2 = 0.0f;
                SpecialWait1P = false;
                SpecialWait2P = false;

                GetComponent<PlayerMove>().isMove = false; // 移動を一時停止
                Debug.Log("必殺技出すぞ！");
                RPC_SpecialAttack();
            }

            // タイマー減少処理
            if (SpecialTime > 0.0f) SpecialTime -= Time.deltaTime;
            if (SpecialTime2 > 0.0f) SpecialTime2 -= Time.deltaTime;
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
        }
        else
        {
            foreach (var combo in ComboList)
            {
                var color = Color.white;
                color.a = combo.color.a;
                combo.color = color;
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
