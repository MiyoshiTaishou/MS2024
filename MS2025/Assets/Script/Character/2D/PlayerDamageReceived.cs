using Fusion;
using Fusion.Addons.Physics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDamageReceived : NetworkBehaviour
{
    [SerializeField, Tooltip("�G�t�F�N�g�I�u�W�F�N�g")]
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
