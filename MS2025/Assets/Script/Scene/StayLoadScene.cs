using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StayLoadScene : NetworkBehaviour
{
    [SerializeField, Header("何人待つか")]
    private int maxNum = 1;

    [Networked]
    private int nowNum { get; set; }

    [SerializeField]
    private string nextSceneName; // 遷移先のシーン名

    [SerializeField]
    private TransitionManager transitionManager;

    private NetworkRunner networkRunner;

    private bool isOnce = false;
   
    public override void Spawned()
    {
        networkRunner = FindObjectOfType<NetworkRunner>();
    }

    // Trigger がエリア内に入ったときに呼ばれる
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // プレイヤーがエリアに入ったかどうかを確認
        {
            if (Object.HasStateAuthority)
            {
                nowNum++;
                Debug.Log("Player entered. Current count: " + nowNum);
            }
        }
    }

    // Trigger がエリアから出たときに呼ばれる
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
        // 全員がエリアに入っているかどうかをチェック
        if (Object.HasStateAuthority && nowNum == maxNum && !isOnce)
        {
            // シーン遷移の処理
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
