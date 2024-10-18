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

    [SerializeField] private GameObject[] HPUI;

    private bool isOnce = false;

    public void AddHitnum()
    {
        nHitnum++;
        if (nHitnum >= maxHitnum)
        {
            nHitnum = 0;
        }
        Debug.Log("�A����:" + nHitnum);
    }



    public override void Spawned()
    {
        maxHitnum = 3;
        nHitnum = 0;
        CurrentHP =5;
        nCombo = 0;
        Debug.Log("�v���C���[��HP�Ƃ�������");         
    }

    public override void FixedUpdateNetwork()
    {
        if (!isOnce)
        {
            isOnce = true;

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
            HPUI = new GameObject[allChildren.Length];

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
        }

        switch (CurrentHP)
        {
            case 0:
                HPUI[CurrentHP].SetActive(false);
                break;
            case 1:
                HPUI[CurrentHP].SetActive(false);
                break;
            case 2:
                HPUI[CurrentHP].SetActive(false);
                break;
            case 3:
                HPUI[CurrentHP].SetActive(false);
                break;
            case 4:
                HPUI[CurrentHP].SetActive(false);
                break;
        }
    }

}
