using Fusion;
using UnityEngine;

public class PlayerRaiseAttack : NetworkBehaviour
{
    PlayerRaise raise;
    ShareNumbers share;
    AudioSource audioSource;
    GameObject netobj;
    GameObject attackArea;
    [Networked] public NetworkButtons ButtonsPrevious { get; set; }

    [SerializeField, Tooltip("Ç©ÇøÇ†Ç∞çUåÇÇÃSE")]
    AudioClip raiseAttackSE;

    bool isAttack = false;

    [SerializeField, Tooltip("î≠ê∂f")]
    int Startup;
    [SerializeField, Tooltip("éùë±f")]
    int Active;
    [SerializeField, Tooltip("çdíºf")]
    int Recovery;

    int Count;

    // Start is called before the first frame update
    public override void Spawned()
    {
        audioSource = GetComponent<AudioSource>();
        raise = GetComponent<PlayerRaise>();
        attackArea = gameObject.transform.Find("AttackArea").gameObject;
        netobj = GameObject.Find("Networkbox");
    }
    public override void FixedUpdateNetwork()
    {
        if (Object.HasStateAuthority && GetInput(out NetworkInputData data))
        {
            var pressed = data.Buttons.GetPressed(ButtonsPrevious);
            ButtonsPrevious = data.Buttons;
            if (pressed.IsSet(NetworkInputButtons.Attack) && !isAttack&&raise.GetisRaise()) 
            {
                Attack();
                RPC_SE();
            }
        }
    }

    void Attack()
    {
        if (isAttack == false)
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
}
