using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using static UnityEngine.ParticleSystem;
using System.Reflection;
using Unity.VisualScripting;
using UnityEditor.Presets;

public class PlayerChargeAttack : NetworkBehaviour
{
    [Networked] public NetworkButtons ButtonsPrevious { get; set; }
    GameObject attackArea;
    [SerializeField]
    GameObject netobj;
    ShareNumbers sharenum;

    bool isCharge=false;//�`���[�W�����ۂ�
    [SerializeField, Tooltip("����f")]
    int Startup;
    [SerializeField, Tooltip("����f")]
    int Active;
    [SerializeField, Tooltip("�d��f")]
    int Recovery;
    [SerializeField, Tooltip("���߂ɕK�v�ȃt���[��")]
    int maxCharge;
    int Count;
    int chargeCount;
    // Start is called before the first frame update
    public override void Spawned()
    {
        attackArea = gameObject.transform.Find("ChargeAttackArea").gameObject;
        attackArea.SetActive(false);
        netobj = GameObject.Find("Networkbox");
        if (netobj == null)
        {
            Debug.LogError("�l�b�g�I�u�W�F�N�g�Ȃ���");
        }
        sharenum = netobj.GetComponent<ShareNumbers>();
    }
    public override void FixedUpdateNetwork()
    {
        if (Object.HasStateAuthority && GetInput(out NetworkInputData data))
        {
            //AnimatorStateInfo landAnimStateInfo = GetComponent<Animator>().GetCurrentAnimatorStateInfo(0);
            //if (landAnimStateInfo.IsName("APlayerParry") ||//�p���B���͍U�����Ȃ�
            //    landAnimStateInfo.IsName("APlayerJumpUp") || landAnimStateInfo.IsName("APlayerJumpDown"))//�W�����v���͍U�����Ȃ�
            //{
            //    return;
            //}

            var released = data.Buttons.GetReleased(ButtonsPrevious);
            ButtonsPrevious = data.Buttons;

            // Attack�{�^���������ꂽ���A���A�j���[�V�������Đ����łȂ����`�F�b�N
            if (data.Buttons.IsSet(NetworkInputButtons.ChargeAttack))
            {
                Debug.Log("��������������������������������" + chargeCount);
                isCharge = true;
                chargeCount++;
                Count++;
            }
            else if (released.IsSet(NetworkInputButtons.ChargeAttack))
            {
                attackArea.SetActive(true);
                chargeCount = 0;
                isCharge = false;
            }
            //if(released.IsSet(chargeCount)&&chargeCount>maxCharge)
            //{
            //    Debug.Log("���ߍU��ű������");
            //}
        }
    }
}
