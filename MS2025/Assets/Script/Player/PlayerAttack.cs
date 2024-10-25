using Fusion;
using Fusion.Addons.Physics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : NetworkBehaviour
{
    Animator animator;
    AudioSource audioSource;
    GameObject attackArea;
    [SerializeField]
    GameObject netobj;
    [Networked] public int currentCombo { get; set; }

    ShareNumbers sharenum;

    [Networked] private bool isAttack { get; set; }
    [Networked] private bool isOnce { get; set; }
    [Networked] private bool isPlayingAnimation { get; set; }
    [Networked] public NetworkButtons ButtonsPrevious { get; set; }

    [Header("攻撃SE"), SerializeField] private AudioClip attackSE;

    [SerializeField, Tooltip("発生f")]
    int Startup;
    [SerializeField, Tooltip("持続f")]
    int Active;
    [SerializeField, Tooltip("硬直f")]
    int Recovery;

    int Count;


    public override void Spawned()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();        
        attackArea = gameObject.transform.Find("AttackArea").gameObject;
        attackArea.SetActive(false);
        netobj = GameObject.Find("Networkbox");
        if(netobj==null) 
        {
            Debug.LogError("ネットオブジェクトないよ");
        }
        sharenum = netobj.GetComponent<ShareNumbers>();
    }

    public override void FixedUpdateNetwork()
    {
        if (Object.HasStateAuthority && GetInput(out NetworkInputData data))
        {
            var pressed = data.Buttons.GetPressed(ButtonsPrevious);
            ButtonsPrevious = data.Buttons;

            // Attackボタンが押されたか、かつアニメーションが再生中でないかチェック
            if (pressed.IsSet(NetworkInputButtons.Attack) && !isAttack && currentCombo<2)
            {
                isAttack = true; // 攻撃フラグを立てる
                isPlayingAnimation = true;
                isOnce = true;
                //全プレイヤーにSEを再生する
                RPC_SE();
            }
            else if(pressed.IsSet(NetworkInputButtons.Attack) && currentCombo >= 2)
            {
                isAttack = true; // 攻撃フラグを立てる
                isPlayingAnimation = true;
                isOnce = true;
                //全プレイヤーにSEを再生する
                RPC_SE();
            }
        }
        Attack();
    }

    public override void Render()
    {
        // 現在のアニメーションの状態を取得
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        // 攻撃フラグが立っている場合にアニメーションをトリガー
        if (isOnce&& currentCombo==0)
        {
            //Debug.LogError("壱の秘剣");
            //animator.SetTrigger("Attack"); // アニメーションのトリガー
            animator.Play("APlayerAttack");
            isOnce = false; // フラグをリセット
        }
        else if (isOnce&& currentCombo==1)
        {
            //Debug.LogError("弐の秘剣");
            //animator.SetTrigger("Attack2"); // アニメーションのトリガー
            animator.Play("APlayerAttack2");
            isOnce = false; // フラグをリセット
        }
        else if (isOnce&& currentCombo>=2)
        {
            //Debug.LogError("終の秘剣");
            //animator.SetTrigger("Attack3"); // アニメーションのトリガー
            animator.Play("APlayerAttack3");
            isOnce = false; // フラグをリセット
        }


        //// アニメーションが再生中である場合の処理
        //if (stateInfo.IsName("APlayerAttack"))
        //{
        //    attackArea.SetActive(true); // 攻撃エリアをアクティブに       
        //}
        //else
        //{
        //    attackArea.SetActive(false); // アニメーションが再生中でない場合は攻撃エリアを無効化
        //    isAttack = false;
        //}
    }

    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void RPC_SE()
    {       
        audioSource.PlayOneShot(attackSE);
        isAttack = true; // 攻撃フラグを立てる
        isPlayingAnimation = true;
        isOnce = true;
        currentCombo = sharenum.nHitnum;
    }

    void Attack()
    {
        if(isAttack==false)
        {
            return;
        }

        if (Count < Startup) 
        {
            Count++;
        }
        else if(Count < Startup+Active)
        {
            Count++;
            attackArea.SetActive(true);
        }
        else if(Count < Startup+Active+Recovery)
        {
            Count++;
            attackArea.SetActive(false);
        }
        else if(Count >= Startup + Active + Recovery)
        {
            Count = 0;
            isAttack = false;
        }
    }
}
