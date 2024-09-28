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
    [Tooltip("攻撃発生の長さを決めます")]
    public float totalTime;
    [Tooltip("攻撃のクールダウンを決めます")]
    public float coolDown;
    // [Tooltip("攻撃準備時間を決めます")]
    // public float prepareTime;
    // [Tooltip("攻撃の持続時間を決めます")]
    // public float duration;

    private float currentCooldown = 0f;

    public float CurrentCooldown => currentCooldown;

    public void UpdateCooldown(float deltaTime){
        if (currentCooldown > 0) {
            currentCooldown -= deltaTime;
        }
    }

    public void ResetCooldown() {
        currentCooldown = coolDown;
    }

    public abstract void UseSkill(Transform bossTransform, Transform targetTransform);
}
