using Fusion;
using UnityEditor;
using UnityEngine;

public class PlayerAnimChange : NetworkBehaviour
{
    [SerializeField, Header("アニメーションデータ")]
    private RuntimeAnimatorController[] animators;

    private NetworkRunner runner;

    private Animator animator;

    [Networked]
    private  NetworkString<_16> networkedAnimationName{get; set;}
    [Networked]
    private  NetworkString<_16> oldnetworkedAnimationName{get; set;}

    public string GetAnim() { return (string)networkedAnimationName; }
    public override void Spawned()
    {
        // NetworkRunnerのインスタンスを取得
        runner = NetworkRunner.FindObjectOfType<NetworkRunner>();
        animator = GetComponent<Animator>();
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

    [Rpc(RpcSources.All, RpcTargets.All)]
    public void RPC_InitAction(string _anim)
    {
        networkedAnimationName= _anim;
        oldnetworkedAnimationName= networkedAnimationName;
        Debug.Log("PSynchronizing" + networkedAnimationName);
    }

    public override void Render()
    {
        // クライアント側でもアニメーションを再生（ネットワーク変数が変わったときに実行）
        if (animator != null && !string.IsNullOrEmpty((string)networkedAnimationName) && animator.GetCurrentAnimatorStateInfo(0).IsName((string)networkedAnimationName) == false)
        {
            Debug.Log($"PSynchronizing animation: {networkedAnimationName}");
            animator.Play((string)networkedAnimationName);
        }
        AnimatorStateInfo landAnimStateInfo = GetComponent<Animator>().GetCurrentAnimatorStateInfo(0);
        if (landAnimStateInfo.normalizedTime>=1.0f&&
            (landAnimStateInfo.IsName("APlayerJumpUp") || landAnimStateInfo.IsName("APlayerJumpDown") ||
            landAnimStateInfo.IsName("APlayerParry") || landAnimStateInfo.IsName("APlayerCounter") ||
            landAnimStateInfo.IsName("APlayerAttack") || landAnimStateInfo.IsName("APlayerAttack2") || landAnimStateInfo.IsName("APlayerAttack3") ||
            landAnimStateInfo.IsName("APlayerCoordinatedAttack") || landAnimStateInfo.IsName("APlayerKachiage"))&&
            !landAnimStateInfo.IsName("APlayerCharge"))
        {
           RPC_InitAction("APlayerIdle");
           Debug.Log($"あるかないで {networkedAnimationName}"+landAnimStateInfo.normalizedTime);

        }
        if(oldnetworkedAnimationName!=networkedAnimationName)
        {
            Debug.Log($"PSynchronizing animation: {networkedAnimationName}");
            animator.Play((string)networkedAnimationName);
        }
    }
}
