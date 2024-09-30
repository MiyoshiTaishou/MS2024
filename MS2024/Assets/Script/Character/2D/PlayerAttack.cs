using UnityEngine.InputSystem;
using UnityEngine;

public class PlayerAttack : IState
{
    //�U���͈�
    [SerializeField, Tooltip("�p���B�����p")] private GameObject AttackArea;

    //�U������������܂ł̎���
    [SerializeField, Tooltip("�U���̔����t���[��")] int AttackStartupFrame = 25;

    //�U���̌��ʎ���
    [SerializeField, Tooltip("�U���̎����t���[��")] int AttackActiveFrame = 50;

    //�U���̍d������
    [SerializeField, Tooltip("�U���̍d���t���[��")] int AttackRecoveryFrame = 100;

    [SerializeField, ReadOnly] bool isAttack = false;
    [SerializeField, ReadOnly] int Count = 0;

    [ReadOnly,Tooltip("���A����")] static int nHit = 0;
    [SerializeField, Tooltip("�ő�A����")] int nMaxHit = 2;
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

        if (isAttack == false)
        {
            Debug.Log("�U��");
            Count = AttackStartupFrame;
            state = AttackState.Startup;
            isAttack = true;
        }
        else if (nHit == 2)
        {
            Debug.Log("�A�g�U��");
            Count = AttackStartupFrame;
            state = AttackState.Startup;
            isAttack = true;
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
                Count--;
                if(Count <=0) 
                {
                    state= AttackState.Active;
                    AttackArea.SetActive(true);
                    Count = AttackActiveFrame;
                }
                break;
            case AttackState.Active:
                Count--;
                if (Count <= 0)
                {
                    state = AttackState.Recovery;
                    AttackArea.SetActive(false);
                    Count = AttackRecoveryFrame;
                }
                break;
            case AttackState.Recovery:
                Count--;
                if (Count <= 0)
                {
                    state = AttackState.None;
                    isAttack = false;
                    Count = 0;
                }
                break;
        }
    }
}
