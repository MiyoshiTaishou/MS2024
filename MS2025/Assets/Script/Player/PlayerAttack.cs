using Fusion;
using Fusion.Addons.Physics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : NetworkBehaviour
{
    Animator animator;
    AudioSource audioSource;
    GameObject attackArea;
    [SerializeField]
    GameObject netobj;
    [Networked] public int currentCombo { get; set; }

    ShareNumbers sharenum;

    [Networked] private bool isAttack { get; set; }
    [Networked] private bool isOnce { get; set; }
    [Networked] private bool isPlayingAnimation { get; set; }
    [Networked] public NetworkButtons ButtonsPrevious { get; set; }

    [Header("�U��SE"), SerializeField] private AudioClip attackSE;

    [SerializeField, Tooltip("����f")]
    int Startup;
    [SerializeField, Tooltip("����f")]
    int Active;
    [SerializeField, Tooltip("�d��f")]
    int Recovery;

    int Count;


    public override void Spawned()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();        
        attackArea = gameObject.transform.Find("AttackArea").gameObject;
        attackArea.SetActive(false);
        netobj = GameObject.Find("Networkbox");
        if(netobj==null) 
        {
            Debug.LogError("�l�b�g�I�u�W�F�N�g�Ȃ���");
        }
        sharenum = netobj.GetComponent<ShareNumbers>();
    }

    public override void FixedUpdateNetwork()
    {
        if (Object.HasStateAuthority && GetInput(out NetworkInputData data))
        {
            var pressed = data.Buttons.GetPressed(ButtonsPrevious);
            ButtonsPrevious = data.Buttons;

            // Attack�{�^���������ꂽ���A���A�j���[�V�������Đ����łȂ����`�F�b�N
            if (pressed.IsSet(NetworkInputButtons.Attack) && !isAttack && currentCombo<2)
            {
                isAttack = true; // �U���t���O�𗧂Ă�
                isPlayingAnimation = true;
                isOnce = true;
                //�S�v���C���[��SE���Đ�����
                RPC_SE();
            }
            else if(pressed.IsSet(NetworkInputButtons.Attack) && currentCombo >= 2)
            {
                isAttack = true; // �U���t���O�𗧂Ă�
                isPlayingAnimation = true;
                isOnce = true;
                //�S�v���C���[��SE���Đ�����
                RPC_SE();
            }
        }
        Attack();
    }

    public override void Render()
    {
        // ���݂̃A�j���[�V�����̏�Ԃ��擾
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        // �U���t���O�������Ă���ꍇ�ɃA�j���[�V�������g���K�[
        if (isOnce&& currentCombo==0)
        {
            //Debug.LogError("��̔錕");
            //animator.SetTrigger("Attack"); // �A�j���[�V�����̃g���K�[
            animator.Play("APlayerAttack");
            isOnce = false; // �t���O�����Z�b�g
        }
        else if (isOnce&& currentCombo==1)
        {
            //Debug.LogError("��̔錕");
            //animator.SetTrigger("Attack2"); // �A�j���[�V�����̃g���K�[
            animator.Play("APlayerAttack2");
            isOnce = false; // �t���O�����Z�b�g
        }
        else if (isOnce&& currentCombo>=2)
        {
            //Debug.LogError("�I�̔錕");
            //animator.SetTrigger("Attack3"); // �A�j���[�V�����̃g���K�[
            animator.Play("APlayerAttack3");
            isOnce = false; // �t���O�����Z�b�g
        }


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
        isAttack = true; // �U���t���O�𗧂Ă�
        isPlayingAnimation = true;
        isOnce = true;
        currentCombo = sharenum.nHitnum;
    }

    void Attack()
    {
        if(isAttack==false)
        {
            return;
        }

        if (Count < Startup) 
        {
            Count++;
        }
        else if(Count < Startup+Active)
        {
            Count++;
            attackArea.SetActive(true);
        }
        else if(Count < Startup+Active+Recovery)
        {
            Count++;
            attackArea.SetActive(false);
        }
        else if(Count >= Startup + Active + Recovery)
        {
            Count = 0;
            isAttack = false;
        }
    }
}
