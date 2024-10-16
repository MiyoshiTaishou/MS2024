using Fusion;
using Fusion.Addons.Physics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class PlayerAttack : NetworkBehaviour
{
    Animator animator;
    AudioSource audioSource;
    GameObject attackArea;
    GameObject director;

    [Networked] private bool isAttack { get; set; }
    [Networked] private bool isOnce { get; set; }
    [Networked] private bool isPlayingAnimation { get; set; }
    [Networked] public NetworkButtons ButtonsPrevious { get; set; }

    [Header("攻撃SE"), SerializeField] private AudioClip attackSE;

    public override void Spawned()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();        
        attackArea = gameObject.transform.Find("AttackArea").gameObject;
        director = GameObject.Find("TLDirector");

        attackArea.SetActive(false);
    }

    public override void FixedUpdateNetwork()
    {
        if (Object.HasStateAuthority && GetInput(out NetworkInputData data))
        {
            var pressed = data.Buttons.GetPressed(ButtonsPrevious);
            ButtonsPrevious = data.Buttons;

            // Attackボタンが押されたか、かつアニメーションが再生中でないかチェック
            if (pressed.IsSet(NetworkInputButtons.Attack) && !isAttack)
            {
                isAttack = true; // 攻撃フラグを立てる
                isPlayingAnimation = true;
                isOnce = true;

                //全プレイヤーにSEを再生する
                RPC_SE();

                Debug.Log("攻撃を押した");
                //GetComponent<SpecialAttackTLPlay>().RPC_Director();
                director.GetComponent<PlayableDirector>().Play();
            }
        }
    }

    public override void Render()
    {
        //// 現在のアニメーションの状態を取得
        //AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        //// 攻撃フラグが立っている場合にアニメーションをトリガー
        //if (isOnce)
        //{
        //    animator.SetTrigger("Attack"); // アニメーションのトリガー
        //    isOnce = false; // フラグをリセット
        //}

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
    }
}
