using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum BOSS_STATE {
    IDLE,
    MOVING,
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
    private float cooldownTimer = 0f;
    private CoolTime coolTime;
    private CoolTime nowTime;

    private void Start() {
        coolTime.changeTargetInterval = changeTargetInterval * 60;
        coolTime.minCooldownAfterAttack = minCooldownAfterAttack * 60;
        coolTime.downTime = downTime * 60;
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
        if (playerObjects.Count >= 0) {
            PlayerSearch();
            ChangeTargetRoutine();
        }
        else 
            Debug.LogWarning("プレイヤー発見" + playerObjects);
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

            case BOSS_STATE.ATTACKING:
                // 攻撃中の処理はスキル側で行う
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

    private bool IsTargetWithSkillRange(int skillNum) {
        float direction = (transform.position + currentTarget.transform.position).magnitude;
        if (skills[skillNum].minAttackRange >= direction && skills[skillNum].maxAttackRange <= direction)
            return true;
        return false;
    }

    private void TryStartAttack() {
        SkillBase skillToUse = GetAvailableSkill();
        if (skillToUse != null) {
            bossState = BOSS_STATE.ATTACKING;
            AttackRoutine(skillToUse);
        }
    }

    private SkillBase GetAvailableSkill() {
        List<SkillBase> availableSkills = new List<SkillBase>();
        foreach (var a in skills)  {
            foreach (var skill in skills) {
                if (skill.CurrentCooldown <= 0) {
                    availableSkills.Add(skill);
                }
            }

            if (availableSkills.Count == 0) return null;
            // クールダウンが0のスキルが複数ある場合はランダムで選択
            int index = Random.Range(0, availableSkills.Count);
            if (IsTargetWithSkillRange(index))
                return availableSkills[index];
        }
        return null;
    }

    private void AttackRoutine(SkillBase skill) {
        // 攻撃準備時間がある場合はここで待機 
        // 攻撃時間がある場合はここで待機
        // if (skill.coolDown <= 0) return;
        // if (skill.duration <= 0) return;

        // bossState = BOSS_STATE.ATTACKING;
        skill.UseSkill(transform, currentTarget.transform);


        // クールダウン開始
        skill.ResetCooldown();
        nowTime.minCooldownAfterAttack = coolTime.minCooldownAfterAttack;
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
        Debug.LogWarning("プレイヤーサーチ");
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


/*
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum BOSS_TURN {
    MOVE,
    ATTACK,
}

struct PlayerDate {
    public GameObject playerObj;
    public Vector3 direction;
    public float distance;
}

public class BossAI : MonoBehaviour
{
    [Header("移動設定")]
    [Tooltip("移動速度を決めます")]
    [SerializeField]
    private float moveSpeed;
    [Tooltip("ターゲットが切り替わる決めます")]
    [SerializeField]
    private float changeTarget;
    [Header("攻撃設定")]
    [Tooltip("攻撃技を決めます")]
    [SerializeField]
    public SkillBase[] skill;
    [SerializeField]
    // private GameObject[] attackSkill;
    [Tooltip("攻撃の最低クールダウンを決めます")]
    public int minCoolDown;
    private BOSS_TURN bossTurn;
    private List<GameObject> playerObjects = new List<GameObject>();
    private List<PlayerDate> playerData = new List<PlayerDate>();
    private int targetPlayer = 0;
    private string targetTag = "Player";
    private float direction = 5.0f;
    
    private void Update(){
        if (Input.GetKeyDown(KeyCode.Space))
            PlayerSearch();
    }
    
    private void FixedUpdate() {
        for (int i = 0; i < playerObjects.Count; i++) {
            Vector3 pointA = transform.position; // 自分の位置
            Vector3 pointB = playerObjects[i].transform.position; // ターゲットの位置
            
            PlayerDate playerInfo = new PlayerDate {
                playerObj = playerObjects[i],
                direction = new Vector3 { x = pointB.x - pointA.x, y = pointA.y, z = pointB.z - pointA.z},
                distance = (pointB - pointA).magnitude
            };
            playerData.Add(playerInfo); // プレイヤー情報を追加

            // デバッグ用の線を描画
            Debug.DrawLine(pointA, pointB, Color.red);
            Debug.Log("Distance: " + playerInfo.distance);
        }

        switch (bossTurn) {
            case BOSS_TURN.MOVE:
                for (int i = 0; i < playerData.Count; i++) {
                    if (playerData[i].distance < 5) {
                        targetPlayer = i;
                        bossTurn = BOSS_TURN.ATTACK;
                    }
                }
                break;
            case BOSS_TURN.ATTACK:
                for (int i = 0; i < skill.Length; i++) {
                    Vector3 position = transform.position + playerData[targetPlayer].direction.normalized * i; // 正規化
                    Instantiate(skill[0].skillObj, position, Quaternion.identity);
                }
                bossTurn = BOSS_TURN.MOVE;
                break;
            default:
                bossTurn = BOSS_TURN.MOVE;
                break;
        }
        playerData.Clear(); // 次のフレームのためにクリア
    }

    private void PlayerSearch() {
        GameObject[] allObjects = GameObject.FindGameObjectsWithTag(targetTag);
        playerObjects.Clear();
        playerData.Clear(); // プレイヤーデータもクリア
        
        for (int i = 0; i < allObjects.Length; i++) {
            playerObjects.Add(allObjects[i]);
        }
    }
}

*/