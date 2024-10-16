using Fusion;
using Fusion.Addons.Physics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class PlayerAttack : NetworkBehaviour
{
    Animator animator;
    AudioSource audioSource;
    GameObject attackArea;
    GameObject director;

    [Networked] private bool isAttack { get; set; }
    [Networked] private bool isOnce { get; set; }
    [Networked] private bool isPlayingAnimation { get; set; }
    [Networked] public NetworkButtons ButtonsPrevious { get; set; }

    [Header("�U��SE"), SerializeField] private AudioClip attackSE;

    public override void Spawned()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();        
        attackArea = gameObject.transform.Find("AttackArea").gameObject;
        director = GameObject.Find("TLDirector");

        attackArea.SetActive(false);
    }

    public override void FixedUpdateNetwork()
    {
        if (Object.HasStateAuthority && GetInput(out NetworkInputData data))
        {
            var pressed = data.Buttons.GetPressed(ButtonsPrevious);
            ButtonsPrevious = data.Buttons;

            // Attack�{�^���������ꂽ���A���A�j���[�V�������Đ����łȂ����`�F�b�N
            if (pressed.IsSet(NetworkInputButtons.Attack) && !isAttack)
            {
                isAttack = true; // �U���t���O�𗧂Ă�
                isPlayingAnimation = true;
                isOnce = true;

                //�S�v���C���[��SE���Đ�����
                RPC_SE();

                Debug.Log("�U����������");
                //GetComponent<SpecialAttackTLPlay>().RPC_Director();
                director.GetComponent<PlayableDirector>().Play();
            }
        }
    }

    public override void Render()
    {
        //// ���݂̃A�j���[�V�����̏�Ԃ��擾
        //AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        //// �U���t���O�������Ă���ꍇ�ɃA�j���[�V�������g���K�[
        //if (isOnce)
        //{
        //    animator.SetTrigger("Attack"); // �A�j���[�V�����̃g���K�[
        //    isOnce = false; // �t���O�����Z�b�g
        //}

        //// �A�j���[�V�������Đ����ł���ꍇ�̏���
        //if (stateInfo.IsName("APlayerAttack"))
        //{
        //    attackArea.SetActive(true); // �U���G���A���A�N�e�B�u��       
        //}
        //else
        //{
        //    attackArea.SetActive(false); // �A�j���[�V�������Đ����łȂ��ꍇ�͍U���G���A�𖳌���
        //    isAttack = false;
        //}
    }

    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void RPC_SE()
    {       
        audioSource.PlayOneShot(attackSE);
    }
}
