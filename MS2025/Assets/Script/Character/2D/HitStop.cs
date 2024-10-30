using Fusion;
using System.Collections;
using UnityEngine;

public class HitStop : NetworkBehaviour
{
    private Animator animator;
    [SerializeField, Tooltip("��~����p�[�e�B�N��")] ParticleSystem[] particleSystems;
    [SerializeField, Tooltip("��~����(f)")] int stopFrame;
    public override void Spawned()
    {
        animator = GetComponent<Animator>();
    }

    // �q�b�g�X�g�b�v�𔭓����郁�\�b�h
    public void ApplyHitStop(float hitStopDuration)
    {
        StartCoroutine(DoHitStop(hitStopDuration));
    }

    private IEnumerator DoHitStop(float hitStopDuration)
    {
        // Debug.Log("�X�g�b�v");
        if (animator != null)
        {
            animator.speed = 0;
        }
        if (particleSystems != null)
        {
            foreach (var particleSystem in particleSystems)
            {
                particleSystem.Stop();
            }
        }
        // hitStopDuration�b�ҋ@ (���ۂ̎��Ԃł̑ҋ@)
        yield return new WaitForSecondsRealtime(hitStopDuration/60.0f);
        if (animator != null)
        {
            animator.speed = 1;
        }
        if (particleSystems != null)
        {
            foreach (var particleSystem in particleSystems)
            {
                particleSystem.Play();
            }
        }
        //Debug.Log("�ĊJ");
    }
}
