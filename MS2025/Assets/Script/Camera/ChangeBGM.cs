using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeBGM : NetworkBehaviour
{
    [SerializeField, Header("変えるBGM")]
    private AudioSource bgm;

    [Networked]
    private bool once { get; set; }

    public override void Spawned()
    {
        once = true;
    }

    [Rpc(RpcSources.All, RpcTargets.All)]
    public void RPC_ChangeBGM()
    {
        if (once)
        {
            Debug.Log("BGM変えたよ");
            GetComponent<AudioSource>().Stop();
            bgm.Play();
            once = false;
        }
    }
}
