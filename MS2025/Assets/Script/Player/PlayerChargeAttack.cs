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

    bool isCharge=false;//チャージ中か否か
    [SerializeField, Tooltip("発生f")]
    int Startup;
    [SerializeField, Tooltip("持続f")]
    int Active;
    [SerializeField, Tooltip("硬直f")]
    int Recovery;
    [SerializeField, Tooltip("溜めに必要なフレーム")]
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
            Debug.LogError("ネットオブジェクトないよ");
        }
        sharenum = netobj.GetComponent<ShareNumbers>();
    }
    public override void FixedUpdateNetwork()
    {
        if (Object.HasStateAuthority && GetInput(out NetworkInputData data))
        {
            //AnimatorStateInfo landAnimStateInfo = GetComponent<Animator>().GetCurrentAnimatorStateInfo(0);
            //if (landAnimStateInfo.IsName("APlayerParry") ||//パリィ時は攻撃しない
            //    landAnimStateInfo.IsName("APlayerJumpUp") || landAnimStateInfo.IsName("APlayerJumpDown"))//ジャンプ中は攻撃しない
            //{
            //    return;
            //}

            var released = data.Buttons.GetReleased(ButtonsPrevious);
            ButtonsPrevious = data.Buttons;

            // Attackボタンが押されたか、かつアニメーションが再生中でないかチェック
            if (data.Buttons.IsSet(NetworkInputButtons.ChargeAttack))
            {
                Debug.Log("ああああああああああああああああ" + chargeCount);
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
            //    Debug.Log("溜め攻撃ﾅｱｱｱｱｱｱﾝ");
            //}
        }
    }
}
