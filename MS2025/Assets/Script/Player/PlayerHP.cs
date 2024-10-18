using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHP : NetworkBehaviour
{
    GameObject box;

    public override void Spawned()
    {
        box = GameObject.Find("Networkbox");
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    public void RPC_DamageAnim()
    {
        GetComponent<Animator>().SetTrigger("Hurt");
    }

    /// <summary>
    /// �Q�X�g���ɑޏo���߂𑗐M
    /// </summary>
    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    public void RPC_ExitGameForGuests()
    {
        if (!Object.HasStateAuthority)
        {
            // �Q�X�g�����[����ޏo���ăV�[����ύX����
            Runner.Shutdown();
            UnityEngine.SceneManagement.SceneManager.LoadScene("MenuScene");
        }
        else
        {
            // �z�X�g�͏����҂��Ă��玩�g�̏��������s
            StartCoroutine(HandleHostShutdown());
        }
    }

    /// <summary>
    /// �z�X�g���I�����鏈��
    /// </summary>
    private IEnumerator HandleHostShutdown()
    {
        // �Q�X�g���ޏo����̂�҂i1�b���x�̒x��������j
        yield return new WaitForSeconds(1.0f);

        // �z�X�g���ޏo���ăV�[����ύX����
        Runner.Shutdown();
        UnityEngine.SceneManagement.SceneManager.LoadScene("MenuScene");
    }
}
