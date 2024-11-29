using Fusion;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

public class PlayerSpecialAttackNet : NetworkBehaviour
{
    [Networked] public NetworkButtons ButtonsPrevious { get; set; }
    [Networked] public int specialNum { get; set; }
    [Networked] public int specialDamage { get; set; }
   
    GameObject director;
    GameObject comboCountObject;

    private float SpecialTime = 0.0f;
    private float SpecialTime2 = 0.0f;

    [Header("猶予時間"), SerializeField]
    private float specialTimeWait = 0.2f;

    [SerializeField,Tooltip("必殺技使える時のコンボ数の色")] Color specialColor;
    [SerializeField,ReadOnly] private List<Image> ComboList;

    [SerializeField, Tooltip("必殺技使える時のプレイヤーパーティクル")] private ParticleSystem Tanukiparticle;
    [SerializeField, Tooltip("必殺技使える時のプレイヤーパーティクル")] private ParticleSystem Kituneparticle;

    private AudioSource source;
    [SerializeField, Tooltip("必殺技使えるタイミングで鳴らす音")] private AudioClip clip;

    PlayerParryNet parry;

    [Networked]
    private bool SpecialWait1P { get;set; }

    [Networked]
    private bool SpecialWait2P { get; set; }

    private bool isHost = false;

    private float SpecialTimeDouble = 10.0f;

    public override void Spawned()
    {
        //必殺技再生用オブジェクト探索
        director = GameObject.Find("Director");
        comboCountObject = GameObject.Find("Networkbox");
        for (int j = 0; j < GameObject.Find("MainGameUI/Combo").transform.childCount; j++)
        {
            if (GameObject.Find("MainGameUI/Combo").transform.GetChild(j).GetComponent<Image>())
            {
                ComboList.Add(GameObject.Find("MainGameUI/Combo").transform.GetChild(j).GetComponent<Image>());
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

            if(pressed.IsSet(NetworkInputButtons.Special))
            {
                SpecialTime = specialTimeWait;
            }
            
            if(pressed.IsSet(NetworkInputButtons.Attack))
            {
                SpecialTime2 = specialTimeWait;
            }

            //攻撃ボタンを押したときにコンボカウントが指定の数を超えてる場合再生
            if (SpecialTime > 0.0f && SpecialTime2 > 0.0f && comboCountObject.GetComponent<ShareNumbers>().nCombo >= specialNum)
            {               
                if(isHost)
                {
                    SpecialWait1P = true;
                }
                else
                {
                    SpecialWait2P = true;
                }
                
                SpecialTimeDouble = 10.0f;
            }

            //二人とも押したら
            if(SpecialWait1P && SpecialWait2P)
            {
                SpecialTime = 0.0f;
                SpecialTime2 = 0.0f;
                GetComponent<PlayerMove>().isMove = false;
                RPC_SpecialAttack();
            }


            if(SpecialTime > 0.0f)
            {
                SpecialTime -= Time.deltaTime;
            }

            if (SpecialTime2 > 0.0f)
            {
                SpecialTime2 -= Time.deltaTime;
            }

            if(SpecialTimeDouble > 0.0f)
            {
                SpecialTimeDouble -= Time.deltaTime;
            }
        }

        if (comboCountObject.GetComponent<ShareNumbers>().nCombo >= specialNum)
        {
            for(int i = 0;i < ComboList.Count;i++)
            {
                ComboList[i].color = specialColor;
            }
        }
        else
        {
            for (int i = 0; i < ComboList.Count; i++)
            {
                Color color = Color.white;
                color.a = ComboList[i].color.a;
                ComboList[i].color = color;
            }
        }
    }

    public override void Render()
    {
        if (comboCountObject.GetComponent<ShareNumbers>().nCombo >= specialNum)
        {

            
            if (parry.isTanuki)
            {
                if (!Tanukiparticle.isPlaying)
                {
                    Tanukiparticle.Play();
                    source.PlayOneShot(clip);
                }



            }
            else
            {
                if (!Kituneparticle.isPlaying)
                {
                    Kituneparticle.Play();
                    source.PlayOneShot(clip);
                }


            }

        }
        else
        {
            Tanukiparticle.Stop();
            Kituneparticle.Stop();
        }

    }

    [Rpc(RpcSources.All, RpcTargets.All)]
    public void RPC_SpecialAttack()
    {
        director.GetComponent<PlayableDirector>().Play();       
    }
}
