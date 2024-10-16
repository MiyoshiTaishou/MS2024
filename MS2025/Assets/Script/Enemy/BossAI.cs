using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

enum BOSS_STATE {
	IDLE,
	ROTATION,
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

public class BossAI : NetworkBehaviour {

	[Header("ボス設定\n松木君作成のスクリプトに統合予定")]
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
	[Tooltip("振向き速度を決めます")]
	[SerializeField]
	private float rotationSpeed;
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
		UpdateCooldowns();
	}

	private void FixedUpdate() {
		// 一定時間ごとにターゲット変更
		if (playerObjects.Count <= 0) {
			PlayerSearch();
			ChangeTargetRoutine();
		}
		else 
		if (nowTime.changeTargetInterval <= 0) {
			ChangeTargetRoutine();
			nowTime.changeTargetInterval = coolTime.changeTargetInterval;
		}

		switch (bossState) {
			case BOSS_STATE.IDLE:
				bossState = BOSS_STATE.ROTATION;
				break;

			case BOSS_STATE.ROTATION:
				RotisonTowardsTarget();
				break;

			case BOSS_STATE.MOVING:
				RotisonTowardsTarget();
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
					bossState = BOSS_STATE.ROTATION;
				break;
		}
	}

	private void RotisonTowardsTarget() {
		if (currentTarget == null) {
			PlayerSearch();
			return;
		}

		Vector3 targetPos = currentTarget.transform.position;
		Vector3 direction = targetPos - transform.position;
		direction.y = 0;

		if (direction != Vector3.zero) {
			// ターゲットに向かう回転を計算
			Quaternion targetRotation = Quaternion.LookRotation(direction);
			transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
		}

		// ある程度ターゲットの方向を向いた場合、ステートを切り替える
		Vector3 forward = transform.forward;
		forward.y = 0;
		direction.Normalize();

		float angleToTarget = Vector3.Angle(forward, direction);

		if (angleToTarget < 10f) {
			bossState = BOSS_STATE.MOVING;
		}
	}

	private void MoveTowardsTarget() {
		if (currentTarget == null) {
			PlayerSearch();
			return;
		}
		float distance = (transform.position - currentTarget.transform.position).magnitude;
		if (distance < 3) return;

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

	private SkillBase GetAvailableSkill() {
        List<SkillBase> availableSkills = new List<SkillBase>();

        // クールダウンが終了しているスキルをリストに追加
        foreach (var skill in skills) {
            if (skill.CurrentCooldown <= 0) {
                availableSkills.Add(skill);
            }
        }

        // 使用可能なスキルがない場合はnullを返す
        if (availableSkills.Count == 0) return null;

        // 射程内のスキルを見つけるまでループ
        while (availableSkills.Count > 0) {
            int index = Random.Range(0, availableSkills.Count);
            if (IsTargetWithSkillRange(index)) {
                return availableSkills[index];
            } else {
                // 射程外のスキルをリストから削除
                availableSkills.RemoveAt(index);
            }
        }

        // 射程内のスキルが見つからなかった場合はnullを返す
        return null;
    }

	private bool IsTargetWithSkillRange(int skillNum) {
		float distance = (transform.position - currentTarget.transform.position).magnitude;
		if (skills[skillNum].minAttackRange <= distance && skills[skillNum].maxAttackRange >= distance)
			return true;
		return false;
	}

	private void CheckAttacking() {
		if (skillToUse.IsSkillUsing()) return;
		// クールダウン開始
		skillToUse.ResetCooldown();
		nowTime.minCooldownAfterAttack = coolTime.minCooldownAfterAttack;
		bossState = BOSS_STATE.ROTATION;
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
		bossState = BOSS_STATE.ROTATION;
	}
}
