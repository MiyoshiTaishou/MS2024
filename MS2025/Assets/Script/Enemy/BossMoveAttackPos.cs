using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 攻撃の判定が移動する攻撃
/// </summary>
[CreateAssetMenu(fileName = "MoveAttackPos", menuName = "Boss/Actions/MoveAttackPos")]
public class BossMoveAttackPos : BossActionData
{  
    [SerializeField, Header("移動する時間")]
    private float moveAttackEndPosTime = 3.0f;

    [SerializeField, Header("到着時間")]
    private Vector3 EndPosition = Vector3.zero;

    [SerializeField, Header("拳向き")]
    private float rotPunch;

    [SerializeField, Header("アニメーションカーブで移動をリッチにする")]
    private AnimationCurve curve; 

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

    [SerializeField, Header("攻撃エリアの名前")]
    public string attackAreaName;

    [SerializeField, Header("パリィ不可能かどうか")]
    private bool canParry;

    [SerializeField, Header("ノックバック可能かどうか")]
    private bool canKnockBack;

    [SerializeField, Header("移動不可にする")]
    private bool canMove;

    [SerializeField, Header("攻撃の判定")]
    private PARRYTYPE parryType;

    public AudioClip attackClip;

    private GameObject attackArea;
    private float attackStartTime;
    private float moveStartTime;    
    private bool isMoving;
    private Vector3 originalPosition;

    private bool isAttack = false;
    private bool isComp = false;

    private Vector3 linkedImageOriginalPosition; // 画像の元の位置

    private IEnumerator resetCoroutine; // リセット処理用コルーチン

    private GameObject attackAreaView; // 既存の攻撃エリアの参照

    public override void InitializeAction(GameObject boss, Transform player)
    {        
        attackStartTime = Time.time;       

        attackArea = GameObject.Find(attackAreaName)?.gameObject;
        originalPosition = attackArea.transform.position;
        attackArea.GetComponent<BoxCollider>().size = attackScale;
        attackArea.SetActive(true);

        attackArea.transform.rotation = Quaternion.identity;             
        attackArea.transform.rotation = Quaternion.Euler(0, 0, rotPunch);

        //attackArea.transform.localRotation = rotPunch2;

        isMoving = false;

        attackArea.GetComponent<BossAttackArea2Boss>().Type = parryType;

        // ボスのアニメーション設定
        boss.GetComponent<Animator>().speed = attackAnimSpeed;
        boss.GetComponent<BossAI>().isKnockBack = canKnockBack;
        boss.GetComponent<BossAI>().isParry = canParry;

        attackAreaView = boss.transform.Find("Area")?.gameObject;

        isComp = false;

        attackArea.GetComponent<BoxCollider>().enabled = true;

        attackArea.GetComponent<MoveToBossObject>().RPC_SetToMove(false);

        resetCoroutine = null;

        Camera.main.GetComponent<CameraShake>().RPC_CameraShake(cameraDuration, magnitude);
        attackAreaView.transform.position = new Vector3(EndPosition.x, 2f, EndPosition.z);
        attackAreaView.GetComponent<PulsatingCircle>().RPC_Scale(attackScale.z);
        attackAreaView.GetComponent<PulsatingCircle>().RPC_Spedd(attackAnimSpeed);
        attackAreaView.GetComponent<PulsatingCircle>().RPC_Active(true);
    }

    public override bool ExecuteAction(GameObject boss, Transform player)
    {       
        if (isComp)
        {
            return true; // アクション完了
        }
      
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

            Vector3 targetPosition = Vector3.Lerp(originalPosition, EndPosition, curveValue);
            attackArea.transform.position = targetPosition;

            if (progress >= 1.0f && resetCoroutine == null && canMove)
            {
                // リセット処理開始
                //resetCoroutine = ResetToOriginalPosition();
                //boss.GetComponent<MonoBehaviour>().StartCoroutine(resetCoroutine);
                attackArea.GetComponent<MoveToBossObject>().RPC_SetToMove(true);
                attackAreaView.GetComponent<PulsatingCircle>().RPC_Active(false);
                return true;
            }
            else if(progress >= 1.0f && resetCoroutine == null && !canMove)
            {
                return true;
            }

            // リセット処理が完了するのを待つ
            if (resetCoroutine != null && !isMoving)
            {
                resetCoroutine = null; // 完了後にコルーチンをリセット
                return true; // 完了した場合
            }

            return false; // リセットが完了していない場合        
    }

    private IEnumerator ResetToOriginalPosition()
    {
        float resetStartTime = Time.time;
        float resetDuration = 0.5f;

        Vector3 attackAreaStartPosition = attackArea.transform.position;

        while (Time.time - resetStartTime < resetDuration)
        {
            float progress = (Time.time - resetStartTime) / resetDuration;
            attackArea.transform.position = Vector3.Lerp(attackAreaStartPosition, originalPosition, progress);
            yield return null;
        }

        attackArea.transform.position = originalPosition;
        attackArea.GetComponent<BoxCollider>().enabled = false;

        attackArea.GetComponent<MoveToBossObject>().RPC_SetToMove(true);
        attackAreaView.GetComponent<PulsatingCircle>().RPC_Active(false);

        isMoving = false;
        isComp = true;
    }

}
