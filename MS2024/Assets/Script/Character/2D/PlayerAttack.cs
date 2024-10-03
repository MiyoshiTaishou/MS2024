//using UnityEngine.InputSystem;
//using UnityEngine;

//public class PlayerAttack : IState
//{
//    private PlayerState character;
//    public PlayerAttack(PlayerState character)
//    {
//        this.character = character;
//    }

//    GameObject AttackArea;

//    //何連撃目
//    static int nHit = 0;
//    //最大連撃数
//    int nMaxHit = 2;
//    public int GetHit() {return nHit;}
//    public void AddHit()
//    {
//        nHit++;
//        if(nHit>nMaxHit)
//        {
//            nHit = 0;
//        }
//        Debug.Log("連撃数:" + nHit);
//    }
//    enum AttackState
//    {
//        None,Startup,Active,Recovery
//    }

//    AttackState state=AttackState.None;

//    public void OnAttack(InputAction.CallbackContext context)
//    {
//    }

//    public void Enter()
//    {
//        AttackArea = transform.Find("PlayerAttackArea").gameObject;
//        if (isAttack == false)
//        {
//            Debug.Log("攻撃");
//            AttackCount = AttackStartupFrame;
//            state = AttackState.Startup;
//            isAttack = true;
//        }
//        else if (nHit == 2)
//        {
//            Debug.Log("連携攻撃");
//            AttackCount = AttackStartupFrame;
//            state = AttackState.Startup;
//            isAttack = true;

//        }
//    }
//    public void Exit() 
//    {

//    }

//    // Start is called before the first frame update
//    void Start()
//    {
//    }

//    // Update is called once per frame
//    public void Update()
//    {
//        switch(state)
//        {
//            case AttackState.None:
//                break;
//            case AttackState.Startup:
//                AttackCount--;
//                if(AttackCount <=0) 
//                {
//                    state= AttackState.Active;
//                    AttackArea.SetActive(true);
//                    AttackCount = AttackActiveFrame;
//                }
//                break;
//            case AttackState.Active:
//                AttackCount--;
//                if (AttackCount <= 0)
//                {
//                    state = AttackState.Recovery;
//                    AttackArea.SetActive(false);
//                    AttackCount = AttackRecoveryFrame;
//                }
//                break;
//            case AttackState.Recovery:
//                AttackCount--;
//                if (AttackCount <= 0)
//                {
//                    state = AttackState.None;
//                    isAttack = false;
//                    AttackCount = 0;
//                }
//                break;
//        }
//    }
//}

