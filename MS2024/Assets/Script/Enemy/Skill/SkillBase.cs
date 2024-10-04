using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SkillBase : MonoBehaviour
{
    [Header("攻撃スキル設定")]
    [Tooltip("攻撃エフェクトを決めます")]
    public GameObject skillObj;
    [Tooltip("最大同時攻撃数を決めます")]
    public int maxSkillCount;
    [Tooltip("予備動作の長さを決めます(1秒間隔)")]
    public float preliminaryTime;
    [Tooltip("攻撃のクールダウンを決めます(1秒間隔)")]
    public float coolDown;
    [Tooltip("攻撃の最低射程を決めます")]
    public float minAttackRange;
    [Tooltip("攻撃の最大射程を決めます")]
    public float maxAttackRange;
    // [Tooltip("攻撃準備時間を決めます")]
    // public float prepareTime;
    // [Tooltip("攻撃の持続時間を決めます")]
    // public float duration;
    protected int nowSkillCount;

    private float currentCooldown = 0f;
    public float CurrentCooldown => currentCooldown;

    public void Start() {
        nowSkillCount = maxSkillCount;
    }

    public void UpdateCooldown(float deltaTime){
        if (currentCooldown > 0) {
            // Debug.LogWarning("スキルクールタイム:"+currentCooldown);
            currentCooldown -= deltaTime;
        }
    }

    public void ResetCooldown() {
        currentCooldown = coolDown;
    }

    public abstract void UseSkill(Transform bossTransform, Transform targetTransform);
    public abstract bool IsSkillUsing();
}
