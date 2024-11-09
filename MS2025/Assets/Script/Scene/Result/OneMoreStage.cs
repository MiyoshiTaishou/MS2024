using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneMoreStage : NetworkBehaviour
{
    [SerializeField]
    private string nextSceneName; // ëJà⁄êÊÇÃÉVÅ[Éìñº

    private NetworkRunner networkRunner;
   
    public override void Spawned()
    {
        networkRunner = FindObjectOfType<NetworkRunner>();
    }
    public void OneMoreLoad()
    {
        StartCoroutine(Load());
    }

    private IEnumerator Load()
    {
        yield return new WaitForSeconds(2f);
        networkRunner.LoadScene(nextSceneName);
    }
}
