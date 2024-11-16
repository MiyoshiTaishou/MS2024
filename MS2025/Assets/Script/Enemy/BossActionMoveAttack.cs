using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 攻撃の判定が移動する攻撃
/// </summary>
[CreateAssetMenu(fileName = "MoveAttackAction", menuName = "Boss/Actions/MoveAttack")]
public class BossActionMoveAttack : BossActionData
{
    [SerializeField, Header("攻撃が移動する時間")]
    private float moveAttackEndPosTime = 3.0f;

    [SerializeField, Header("アニメーションカーブで移動をリッチにする")]
    private AnimationCurve curve;

    [SerializeField, Header("攻撃開始するまでの待機時間")]
    private float attackDuration;

    [SerializeField, Header("アニメーションの速度")]
    private float attackAnimSpeed;

    [SerializeField, Header("攻撃の当たり判定の大きさ")]
    private Vector3 attackScale;

    [SerializeField, Header("カメラを揺らす処理を適用するか")]
    private bool isCameraShake;

    [SerializeField, Header("カメラ揺れの時間")]
    private float cameraDuration;

    [SerializeField, Header("揺れの強さ")]
    private float magnitude;

    [SerializeField, Header("パリィ不可能かどうか")]
    private bool canParry;

    [SerializeField, Header("ノックバック可能かどうか")]
    private bool canKnockBack;

    [SerializeField, Header("攻撃エリアの名前")]
    public string attackAreaName;

    public AudioClip attackClip;

    private GameObject attackArea;
    private float attackStartTime;
    private float moveStartTime;
    private Transform attackTarget;
    private Vector3 moveAttackEndPos;
    private Vector3 originalPosition;
    private bool isMoving;

    public override void InitializeAction(GameObject boss, Transform player)
    {
        attackTarget = player;
        attackStartTime = Time.time;
        moveAttackEndPos = player.transform.position;

        // 攻撃エリアの設定
        attackArea = boss.transform.Find(attackAreaName)?.gameObject;
        originalPosition = attackArea.transform.position;
        attackArea.transform.localScale = attackScale;
        attackArea.SetActive(false);
        isMoving = false;

        // ボスのアニメーション設定
        boss.GetComponent<Animator>().speed = attackAnimSpeed;
        boss.GetComponent<BossAI>().isKnockBack = canKnockBack;
        boss.GetComponent<BossAI>().isParry = canParry;
    }

    public override bool ExecuteAction(GameObject boss)
    {
        // 攻撃開始までの待機
        if (Time.time - attackStartTime < attackDuration)
        {
            return false;
        }

        // 移動開始時の初期化
        if (!isMoving)
        {
            isMoving = true;
            moveStartTime = Time.time;
            attackArea.SetActive(true);

            // 音を再生
            if (boss.GetComponent<AudioSource>() != null && attackClip != null)
            {
                boss.GetComponent<AudioSource>().clip = attackClip;
                boss.GetComponent<AudioSource>().Play();
            }
        }

        // 移動処理
        float elapsed = Time.time - moveStartTime;
        float progress = elapsed / moveAttackEndPosTime;
        float curveValue = curve.Evaluate(progress);

        attackArea.transform.position = Vector3.Lerp(originalPosition, moveAttackEndPos, curveValue);

        // プレイヤーへの攻撃がヒットしたか、移動が完了した場合
        if (progress >= 1.0f || CheckForHit(attackArea))
        {
            ResetAttackArea();
            return true; // アクション完了
        }

        return false; // アクション継続中
    }

    // 攻撃エリアを元の位置に戻して非アクティブ化
    private void ResetAttackArea()
    {
        attackArea.transform.position = originalPosition;
        attackArea.SetActive(false);
        isMoving = false;
    }

    // 攻撃ヒット判定の例（実際のヒット判定処理に応じて変更すること）
    private bool CheckForHit(GameObject attackArea)
    {
        // プレイヤーが攻撃エリア内にいるかどうかの判定
        Collider[] hits = Physics.OverlapBox(attackArea.transform.position, attackArea.transform.localScale / 2);
        foreach (var hit in hits)
        {
            if (hit.CompareTag("Player"))
            {
                Debug.Log("攻撃がヒットしました！");
                return true;
            }
        }
        return false;
    }
}
