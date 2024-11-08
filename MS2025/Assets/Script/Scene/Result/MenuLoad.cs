using Fusion;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuLoad : NetworkBehaviour
{
    [Header("���j���[�V�[����"), SerializeField]
    private String ResultSceneName;

    public void LoadMenu()
    {
        if (Object.HasStateAuthority)
        {
            // �N���C�A���g�ɐ�ɃV�[���J�ڂ��w��
            RPC_ClientSceneTransition();
        }
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    private void RPC_ClientSceneTransition()
    {
        // �N���C�A���g�͐�ɃV�[���J�ڂ����s
        if (!Object.HasStateAuthority)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(ResultSceneName);
        }
        else
        {
            // �z�X�g���̓N���C�A���g�̑J�ڂ�����������ɃV�[���J��
            StartCoroutine(HostSceneTransition());
        }
    }

    private IEnumerator HostSceneTransition()
    {
        yield return new WaitForSeconds(2); // �N���C�A���g�����V�[���J�ڂ���܂ł̎��Ԃ𒲐�
        Runner.Shutdown();
        UnityEngine.SceneManagement.SceneManager.LoadScene(ResultSceneName);
    }
}
