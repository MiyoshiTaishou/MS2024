using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 画面遷移関係を管理する
/// </summary>
public class TransitionManager : MonoBehaviour
{
    [SerializeField, Header("トランジションオブジェクト")] private GameObject[] transitions;

    [SerializeField, Header("開始時にトランジションするか")] private bool isStart = true;
   
    // Start is called before the first frame update
    void Start() {
        if (isStart) {
            // 各トランジションオブジェクトに対してトリガーをセットし、逆再生の設定を行う
            for (int i = 0; i < transitions.Length; i++) {
                Animator animator = transitions[i].GetComponent<Animator>();
                // トリガーを設定してアニメーションを開始
                animator.SetTrigger("Reverse");
            }
        }
    }

    public void TransitionStart() {
        // 各トランジションオブジェクトに対してトリガーをセットし、逆再生の設定を行う
        for (int i = 0; i < transitions.Length; i++) {
            Animator animator = transitions[i].GetComponent<Animator>();
            // トリガーを設定してアニメーションを開始
            animator.SetTrigger("Start");
        }
    }

    public void TransitionStartReverse() {
        // 各トランジションオブジェクトに対してトリガーをセットし、逆再生の設定を行う
        for (int i = 0; i < transitions.Length; i++) {
            Animator animator = transitions[i].GetComponent<Animator>();
            // トリガーを設定してアニメーションを開始
            animator.SetTrigger("Reverse");
        }
    }
}
