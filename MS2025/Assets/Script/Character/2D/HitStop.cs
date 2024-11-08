using ExitGames.Client.Photon.StructWrapping;
using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitStop : NetworkBehaviour
{
    private Animator animator;
    [SerializeField, Tooltip("停止するパーティクルのオブジェクト")] GameObject[] particleSystems;

    public override void Spawned()
    {
        animator = GetComponent<Animator>();
    }

    /*
     * ヒットストップを発動するメソッド
     * @param hitStopDuration ヒットストップの時間(f)
     */
    public void ApplyHitStop(float hitStopDuration)
    {
        Debug.Log("とまれええええええええ");
        StartCoroutine(DoHitStop(hitStopDuration));
    }

    private IEnumerator DoHitStop(float hitStopDuration)
    {
        List<float> time=new List<float>();
        // Debug.Log("ストップ");
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
                    Debug.Log("とマップ"+particleSystem.name);
                }
                else
                {
                    time.Add(0);
                }
            }
        }

        // hitStopDuration秒待機 (実際の時間での待機)
        yield return new WaitForSecondsRealtime(hitStopDuration/60.0f);

        if (animator != null)
        {
            animator.speed = 1;
        }
        if (particleSystems != null)
        {
            for (int i=0;i<particleSystems.Length;i++)
            {
                Debug.Log("あああああああああああああ" + time[i]);
                if (time[i] != 0)
                {
                    Debug.Log("とまああぷ" + particleSystems[i].name);
                    particleSystems[i].GetComponent<ParticleSystem>().Play();
                }
            }
        }
        //Debug.Log("再開");
    }
}
