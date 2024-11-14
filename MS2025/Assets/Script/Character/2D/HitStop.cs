using ExitGames.Client.Photon.StructWrapping;
using Fusion;
using Fusion.Addons.Physics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitStop : NetworkBehaviour
{
    private Animator animator;
    [SerializeField, Tooltip("��~����p�[�e�B�N���̃I�u�W�F�N�g")] GameObject[] particleSystems;

    private Coroutine hitStopCoroutine; // �q�b�g�X�g�b�v�R���[�`���̃C���X�^���X��ێ�

    public override void Spawned()
    {
        animator = GetComponent<Animator>();
    }

    public bool IsHitStopActive
    {
        get { return hitStopCoroutine != null; } // �R���[�`�������s���Ȃ�q�b�g�X�g�b�v��
    }

    /*
     * �q�b�g�X�g�b�v�𔭓����郁�\�b�h
     * @param hitStopDuration �q�b�g�X�g�b�v�̎���(f)
     */
    public void ApplyHitStop(float hitStopDuration)
    {
        if (hitStopCoroutine == null) // ���łɃq�b�g�X�g�b�v�����s���łȂ��ꍇ�̂ݔ���
        {
            Debug.Log("�q�b�g�X�g�b�v�𔭓�");
            hitStopCoroutine = StartCoroutine(DoHitStop(hitStopDuration));
        }
    }

    private IEnumerator DoHitStop(float hitStopDuration)
    {
        List<float> time = new List<float>();
        Vector3 vel=GetComponent<NetworkRigidbody3D>().Rigidbody.velocity;
        Vector3 hozonvel=GetComponent<NetworkRigidbody3D>().Rigidbody.velocity;

        if (animator != null)
        {
            animator.speed = 0;
        }
        vel.x = 0;
        vel.y = 0;
        vel.z = 0;
        GetComponent<NetworkRigidbody3D>().Rigidbody.velocity = vel;
        if (particleSystems != null)
        {
            foreach (var particleSystem in particleSystems)
            {
                var ps = particleSystem.GetComponent<ParticleSystem>();
                if (ps.isPlaying)
                {
                    time.Add(0.5f);
                    ps.Pause();
                    Debug.Log("�ƃ}�b�v " + particleSystem.name);
                }
                else
                {
                    time.Add(0);
                }
            }
        }

        // hitStopDuration�b�ҋ@ (���ۂ̎��Ԃł̑ҋ@)
        yield return new WaitForSecondsRealtime(hitStopDuration / 60.0f);

        if (animator != null)
        {
            animator.speed = 1;
        }

        GetComponent<NetworkRigidbody3D>().Rigidbody.velocity = hozonvel;

        if (particleSystems != null)
        {
            for (int i = 0; i < particleSystems.Length; i++)
            {
                if (time[i] != 0)
                {
                    particleSystems[i].GetComponent<ParticleSystem>().Play();
                }
            }
        }

        hitStopCoroutine = null; // �q�b�g�X�g�b�v�I�������̂ŃR���[�`���C���X�^���X�����Z�b�g
        //Debug.Log("�q�b�g�X�g�b�v�I��");
    }
}
