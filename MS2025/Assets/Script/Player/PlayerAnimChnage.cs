using Fusion;
using UnityEngine;

public class PlayerAnimChange : NetworkBehaviour
{
    [SerializeField, Header("�A�j���[�V�����f�[�^")]
    private RuntimeAnimatorController[] animators;

    private NetworkRunner runner;

    public override void Spawned()
    {
        // NetworkRunner�̃C���X�^���X���擾
        runner = NetworkRunner.FindObjectOfType<NetworkRunner>();

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
}
