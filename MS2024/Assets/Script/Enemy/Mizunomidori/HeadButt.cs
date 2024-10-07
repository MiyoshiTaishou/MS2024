using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadButt : SkillBase
{
    private Animator animator;
    // Animator Controllerで設定したハッシュコードを事前に取得
    private int attackTriggerHash = Animator.StringToHash("AttackTrigger");
    private int isAfterAttackHash = Animator.StringToHash("IsAfterAttack");
    private float animationSpeed;

    public void Start() {
        // Animatorコンポーネントを取得
        animator = GetComponentInParent<Animator>();
    }

    public void Update() {
        nowPreliminaryTime += Time.deltaTime;
    }

    public void FixedUpdate() {
        // animationSpeed = Easing.Easing.InExp(animationSpeed, preliminaryTime,1 ,5);
        // animator.speed = animationSpeed;
        // animationSpeed += Time.deltaTime;

        var stateInfo = animator.GetCurrentAnimatorStateInfo(0); // 0はベースレイヤー
        if (stateInfo.shortNameHash == attackTriggerHash && preliminaryTime >= nowPreliminaryTime) {
            // 後隙アニメーションへ遷移するためにパラメータを設定
            animator.SetBool(isAfterAttackHash, true);
        }
        if (stateInfo.shortNameHash == isAfterAttackHash && stateInfo.normalizedTime >= 1.0f) {
            animator.SetBool(attackTriggerHash, false);
            animator.SetBool(isAfterAttackHash, false);
        }
    }
    
    // アニメーションイベントから呼び出されるメソッド: 攻撃アニメーション完了
    public void OnAttackAnimationComplete() {
        if (animator != null) {
            animator.SetBool(isAfterAttackHash, true);
            Debug.Log("攻撃アニメーションが完了し、IsAfterAttackがtrueに設定されました。");
        }
    }

    // アニメーションイベントから呼び出されるメソッド: 後隙アニメーション完了
    public void OnAfterAttackAnimationComplete() {
        if (animator != null) {
            animator.SetBool(isAfterAttackHash, false);
            Debug.Log("後隙アニメーションが完了し、IsAfterAttackがfalseに設定されました。");
        }
    }

    public override void UseSkill(Transform BTF, Transform TTF){
        if (animator != null) {
            animator.SetBool(attackTriggerHash, true);
            animator.SetBool(isAfterAttackHash, false);
            nowPreliminaryTime = 0;
        }
    }

    public override bool IsSkillUsing() {
        var stateInfo = animator.GetCurrentAnimatorStateInfo(0); // 0はベースレイヤー
        if (stateInfo.shortNameHash == attackTriggerHash  && stateInfo.shortNameHash == isAfterAttackHash) return false;
        return true;
    }
}
