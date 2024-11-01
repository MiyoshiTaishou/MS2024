using Fusion;
using Fusion.Addons.Physics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDamageReceived : NetworkBehaviour
{
    [SerializeField, Tooltip("エフェクトオブジェクト")]
    GameObject effect;

    ParticleSystem particle;

    // Start is called before the first frame update
    public override void Spawned()
    {
        particle = effect.GetComponent<ParticleSystem>();
    }

    public void DamageReceived()
    {
        particle.Play();
    }
}
