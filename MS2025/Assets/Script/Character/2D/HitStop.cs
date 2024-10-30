using Fusion;
using System.Collections;
using UnityEngine;

public class HitStop : NetworkBehaviour
{
    private Animator animator;
    [SerializeField, Tooltip("停止するパーティクル")] ParticleSystem[] particleSystems;
    [SerializeField, Tooltip("停止時間(f)")] int stopFrame;
    public override void Spawned()
    {
        animator = GetComponent<Animator>();
    }

    // ヒットストップを発動するメソッド
    public void ApplyHitStop(float hitStopDuration)
    {
        StartCoroutine(DoHitStop(hitStopDuration));
    }

    private IEnumerator DoHitStop(float hitStopDuration)
    {
        // Debug.Log("ストップ");
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
        // hitStopDuration秒待機 (実際の時間での待機)
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
        //Debug.Log("再開");
    }
}
