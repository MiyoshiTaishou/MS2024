using UnityEngine.InputSystem;
using UnityEngine;

public class PlayerAttack : IState
{
    private PlayerState character;
    public PlayerAttack(PlayerState character)
    {
        this.character = character;
    }

    GameObject AttackArea;

    //�����A����
    static int nHit = 0;
    //�ő�A����
    int nMaxHit = 2;
    public int GetHit() {return nHit;}
    public void AddHit()
    {
        nHit++;
        if(nHit>nMaxHit)
        {
            nHit = 0;
        }
        Debug.Log("�A����:" + nHit);
    }
    enum AttackState
    {
        None,Startup,Active,Recovery
    }

    AttackState state=AttackState.None;

    public void OnAttack(InputAction.CallbackContext context)
    {
    }

    public void Enter()
    {
        AttackArea = character.transform.Find("PlayerAttackArea").gameObject;
        if (character.isAttack == false)
        {
            Debug.Log("�U��");
            character.AttackCount = character.AttackStartupFrame;
            state = AttackState.Startup;
            character.isAttack = true;
        }
        else if (nHit == 2)
        {
            Debug.Log("�A�g�U��");
            character.AttackCount = character.AttackStartupFrame;
            state = AttackState.Startup;
            character.isAttack = true;
        }
    }
    public void Exit() 
    {

    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    public void Update()
    {
        switch(state)
        {
            case AttackState.None:
                break;
            case AttackState.Startup:
                character.AttackCount--;
                if(character.AttackCount <=0) 
                {
                    state= AttackState.Active;
                    AttackArea.SetActive(true);
                    character.AttackCount = character.AttackActiveFrame;
                }
                break;
            case AttackState.Active:
                character.AttackCount--;
                if (character.AttackCount <= 0)
                {
                    state = AttackState.Recovery;
                    AttackArea.SetActive(false);
                    character.AttackCount = character.AttackRecoveryFrame;
                }
                break;
            case AttackState.Recovery:
                character.AttackCount--;
                if (character.AttackCount <= 0)
                {
                    state = AttackState.None;
                    character.isAttack = false;
                    character.AttackCount = 0;
                }
                break;
        }
    }
}
