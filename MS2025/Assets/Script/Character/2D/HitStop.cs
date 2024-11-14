using ExitGames.Client.Photon.StructWrapping;
using Fusion;
using Fusion.Addons.Physics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitStop : NetworkBehaviour
{
    private Animator animator;
    [SerializeField, Tooltip("停止するパーティクルのオブジェクト")] GameObject[] particleSystems;

    private Coroutine hitStopCoroutine; // ヒットストップコルーチンのインスタンスを保持

    public override void Spawned()
    {
        animator = GetComponent<Animator>();
    }

    public bool IsHitStopActive
    {
        get { return hitStopCoroutine != null; } // コルーチンが実行中ならヒットストップ中
    }

    /*
     * ヒットストップを発動するメソッド
     * @param hitStopDuration ヒットストップの時間(f)
     */
    public void ApplyHitStop(float hitStopDuration)
    {
        if (hitStopCoroutine == null) // すでにヒットストップが実行中でない場合のみ発動
        {
            Debug.Log("ヒットストップを発動");
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
                    Debug.Log("とマップ " + particleSystem.name);
                }
                else
                {
                    time.Add(0);
                }
            }
        }

        // hitStopDuration秒待機 (実際の時間での待機)
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

        hitStopCoroutine = null; // ヒットストップ終了したのでコルーチンインスタンスをリセット
        //Debug.Log("ヒットストップ終了");
    }
}
