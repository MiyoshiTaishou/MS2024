using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : NetworkBehaviour
{
    [SerializeField,Header("ƒJƒƒ‰‚ÌU‚ê•")]
    AnimationCurve curve;

    [Rpc(RpcSources.All, RpcTargets.All)]
    public void RPC_CameraShake(float duration,float magnitude)
    {
        StartCoroutine(Shake(duration, magnitude));
    }

    /// <summary>
    /// ƒJƒƒ‰‚ğ—h‚ç‚·ˆ—
    /// </summary>
    /// <param name="duration"></param>
    /// <param name="magnitude"></param>
    /// <returns></returns>
    private IEnumerator Shake(float duration, float magnitude)
    {
        Vector3 originalPosition = transform.position;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            transform.position = originalPosition + Random.insideUnitSphere * magnitude * curve.Evaluate(elapsed);
            elapsed += Time.deltaTime;
            yield return null;
        }
        transform.position = originalPosition;
    }
}
