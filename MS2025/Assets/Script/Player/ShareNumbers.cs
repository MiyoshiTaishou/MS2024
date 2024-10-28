using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShareNumbers : NetworkBehaviour
{
    [Networked] public int CurrentHP { get; set; }
    [Networked] public int nHitnum { get; set; }
    public int maxHitnum { get; set; }

    [Networked] public int nCombo { get; set; }

    [Networked] public bool isSpecial { get; set; }

    public GameObject Boss;

    [SerializeField] private GameObject[] HPUI;

    private bool isOnce = false;

    int magnification = 2;
    [SerializeField] int damage = 10;

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
    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    public void RPC_Damage()
    {
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
        HPUI = new GameObject[5];

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

        CurrentHP--;

        HPUI[CurrentHP].SetActive(false);
    }
    
    public void BossDamage()
    {        
        //�{���v�Z��
        magnification = (nCombo - 1) / 5 * 2 + 2;
        int totalDamage = magnification * damage;

        Debug.Log("�K�E�Z�_���[�W" + totalDamage);

        Boss.GetComponent<BossStatus>().RPC_Damage(totalDamage);

        nCombo = 0;

        isSpecial = false;
    }

    public void SpecialStart()
    {
        isSpecial = true;
    }

    public override void Spawned()
    {
        maxHitnum = 3;
        nHitnum = 0;
        CurrentHP =5;
        nCombo = 0;
        Debug.Log("�v���C���[��HP�Ƃ�������");         
    }    
}
