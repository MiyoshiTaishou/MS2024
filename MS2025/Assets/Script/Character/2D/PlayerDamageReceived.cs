using Fusion;
using Fusion.Addons.Physics;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class PlayerDamageReceived : NetworkBehaviour
{
    [SerializeField, Tooltip("エフェクトオブジェクト")]
    GameObject effect;

    ParticleSystem particle;

    [Networked] public bool isEffect { get; set; } = false;


    // Start is called before the first frame update
    public override void Spawned()
    {
        particle = effect.GetComponent<ParticleSystem>();
    }

    public void DamageReceived()
    {
        isEffect = true;
    }

    public override void Render()
    {

        if (isEffect)
        {
            isEffect = false;
            particle.Play();
        }
    }
}
