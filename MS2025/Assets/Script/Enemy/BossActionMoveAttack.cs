using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 攻撃の判定が移動する攻撃
/// </summary>
[CreateAssetMenu(fileName = "MoveAttackAction", menuName = "Boss/Actions/MoveAttack")]
public class BossActionMoveAttack : BossActionData
{
    [SerializeField, Header("どちらをターゲットにするか0が1P,1が2P")]
    private int taregt = 0;

    [SerializeField, Header("攻撃が移動する時間")]
    private float moveAttackEndPosTime = 3.0f;

    [SerializeField, Header("どれぐらい離れてたら攻撃するか")]
    private float distance = 5.0f;

    [SerializeField, Header("攻撃がどれだけずれるか")]
    private Vector3 deviate;

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
    private GameObject attackAreaImage;
    private float attackStartTime;
    private float moveStartTime;
    private Transform attackTarget;
    private Vector3 moveAttackEndPos;
    private Vector3 originalPosition;
    private bool isMoving;

    private bool isAttack = false;
    private bool isComp = false;

    [SerializeField, Header("攻撃エリアに連動する画像オブジェクト")]
    private string linkedImage; // 動かしたい画像オブジェクト

    private Vector3 linkedImageOriginalPosition; // 画像の元の位置

    public override void InitializeAction(GameObject boss, Transform player)
    {
        // (既存の処理)
        attackTarget = boss.GetComponent<BossAI>().players[taregt];
        attackStartTime = Time.time;
        moveAttackEndPos = attackTarget.transform.position + deviate;

        // 攻撃エリアの設定
        attackArea = boss.transform.Find(attackAreaName)?.gameObject;
        attackAreaImage = boss.transform.Find(linkedImage)?.gameObject;
        originalPosition = attackArea.transform.localPosition; // ローカル座標に変更
        attackArea.transform.localScale = attackScale;
        attackArea.SetActive(true);

        // 画像オブジェクトの元の位置を記録
        if (linkedImage != null)
        {
            linkedImageOriginalPosition = attackAreaImage.transform.position; // ローカル座標に変更
        }

        isMoving = false;

        // 距離判定 (既存の処理)
        float dis = Vector3.Distance(moveAttackEndPos, boss.transform.position);
        isAttack = (distance > dis);

        if (isAttack)
        {
            attackArea.GetComponent<BossAttackArea>().deactivateTime = 0.5f;
        }
        else
        {
            attackArea.GetComponent<BossAttackArea>().deactivateTime = moveAttackEndPosTime;
        }

        // ボスのアニメーション設定
        boss.GetComponent<Animator>().speed = attackAnimSpeed;
        boss.GetComponent<BossAI>().isKnockBack = canKnockBack;
        boss.GetComponent<BossAI>().isParry = canParry;

        isComp = false;

        // 最後に確実に位置を元に戻す
        attackArea.transform.position = Vector3.zero; // ローカル座標に変更
        attackArea.SetActive(false);

        if (linkedImage != null)
        {
            attackAreaImage.transform.localPosition = Vector3.zero; // ローカル座標に変更
        }
    }

    public override bool ExecuteAction(GameObject boss, Transform player)
    {
        if (Time.time - attackStartTime < attackDuration)
        {
            return false; // 攻撃待機中
        }

        if (isComp)
        {
            return true;
        }

        if (isAttack)
        {
            attackArea.SetActive(true);

            if (boss.GetComponent<AudioSource>() != null && attackClip != null)
            {
                boss.GetComponent<AudioSource>().clip = attackClip;
                boss.GetComponent<AudioSource>().Play();
            }

            boss.GetComponent<Animator>().speed = 1;
            return true;
        }
        else
        {
            if (!isMoving)
            {
                isMoving = true;
                moveStartTime = Time.time;
                attackArea.SetActive(true);

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

            // 攻撃エリアと画像オブジェクトの移動 (ワールド座標で計算)
            Vector3 targetPosition = Vector3.Lerp(originalPosition, moveAttackEndPos, curveValue); // ワールド座標で計算
            attackArea.transform.position = targetPosition; // ワールド座標に変換して設定

            if (linkedImage != null)
            {
                Vector3 targetImagePosition = Vector3.Lerp(linkedImageOriginalPosition, moveAttackEndPos, curveValue); // ワールド座標で計算
                attackAreaImage.transform.position = targetImagePosition; // ワールド座標に変換して設定
            }

            // 元のローカル座標に戻す
            //attackArea.transform.localPosition = attackArea.transform.parent.InverseTransformPoint(attackArea.transform.position);
            if (linkedImage != null)
            {
                attackAreaImage.transform.localPosition = attackAreaImage.transform.parent.InverseTransformPoint(attackAreaImage.transform.position);
            }

            if (progress >= 1.0f)
            {
                boss.GetComponent<MonoBehaviour>().StartCoroutine(ResetToOriginalPosition());
                return false; // リセットが完了するまではfalseを返す
            }

            return false;
        }
    }

    private IEnumerator ResetToOriginalPosition()
    {
        float resetStartTime = Time.time;
        float resetDuration = 0.5f; // 元の位置に戻るまでの時間

        Vector3 attackAreaStartPosition = attackArea.transform.position; // ローカル座標に変更
        Vector3 linkedImageStartPosition = linkedImage != null ? attackAreaImage.transform.localPosition : Vector3.zero; // ローカル座標に変更

        while (Time.time - resetStartTime < resetDuration)
        {
            float progress = (Time.time - resetStartTime) / resetDuration;

            // 攻撃エリアをラープで元の位置に戻す
            attackArea.transform.position = Vector3.Lerp(attackAreaStartPosition, originalPosition, progress); // ローカル座標に変更

            // 画像オブジェクトもラープで元の位置に戻す
            if (linkedImage != null)
            {
                attackAreaImage.transform.localPosition = Vector3.Lerp(linkedImageStartPosition, linkedImageOriginalPosition, progress); // ローカル座標に変更
            }

            yield return null;
        }

        // 最後に確実に位置を元に戻す
        attackArea.transform.position = Vector3.zero; // ローカル座標に変更
        attackArea.SetActive(false);

        Debug.Log("完全にオワタ" + attackArea.transform.localPosition); // ローカル座標に変更

        if (linkedImage != null)
        {
            attackAreaImage.transform.localPosition = Vector3.zero; // ローカル座標に変更
        }

        isMoving = false;
        isComp = true;

        yield return true; // 完了を通知
    }
}
