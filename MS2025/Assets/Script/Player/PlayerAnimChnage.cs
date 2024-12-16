using Fusion;
using UnityEditor;
using UnityEngine;

public class PlayerAnimChange : NetworkBehaviour
{
    [SerializeField, Header("�A�j���[�V�����f�[�^")]
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
        // NetworkRunner�̃C���X�^���X���擾
        runner = NetworkRunner.FindObjectOfType<NetworkRunner>();
        animator = GetComponent<Animator>();
        // �z�X�g�ƃN���C�A���g�𔻒肵�ăA�j���[�V������ݒ�
        if (runner != null)
        {
            // StateAuthority�������Ă���i�z�X�g�j�ꍇ
            if (runner.IsServer)
            {
                if (Object.HasInputAuthority)
                {
                    // �z�X�g�p�̃A�j���[�V������ݒ�
                    GetComponent<Animator>().runtimeAnimatorController = animators[0];
                } 
                else
                {
                    // �N���C�A���g�p�̃A�j���[�V������ݒ�
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
                    // �N���C�A���g�p�̃A�j���[�V������ݒ�
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
        // �N���C�A���g���ł��A�j���[�V�������Đ��i�l�b�g���[�N�ϐ����ς�����Ƃ��Ɏ��s�j
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
           Debug.Log($"���邩�Ȃ��� {networkedAnimationName}"+landAnimStateInfo.normalizedTime);

        }
        if(oldnetworkedAnimationName!=networkedAnimationName)
        {
            Debug.Log($"PSynchronizing animation: {networkedAnimationName}");
            animator.Play((string)networkedAnimationName);
        }
    }
}
