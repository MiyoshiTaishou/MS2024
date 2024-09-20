using System.Collections;
using UnityEngine;

public class HitStop : MonoBehaviour
{
    // �q�b�g�X�g�b�v�𔭓����郁�\�b�h
    public void ApplyHitStop(float hitStopDuration)
    {
        StartCoroutine(DoHitStop(hitStopDuration));
    }

    private IEnumerator DoHitStop(float hitStopDuration)
    {
       // Debug.Log("�X�g�b�v");

        // hitStopDuration�b�ҋ@ (���ۂ̎��Ԃł̑ҋ@)
        yield return new WaitForSecondsRealtime(hitStopDuration);

        //Debug.Log("�ĊJ");
    }
}
