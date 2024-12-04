using ExitGames.Client.Photon.StructWrapping;
using Fusion;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class HitStop : NetworkBehaviour
{
    private Animator animator;
    [SerializeField, Tooltip("��~����p�[�e�B�N���̃I�u�W�F�N�g")] GameObject[] particleSystems;

    private Coroutine hitStopCoroutine; // �q�b�g�X�g�b�v�R���[�`���̃C���X�^���X��ێ�

    [SerializeField, Tooltip("�q�b�g�X�g�b�v��̃A�j���[�V�����̖߂��")] AnimationCurve hitStopCurve;

    float SlowCountnum = 0;
    bool hitstopcheck = false;

    [SerializeField, Header("�A�j���[�V�����J�[�u�̑��x�ύX")]
    private float curveSpeed;

    public override void Spawned()
    {
        animator = GetComponent<Animator>();
    }

    public override void FixedUpdateNetwork()
    {
        SlowCountnum += Time.deltaTime * curveSpeed;
        if (hitstopcheck)
        {
            animator.speed = hitStopCurve.Evaluate(SlowCountnum);
            if (hitStopCurve.Evaluate(SlowCountnum) >= 1)
                hitstopcheck = false;

        }
        else 
        {
            animator.speed = 1;
        }

        //Debug.Log("�q�b�g�X�g�b�v"+hitStopCurve.Evaluate(SlowCountnum));
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
            //Debug.Log("�q�b�g�X�g�b�v�𔭓�");
            hitStopCoroutine = StartCoroutine(DoHitStop(hitStopDuration));
        }
    }

    private IEnumerator DoHitStop(float hitStopDuration)
    {
        List<float> time = new List<float>();
        Vector3 vel=GetComponent<Rigidbody>().velocity;
        Vector3 hozonvel=GetComponent<Rigidbody>().velocity;
        AnimatorStateInfo landAnimStateInfo = GetComponent<Animator>().GetCurrentAnimatorStateInfo(0);
        if (animator != null)
        {
            if (landAnimStateInfo.IsName("APlayerCoordinatedAttack"))
            {
                Debug.Log("�X�g�b�v");
            }

            animator.speed = 0;
        }
        vel.x = 0;
        vel.y = 0;
        vel.z = 0;
        GetComponent<Rigidbody>().velocity = vel;
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
            if (landAnimStateInfo.IsName("APlayerCoordinatedAttack"))
            {
                Debug.Log("�X�g�b�v2");
            }
            animator.speed = 1;
        }

        GetComponent<Rigidbody>().velocity = hozonvel;
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
        animator.speed = 0;
        SlowCountnum = 0;
        hitStopCoroutine = null; // �q�b�g�X�g�b�v�I�������̂ŃR���[�`���C���X�^���X�����Z�b�g
        hitstopcheck = true;
        //Debug.Log("�q�b�g�X�g�b�v�I��");
    }
}
