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
        // ���݂̃^�C���X�P�[����ۑ�
        float originalTimeScale = Time.timeScale;

        // �^�C���X�P�[����0�ɂ��Ē�~��Ԃ����
        Time.timeScale = 0.0f;

        // hitStopDuration�b�ҋ@ (���ۂ̎��Ԃł̑ҋ@)
        yield return new WaitForSecondsRealtime(hitStopDuration);

        // �^�C���X�P�[�������ɖ߂�
        Time.timeScale = originalTimeScale;
    }
}
