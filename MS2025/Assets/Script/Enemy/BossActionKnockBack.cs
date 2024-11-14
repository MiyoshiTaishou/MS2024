using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "KnockBackAction", menuName = "Boss/Actions/KnockBack")]
public class BossActionKnockBack : BossActionData
{
    [SerializeField, Header("ノックバックする強さ")]
    private float KnockBackPower = 1.0f;

    /// <summary>
    /// プレイヤーからボスへのベクトル
    /// </summary>
    private Vector3 PBVec;

    [SerializeField,Header("ノックバック時間（これが隙のある時間になる）")]
    private float waitDuration;

    private float waitStartTime;

    bool isknock = false;


    public override void InitializeAction(GameObject boss, Transform player)
    {
        PBVec = player.transform.position - boss.transform.position;

        waitStartTime = Time.time;
        boss.GetComponent<Rigidbody>().useGravity = true;

        PBVec.y = 0;

        isknock = false;
    }

    public override bool ExecuteAction(GameObject boss)
    {
        boss.GetComponent<Rigidbody>().useGravity = true;

        if(!isknock)
        {
            boss.GetComponent<Rigidbody>().AddForce(-PBVec * KnockBackPower, ForceMode.Impulse);
            isknock = true;
        }


        if (Time.time - waitStartTime >= waitDuration)
        {

            return true; // アクション完了
        }

        return false; // まだ実行中
    }
}
