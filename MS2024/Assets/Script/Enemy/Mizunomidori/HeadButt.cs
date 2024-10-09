using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadButt : SkillBase
{
    private Animator animator;
    // Animator Controllerで設定したハッシュコードを事前に取得
    private int attackTriggerHash = Animator.StringToHash("AttackTrigger");
    private int isAfterAttackHash = Animator.StringToHash("IsAfterAttack");
    private int attackStateHash = Animator.StringToHash("Attack"); // アニメーションステート名
    private int leisureStateHash = Animator.StringToHash("Leisure"); // 後隙アニメーションステート名
    private float animationSpeed;

    // トリガー発火を追跡するフラグ
    private bool isAttackTriggered = false;

    public void Start() {
        // Animatorコンポーネントを取得
        animator = GetComponentInParent<Animator>();
        if (animator == null) {
            Debug.LogError("アニメーションが取得できませんでした。");
        }
    }

    public void Update() {
        nowPreliminaryTime += Time.deltaTime;
        
    }

    public void FixedUpdate() {
        if (animator == null) return;

        var stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        // 後隙アニメーション
        if (stateInfo.shortNameHash == attackStateHash && nowPreliminaryTime >= preliminaryTime) {
            animator.SetBool(isAfterAttackHash, true);
        }

        // 後隙アニメーション終了
        if (stateInfo.shortNameHash == leisureStateHash && stateInfo.normalizedTime >= 1.0f) {
            animator.SetBool(isAfterAttackHash, false);
            var DA = GetComponent<DamagedArea>();
            if(DA == null) return;
            DA.SetDelayActive(true, preliminaryTime);
        }
    }

    public override void UseSkill(Transform BTF, Transform TTF){
        if (animator != null) {
            var stateInfo = animator.GetCurrentAnimatorStateInfo(0);
            if (stateInfo.shortNameHash == attackStateHash) return;
            var DA = GetComponent<DamagedArea>();
            if(DA == null) return;
            DA.SetDelayActive(true, preliminaryTime);
            animator.SetTrigger(attackTriggerHash); // Triggerを使用
            animator.SetBool(isAfterAttackHash, false);
            nowPreliminaryTime = 0;
        }
    }

    public override bool IsSkillUsing() {
        if (animator == null) return false;

        // 現在のステートを取得
        var stateInfo = animator.GetCurrentAnimatorStateInfo(0); // 0はベースレイヤー

        // 攻撃中または後隙中の場合
        bool isInAttackOrAfterAttack = (stateInfo.shortNameHash == attackStateHash || stateInfo.shortNameHash == leisureStateHash);

        // トリガーが発火されたが、まだAttackステートに入っていない場合
        bool isTriggerActive = isAttackTriggered && !(stateInfo.shortNameHash == attackStateHash || stateInfo.shortNameHash == leisureStateHash);

        return isInAttackOrAfterAttack || isTriggerActive;
    }
}
