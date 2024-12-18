using Fusion;
using System.Reflection;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class PlayerRaiseAttack : NetworkBehaviour
{
    PlayerRaise raise;
    ShareNumbers share;
    AudioSource audioSource;
    GameObject netobj;
    GameObject attackArea;
    [Networked] public NetworkButtons ButtonsPrevious { get; set; }
    [SerializeField,Tooltip("�G�t�F�N�g")] 
    GameObject effect;
    ParticleSystem particle;

    [SerializeField, Tooltip("���������U����SE")]
    AudioClip raiseAttackSE;

    bool isAttack = false;

    [SerializeField, Tooltip("����f")]
    int Startup;
    [SerializeField, Tooltip("����f")]
    int Active;
    [SerializeField, Tooltip("�d��f")]
    int Recovery;

    int Count;

    GameObject change;

    [Networked] bool isOnce  { get; set; } = false;
    [Networked] bool isEffect{ get; set; } = false;
    PlayerFreeze freeze;

    // Start is called before the first frame update
    public override void Spawned()
    {
        change = GameObject.Find("ChangeAction");
        audioSource = GetComponent<AudioSource>();
        raise = GetComponent<PlayerRaise>();
        attackArea = gameObject.transform.Find("AttackArea").gameObject;
        netobj = GameObject.Find("Networkbox");
        particle = effect.GetComponent<ParticleSystem>();
        freeze= GetComponent<PlayerFreeze>();
    }
    public override void FixedUpdateNetwork()
    {
        if(!raise.GetisRaise())
        {
            Count = 0;
            isAttack = false;
            return;
        }

        if (Object.HasStateAuthority && GetInput(out NetworkInputData data))
        {
            var pressed = data.Buttons.GetPressed(ButtonsPrevious);
            ButtonsPrevious = data.Buttons;
            if (pressed.IsSet(NetworkInputButtons.Attack) && !isAttack) 
            {
                isOnce = true;
                isAttack = true;
                RPC_SE();
            }
                Attack();
        }
    }

    void Attack()
    {
        if (isAttack==false)
        {
            return;
        }

        if (Count < Startup)
        {
            Count++;
        }
        else if (Count < Startup + Active)
        {
            Count++;
            if (change.GetComponent<ChangeBossAction>().TextNo == 2)
            {
                change.GetComponent<ChangeBossAction>().TextNo = 3;
            }
            attackArea.SetActive(true);
        }
        else if (Count < Startup + Active + Recovery)
        {
            Count++;
            attackArea.SetActive(false);
        }
        else if (Count >= Startup + Active + Recovery)
        {
            Count = 0;
            isAttack = false;
        }
    }
    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void RPC_SE() 
    {
        audioSource.PlayOneShot(raiseAttackSE);
    }
    public override void Render()
    {
        // ���݂̃A�j���[�V�����̏�Ԃ��擾
        Animator animator = GetComponent<Animator>();
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        // �U���t���O�������Ă���ꍇ�ɃA�j���[�V�������g���K�[
        if (isOnce)
        {
            GetComponent<PlayerAnimChange>().RPC_InitAction("APlayerAttack");
            isOnce = false; // �t���O�����Z�b�g
            isEffect = true;
        }

        if (isEffect)
        {
            particle.Play();
            isEffect = false;
        }

    }

}
