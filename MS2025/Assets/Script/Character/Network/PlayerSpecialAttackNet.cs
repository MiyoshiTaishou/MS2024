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
    /// �����҂�����
    /// </summary>
    private float SpecialActTime = 0.0f;

    [Header("���͗P�\����"), SerializeField]
    private float specialTimeWait = 0.2f;

    [Header("�����P�\����"), SerializeField]
    private float specialActTimeWait = 10.0f;

    [SerializeField, Tooltip("�K�E�Z�g���鎞�̃R���{���̐F")]
    private Color specialColor;



    [SerializeField, Tooltip("�K�E�Z�g���鎞�̃v���C���[�p�[�e�B�N��")]
    private ParticleSystem Tanukiparticle;

    [SerializeField, Tooltip("�K�E�Z�g���鎞�̃v���C���[�p�[�e�B�N��")]
    private ParticleSystem Kituneparticle;

    private AudioSource source;
    [SerializeField, Tooltip("�K�E�Z�g����^�C�~���O�Ŗ炷��")]
    private AudioClip clip;

    [SerializeField, Tooltip("�K�E�Z�{�^�������������ɖ炷��")]
    private AudioClip clipSpecial;

    private PlayerParryNet parry;

    [SerializeField, ReadOnly]
    private List<Image> ComboList = new List<Image>();

    [SerializeField, ReadOnly]
    private List<Image> PlayerFireList = new List<Image>();

    GameObject change;

    public override void Spawned()
    {
        // �K�E�Z�Đ��p�I�u�W�F�N�g�T��
        director = GameObject.Find("Director");
        comboCountObject = GameObject.Find("Networkbox");
        change = GameObject.Find("ChangeAction");
        // �R���{UI�̃��X�g���擾
        var comboUI = GameObject.Find("MainGameUI/Combo");
        for (int j = 0; j < comboUI.transform.childCount; j++)
        {
            var childImage = comboUI.transform.GetChild(j).GetComponent<Image>();
            if (childImage != null)
            {
                ComboList.Add(childImage);
            }
        }

        // �R���{UI�̃��X�g���擾
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

            // �U���{�^���ƃX�y�V�����{�^����������A�R���{�����w�萔�ȏ�̏ꍇ���K�E�Z�҂����Ԃ�0�ȏꍇ
            if (SpecialTime > 0.0f && SpecialTime2 > 0.0f && SpecialActTime == 0.0f && comboCountObject.GetComponent<ShareNumbers>().nCombo >= specialNum)
            {                
                Debug.Log("�K�E�Z������");
                SpecialTime = 0.0f;
                SpecialTime2 = 0.0f;
                GetComponent<PlayerMove>().isMove = false; // �ړ����ꎞ��~

                isSpecialSound = true;

                //��l�������Ă����甭���\
                if(comboCountObject.GetComponent<ShareNumbers>().AddSpecialNum())
                {
                    Debug.Log("�K�E�Z�o�����I");
                    RPC_SpecialAttack();

                    if(change.GetComponent<ChangeBossAction>().TextNo == 1)
                     {
                        change.GetComponent<ChangeBossAction>().TextNo = 2;
                    }
                    
                    //�J�E���g��0�ɂ���
                    comboCountObject.GetComponent<ShareNumbers>().ResetSpecialNUm();
                }
                else
                {
                    //�����ł��Ȃ������ꍇ�����҂����Ԃ�ǉ�����
                    SpecialActTime = specialActTimeWait;
                }
            }

            // �^�C�}�[��������
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
                GetComponent<PlayerMove>().isMove = true; // �ړ����ꎞ��~
            }
        }

        // �R���{UI�̐F�ύX����
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
