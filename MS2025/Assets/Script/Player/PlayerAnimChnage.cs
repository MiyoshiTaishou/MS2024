using Fusion;
using UnityEngine;

public class PlayerAnimChange : NetworkBehaviour
{
    [SerializeField, Header("アニメーションデータ")]
    private RuntimeAnimatorController[] animators;

    private NetworkRunner runner;

    public override void Spawned()
    {
        // NetworkRunnerのインスタンスを取得
        runner = NetworkRunner.FindObjectOfType<NetworkRunner>();

        // ホストとクライアントを判定してアニメーションを設定
        if (runner != null)
        {
            // StateAuthorityを持っている（ホスト）場合
            if (runner.IsServer)
            {
                if (Object.HasInputAuthority)
                {
                    // ホスト用のアニメーションを設定
                    GetComponent<Animator>().runtimeAnimatorController = animators[0];
                } 
                else
                {
                    // クライアント用のアニメーションを設定
                    GetComponent<Animator>().runtimeAnimatorController = animators[1];
                }
            }
            else
            {
                if (Object.HasInputAuthority)
                {
                    GetComponent<Animator>().runtimeAnimatorController = animators[1];
                }       
                else
                {
                    // クライアント用のアニメーションを設定
                    GetComponent<Animator>().runtimeAnimatorController = animators[0];
                }
            }
        }
    }
}
