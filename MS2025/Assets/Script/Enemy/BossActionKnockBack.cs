using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "KnockBackAction", menuName = "Boss/Actions/KnockBack")]
public class BossActionKnockBack : BossActionData
{
    [SerializeField, Header("�m�b�N�o�b�N���鋭��")]
    private float KnockBackPower = 1.0f;

    /// <summary>
    /// �v���C���[����{�X�ւ̃x�N�g��
    /// </summary>
    private Vector3 PBVec;

    [SerializeField,Header("�m�b�N�o�b�N���ԁi���ꂪ���̂��鎞�ԂɂȂ�j")]
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

            return true; // �A�N�V��������
        }

        return false; // �܂����s��
    }
}
