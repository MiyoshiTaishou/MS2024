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

    [Header("�U��SE"), SerializeField] private AudioClip[] attackSE;


    [SerializeField, Tooltip("����f")]
    int Startup;
    [SerializeField, Tooltip("����f")]
    int Active;
    [SerializeField, Tooltip("�d��f")]
    int Recovery;
    [SerializeField, Tooltip("�A�g�t�B�j�b�V������f")]
    int buddyStartup;
    [SerializeField, Tooltip("�A�g�t�B�j�b�V������f")]
    int buddyActive;
    [SerializeField, Tooltip("�A�g�t�B�j�b�V���d��f")]
    int buddyRecovery;

    int Count;

    [SerializeField, Tooltip("�U���O��̃G�t�F�N�g")]
    List<GameObject> effectList;

    [SerializeField, Tooltip("�U���O��̃G�t�F�N�g")]
    GameObject effectobj;
    ParticleSystem particle;

    [Networked] private bool isEffect { get; set; }

    HitStop hitStop;
    GameObject BossObj = null;

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

        particle = effectobj.GetComponent<ParticleSystem>();
        hitStop = GetComponent<HitStop>();
        BossObj = GameObject.Find("Boss2D");
        if(BossObj==null)
        {
            Debug.LogError("�ڂ��Ȃ���");
        }
    }

    public override void FixedUpdateNetwork()
    {
        if (Object.HasStateAuthority && GetInput(out NetworkInputData data) && !hitStop.IsHitStopActive)
        {
            AnimatorStateInfo landAnimStateInfo = GetComponent<Animator>().GetCurrentAnimatorStateInfo(0);
            if(landAnimStateInfo.IsName("APlayerParry")||//�p���B���͍U�����Ȃ�
                landAnimStateInfo.IsName("APlayerJumpUp")|| landAnimStateInfo.IsName("APlayerJumpDown"))//�W�����v���͍U�����Ȃ�
            {
                return;
            }
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
                //particle.Play();
            }
            else if(pressed.IsSet(NetworkInputButtons.Attack) && currentCombo >= 2)
            {
                isAttack = true; // �U���t���O�𗧂Ă�
                isPlayingAnimation = true;
                isOnce = true;
                //�S�v���C���[��SE���Đ�����
                RPC_SE();
                //isEffect= true;

            }
        }
        Attack();
    }

    public override void Render()
    {
        // ���݂̃A�j���[�V�����̏�Ԃ��擾
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);


        // �U���t���O�������Ă���ꍇ�ɃA�j���[�V�������g���K�[
        if(isOnce&&BossObj.GetComponent<BossAI>().GetCurrentAction().actionName=="Idol")
        {
            //Debug.Log("�A�g�U����������");
            isEffect = true;
            isOnce = false; // �t���O�����Z�b�g
        }
        else if (isOnce&& currentCombo==0)
        {
            //Debug.LogError("��̔錕");
            //animator.SetTrigger("Attack"); // �A�j���[�V�����̃g���K�[
            animator.Play("APlayerAttack");
            //effectList[0].GetComponent<ParticleSystem>().Play();
            isOnce = false; // �t���O�����Z�b�g
        }
        else if (isOnce&& currentCombo==1)
        {
            //Debug.LogError("��̔錕");
            //animator.SetTrigger("Attack2"); // �A�j���[�V�����̃g���K�[
            animator.Play("APlayerAttack2");
            //effectList[1].GetComponent<ParticleSystem>().Play();
            isOnce = false; // �t���O�����Z�b�g

        }
        else if (isOnce&& currentCombo>=2)
        {
            //Debug.LogError("�I�̔錕");
            //animator.SetTrigger("Attack3"); // �A�j���[�V�����̃g���K�[
            animator.Play("APlayerAttack3");
           // effectList[2].GetComponent<ParticleSystem>().Play();
            isOnce = false; // �t���O�����Z�b�g
           
        }

        if (isEffect)
        {
            particle.Play();
            isEffect = false;
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
        switch (currentCombo) 
        {
            case 0:
                audioSource.PlayOneShot(attackSE[0]);
                break;
            case 1:
                audioSource.PlayOneShot(attackSE[1]);
                break;
            case 2:
                audioSource.PlayOneShot(attackSE[2]);
                break;
            default: 
                break;
        }
        audioSource.PlayOneShot(attackSE[0]);
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

        if(BossObj.GetComponent<BossAI>().Nokezori==1)
        {
            if (Count < buddyStartup)
            {
                Count++;
            }
            else if (Count < buddyStartup + buddyActive)
            {
                Count++;
                attackArea.SetActive(true);
            }
            else if (Count < buddyStartup + buddyActive + buddyRecovery)
            {
                Count++;
                attackArea.SetActive(false);
            }
            else if (Count >= buddyStartup + buddyActive + buddyRecovery)
            {
                Count = 0;
                isAttack = false;
            }
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
