using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StayLoadScene : NetworkBehaviour
{
    [SerializeField, Header("���l�҂�")]
    private int maxNum = 1;

    [Networked]
    private int nowNum { get; set; }

    [SerializeField]
    private string nextSceneName; // �J�ڐ�̃V�[����

    [SerializeField]
    private TransitionManager transitionManager;

    private NetworkRunner networkRunner;

    private bool isOnce = false;
   
    public override void Spawned()
    {
        networkRunner = FindObjectOfType<NetworkRunner>();
    }

    // Trigger ���G���A���ɓ������Ƃ��ɌĂ΂��
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // �v���C���[���G���A�ɓ��������ǂ������m�F
        {
            if (Object.HasStateAuthority)
            {
                nowNum++;
                Debug.Log("Player entered. Current count: " + nowNum);
            }
        }
    }

    // Trigger ���G���A����o���Ƃ��ɌĂ΂��
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (Object.HasStateAuthority)
            {
                nowNum--;
                Debug.Log("Player exited. Current count: " + nowNum);
            }
        }
    }

    public override void FixedUpdateNetwork()
    {
        // �S�����G���A�ɓ����Ă��邩�ǂ������`�F�b�N
        if (Object.HasStateAuthority && nowNum == maxNum && !isOnce)
        {
            // �V�[���J�ڂ̏���
            Debug.Log("All players are in. Loading next scene.");

            transitionManager.TransitionStart();

            StartCoroutine(Load());
            
            isOnce = true;
        }
    }

    private IEnumerator Load()
    {
        yield return new WaitForSeconds(2f);
        networkRunner.LoadScene(nextSceneName);
    }
}
