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

    [Header("�P�\����"), SerializeField]
    private float specialTimeWait = 0.2f;

    [SerializeField, Tooltip("�K�E�Z�g���鎞�̃R���{���̐F")]
    private Color specialColor;

    [SerializeField, ReadOnly]
    private List<Image> ComboList = new List<Image>();

    [SerializeField, Tooltip("�K�E�Z�g���鎞�̃v���C���[�p�[�e�B�N��")]
    private ParticleSystem Tanukiparticle;

    [SerializeField, Tooltip("�K�E�Z�g���鎞�̃v���C���[�p�[�e�B�N��")]
    private ParticleSystem Kituneparticle;

    private AudioSource source;
    [SerializeField, Tooltip("�K�E�Z�g����^�C�~���O�Ŗ炷��")]
    private AudioClip clip;

    private PlayerParryNet parry;

    [Networked]
    private bool SpecialWait1P { get; set; }

    [Networked]
    private bool SpecialWait2P { get; set; }

    private bool isHost = false;

    public override void Spawned()
    {
        // �K�E�Z�Đ��p�I�u�W�F�N�g�T��
        director = GameObject.Find("Director");
        comboCountObject = GameObject.Find("Networkbox");

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

            // �U���{�^���ƃX�y�V�����{�^����������A�R���{�����w�萔�ȏ�̏ꍇ
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
                Debug.Log("�K�E�Z������");                
            }

            // ���v���C���[���K�E�Z�{�^�����������ꍇ
            if (SpecialWait1P && SpecialWait2P)
            {
                SpecialTime = 0.0f;
                SpecialTime2 = 0.0f;
                SpecialWait1P = false;
                SpecialWait2P = false;

                GetComponent<PlayerMove>().isMove = false; // �ړ����ꎞ��~
                Debug.Log("�K�E�Z�o�����I");
                RPC_SpecialAttack();
            }

            // �^�C�}�[��������
            if (SpecialTime > 0.0f) SpecialTime -= Time.deltaTime;
            if (SpecialTime2 > 0.0f) SpecialTime2 -= Time.deltaTime;
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
