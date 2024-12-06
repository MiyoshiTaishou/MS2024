using Fusion;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class ShareNumbers : NetworkBehaviour
{
    [Networked] public int CurrentHP { get; set; }
    [Networked] public int nHitnum { get; set; }
    public int maxHitnum { get; set; }

    [Networked] public int nCombo { get; set; }
    [Networked]public int maxCombo { get; set; }
    [Networked] public int jumpAttackNum { get; set; }
    [Networked] private int specilaCombo { get; set; }

    [Networked] public bool isSpecial { get; set; }

    //�K�E�Z�����̃J�E���g������
    [Networked] private int specialNum { get; set; }

    public GameObject Boss;

    [SerializeField] private GameObject[] HPUI;

    private bool isOnce = false;

    int magnification = 2;
    [SerializeField] int damage = 10;

    [Header("���񂾂Ƃ��̑J�ڐ�V�[����"), SerializeField]
    private String SceneName;

    private NetworkRunner networkRunner;

    [SerializeField]
    private TransitionManager transitionManager;

    [SerializeField]
    private GameObject TryObject;

    public override void FixedUpdateNetwork()
    {
        if(nCombo == 0)
        {
            nHitnum = 0;
        }
    }

    public void AddHitnum()
    {
        nHitnum++;
        if (nHitnum >= maxHitnum)
        {
            nHitnum = 0;
        }
        Debug.Log("�A����:" + nHitnum);
    }

    /// <summary>
    /// �Q�X�g���ɑޏo���߂𑗐M
    /// </summary>
    [Rpc(RpcSources.All, RpcTargets.All)]
    public void RPC_Damage()
    {              
        HPUI[CurrentHP].SetActive(false);

        if (CurrentHP == 0)
        {            
            transitionManager.TransitionStart();            
            StartCoroutine(Load());
        }
    
    }

    public void BossDamage()
    {
        Debug.Log("�K�E�Z�O�R���{��" + specilaCombo);
        // �R���{���� 0 �̏ꍇ�͔{�����Œ�l�ɂ���
        if (specilaCombo <= 0)
        {
            magnification = 2; // �R���{�Ȃ��̏ꍇ�̊�{�{��
        }
        else
        {
            // �R���{���Ɋ�Â����{���v�Z
            // ��: �R���{���������邲�Ƃɔ{�����w���֐��I�ɏ㏸
            magnification = (int)(2.0f + Mathf.Log(specilaCombo + 1, 1.2f)); // �x�[�X��1.2�𒲐����Ĕ{���̏オ�����ύX
        }

        Debug.Log("�v�Z���ꂽ�{��: " + magnification);

        // �_���[�W�v�Z
        int totalDamage = magnification * damage;
        Debug.Log("�ŏI�_���[�W: " + totalDamage);

        // �{�X�Ƀ_���[�W�𑗐M
        Boss.GetComponent<BossStatus>().RPC_Damage(totalDamage);

        // �R���{�����Z�b�g
        nCombo = 0;
        isSpecial = false;
    }


    public void SpecialStart()
    {
        isSpecial = true;
        specilaCombo = nCombo;
    }

    public override void Spawned()
    {
        maxHitnum = 3;
        nHitnum = 0;
        CurrentHP =5;
        nCombo = 0;
        jumpAttackNum = 0;
        Debug.Log("�v���C���[��HP�Ƃ�������");

        Debug.Log("�q�I�u�W�F�N�g�T��");

        // "MainGameUI" �̃I�u�W�F�N�g���������擾�ł��Ă��邩�m�F
        GameObject obj = GameObject.Find("MainGameUI");
        if (obj == null)
        {
            Debug.LogError("MainGameUI �I�u�W�F�N�g��������܂���");
            return;
        }

        // GetComponentsInChildren �őS�Ă̊K�w�̎q�I�u�W�F�N�g���擾
        Transform[] allChildren = obj.GetComponentsInChildren<Transform>();

        // �q�v�f�����Ȃ���ΏI��
        if (allChildren.Length == 0)
        {
            Debug.LogError("MainGameUI �̎q�v�f������܂���");
            return;
        }

        // HPUI �z��̃T�C�Y��S�Ă̎q�I�u�W�F�N�g���ɍ��킹�ď�����
        HPUI = new GameObject[10];

        int num = 0;
        foreach (Transform ob in allChildren)
        {
            if (ob.CompareTag("HPUI"))
            {
                Debug.Log("HPUI �I�u�W�F�N�g����");
                HPUI[num] = ob.gameObject;
                num++;
            }
        }

        // HPUI ��������Ȃ������ꍇ�̑Ώ�
        if (num == 0)
        {
            Debug.LogError("HPUI ��������܂���ł���");
        }

        networkRunner = FindObjectOfType<NetworkRunner>();
    }

    private IEnumerator Load()
    {
        yield return new WaitForSeconds(2f);
        TryObject.SetActive(true);
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    private void RPC_ClientSceneTransition()
    {
        // �N���C�A���g�͐�ɃV�[���J�ڂ����s
        if (!Object.HasStateAuthority)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(SceneName);
        }
        else
        {
            // �z�X�g���̓N���C�A���g�̑J�ڂ�����������ɃV�[���J��
            StartCoroutine(HostSceneTransition());
        }
    }

    private IEnumerator HostSceneTransition()
    {
        yield return new WaitForSeconds(2); // �N���C�A���g�����V�[���J�ڂ���܂ł̎��Ԃ𒲐�
        Runner.Shutdown();
        UnityEngine.SceneManagement.SceneManager.LoadScene(SceneName);
    }

    public bool AddSpecialNum()
    {
        specialNum++;

        //��l�Ƃ���������true��Ԃ��ĕK�E�Z�𔭓�������
        if(specialNum == 2)
        {
            specialNum = 0;
            return true;
        }

        return false;
    }

    public void ResetSpecialNUm()
    {
        specialNum = 0;
    }
}
