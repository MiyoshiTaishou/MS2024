using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHP : NetworkBehaviour
{
    [Networked, SerializeField]
    private int nPlayerHP { get; set; }

    /// <summary>
    /// �_���[�W����
    /// </summary>
    /// <param name="_damage"></param>
    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void RPC_Damage(int _damage)
    {
        nPlayerHP -= _damage;

        // HP��0�ȉ��Ȃ玀�S�������Ă�
        if (nPlayerHP <= 0)
        {
            if (Object.HasStateAuthority)
            {
                // �܂��Q�X�g�ɑޏo���߂𑗐M����
                RPC_ExitGameForGuests();
            }
        }
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
