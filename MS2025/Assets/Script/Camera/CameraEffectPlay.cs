using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraEffectPlay : NetworkBehaviour
{
    [SerializeField]
    private ParticleSystem _particleSystem;
    [SerializeField]
    private Animator _animator;

    [Rpc(RpcSources.All, RpcTargets.All)]
    public void RPC_CameraEffect()
    {
        Effect();
    }
   
    private void Effect()
    {
        _particleSystem.Play();
        _animator.SetTrigger("Impact");
    }
}
