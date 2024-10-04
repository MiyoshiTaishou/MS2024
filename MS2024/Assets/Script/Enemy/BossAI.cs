using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum BOSS_STATE {
    IDLE,
    MOVING,
    PRELIMINARY_ACTION,
    ATTACKING,
    DOWN,
}

[System.Serializable]
public struct CoolTime {
    [Tooltip("ターゲットが切り替わる間隔を決めます")]
    public float changeTargetInterval;
    [Tooltip("攻撃の最低クールダウンを決めます")]
    public float minCooldownAfterAttack;
    [Tooltip("ボスのダウン時間を決めます")]
    public float downTime;
}

struct PlayerData {
    public GameObject playerObj;
    public Vector3 direction;
    public float distance;
}

public class BossAI : MonoBehaviour
{
    [Header("ボス設定")]
    [Tooltip("体力を決めます")]
    [SerializeField]
    public float HP;
    [Tooltip("ダウン時間を決めます(1秒間隔)")]
    [SerializeField]
    public float downTime;

    [Header("移動設定")]
    [Tooltip("移動速度を決めます")]
    [SerializeField]
    private float moveSpeed;
    [Tooltip("ターゲットが切り替わる間隔を決めます(1秒間隔)")]
    [SerializeField]
    private float changeTargetInterval;

    [Header("攻撃設定")]
    [Tooltip("攻撃技を決めます")]
    [SerializeField]
    public SkillBase[] skills;
    [Tooltip("攻撃の最低クールダウンを決めます(1秒間隔)")]
    [SerializeField]
    private float minCooldownAfterAttack;

    private BOSS_STATE bossState = BOSS_STATE.IDLE;
    private List<GameObject> playerObjects = new List<GameObject>();
    private GameObject currentTarget;
    private SkillBase skillToUse;
    private float cooldownTimer = 0f;
    private CoolTime coolTime;
    private CoolTime nowTime;

    private void Start() {
        coolTime.changeTargetInterval = changeTargetInterval;
        coolTime.minCooldownAfterAttack = minCooldownAfterAttack;
        coolTime.downTime = downTime;
    }

    private void Update() {
        // nowTime.minCooldownAfterAttack -= Time.deltaTime;
        // if (nowTime.minCooldownAfterAttack <= 0) {
        //     bossState = BOSS_STATE.MOVING;
        // }
        UpdateCooldowns();
    }

    private void FixedUpdate() {
        // 一定時間ごとにターゲット変更
        if (playerObjects.Count <= 0) {
            PlayerSearch();
            ChangeTargetRoutine();
        }
        else 
            // Debug.LogWarning("プレイヤー発見" + playerObjects);
        if (nowTime.changeTargetInterval <= 0) {
            ChangeTargetRoutine();
            nowTime.changeTargetInterval = coolTime.changeTargetInterval;
        }

        switch (bossState) {
            case BOSS_STATE.IDLE:
                bossState = BOSS_STATE.MOVING;
                break;

            case BOSS_STATE.MOVING:
                MoveTowardsTarget();
                TryStartAttack();
                break;

            case BOSS_STATE.PRELIMINARY_ACTION:
                break;

            case BOSS_STATE.ATTACKING:
                // 攻撃中の処理はスキル側で行う
                CheckAttacking();
                Debug.DrawLine(transform.position, currentTarget.transform.position, Color.red);
                break;
            
            case BOSS_STATE.DOWN:
                if (nowTime.downTime <= 0)
                    bossState = BOSS_STATE.MOVING;
                break;
        }
    }

    private void MoveTowardsTarget() {
        if (currentTarget == null) {
            PlayerSearch();
            return;
        }

        Vector3 direction = (currentTarget.transform.position - transform.position).normalized;
        transform.position += direction * moveSpeed * Time.deltaTime;
        for (int i = 0; i < playerObjects.Count; i++) {
            Debug.DrawLine(transform.position, playerObjects[i].transform.position, Color.red);
        }
    }

    private void TryStartAttack() {
        if (playerObjects == null || currentTarget == null) return;
        skillToUse = GetAvailableSkill();
        if (skillToUse != null) {
            bossState = BOSS_STATE.ATTACKING;
            skillToUse.UseSkill(transform, currentTarget.transform);        
        }
    }

    private SkillBase GetAvailableSkill() { // 多分複数スキル使用可能時、ランダム選択されたスキルが射程外の場合スキルの再選択を行わず攻撃をしないという不具合起こす
        List<SkillBase> availableSkills = new List<SkillBase>();
        foreach (var a in skills)  {
            foreach (var skill in skills) {
                if (skill.CurrentCooldown <= 0) {
                    availableSkills.Add(skill);
                }
            }
            // Debug.LogWarning("使用可能スキル"+availableSkills.Count+"個");

            if (availableSkills.Count == 0) return null;
            // クールダウンが0のスキルが複数ある場合はランダムで選択
            int index = Random.Range(0, availableSkills.Count);
            if (IsTargetWithSkillRange(index))
                return availableSkills[index];
        }
        return null;
    }

    private bool IsTargetWithSkillRange(int skillNum) {
        float direction = (transform.position - currentTarget.transform.position).magnitude;
        // Debug.DrawLine(transform.position, currentTarget.transform.position, Color.green);
        // Debug.LogWarning("プレイヤーとの距離："+direction);
        // Debug.LogWarning("最低射程距離："+skills[skillNum].minAttackRange);
        // Debug.LogWarning("最大射程距離："+skills[skillNum].maxAttackRange);
        if (skills[skillNum].minAttackRange <= direction && skills[skillNum].maxAttackRange >= direction)
            return true;
        return false;
    }

    private void CheckAttacking() {
        if (skillToUse.IsSkillUsing()) return;
        // クールダウン開始
        skillToUse.ResetCooldown();
        nowTime.minCooldownAfterAttack = coolTime.minCooldownAfterAttack;
        bossState = BOSS_STATE.MOVING;
    }

    public void BossDown() {}

    private void UpdateCooldowns() {
        float deltaTime = Time.deltaTime;
        foreach (var skill in skills) {
            skill.UpdateCooldown(deltaTime);
        }
        nowTime.changeTargetInterval   -= deltaTime;
        nowTime.minCooldownAfterAttack -= deltaTime;
        nowTime.downTime               -= deltaTime;
    }

    private void PlayerSearch() {
        // Debug.LogWarning("プレイヤーサーチ");
        playerObjects.Clear();
        GameObject[] allObjects = GameObject.FindGameObjectsWithTag("Player");
        playerObjects.AddRange(allObjects);
    }

    private void ChangeTargetRoutine() {
        PlayerSearch();
        if (playerObjects.Count > 0) {
            int index = Random.Range(0, playerObjects.Count);
            currentTarget = playerObjects[index];
        }
    }
}
