using ExitGames.Client.Photon.StructWrapping;
using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitStop : NetworkBehaviour
{
    private Animator animator;
    [SerializeField, Tooltip("��~����p�[�e�B�N���̃I�u�W�F�N�g")] GameObject[] particleSystems;

    public override void Spawned()
    {
        animator = GetComponent<Animator>();
    }

    /*
     * �q�b�g�X�g�b�v�𔭓����郁�\�b�h
     * @param hitStopDuration �q�b�g�X�g�b�v�̎���(f)
     */
    public void ApplyHitStop(float hitStopDuration)
    {
        Debug.Log("�Ƃ܂ꂦ��������������");
        StartCoroutine(DoHitStop(hitStopDuration));
    }

    private IEnumerator DoHitStop(float hitStopDuration)
    {
        List<float> time=new List<float>();
        // Debug.Log("�X�g�b�v");
        if (animator != null)
        {
            animator.speed = 0;
        }
        if (particleSystems != null)
        {
            foreach (var particleSystem in particleSystems)
            {
                if (particleSystem.GetComponent<ParticleSystem>().isPlaying)
                {
                    time.Add(0.5f);
                    particleSystem.GetComponent<ParticleSystem>().Pause();
                    Debug.Log("�ƃ}�b�v"+particleSystem.name);
                }
                else
                {
                    time.Add(0);
                }
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
            for (int i=0;i<particleSystems.Length;i++)
            {
                Debug.Log("��������������������������" + time[i]);
                if (time[i] != 0)
                {
                    Debug.Log("�Ƃ܂�����" + particleSystems[i].name);
                    particleSystems[i].GetComponent<ParticleSystem>().Play();
                }
            }
        }
        //Debug.Log("�ĊJ");
    }
}
