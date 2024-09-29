using UnityEngine.InputSystem;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    //çUåÇîÕàÕ
    [SerializeField, Tooltip("ÉpÉäÉBâ¬éãâªóp")] private GameObject AttackArea;

    //çUåÇÇ™î≠ê∂Ç∑ÇÈÇ‹Ç≈ÇÃéûä‘
    [SerializeField, Tooltip("çUåÇÇÃî≠ê∂ÉtÉåÅ[ÉÄ")] int AttackStartupFrame = 25;

    //çUåÇÇÃå¯â éûä‘
    [SerializeField, Tooltip("çUåÇÇÃéùë±ÉtÉåÅ[ÉÄ")] int AttackActiveFrame = 50;

    //çUåÇÇÃçdíºéûä‘
    [SerializeField, Tooltip("çUåÇÇÃçdíºÉtÉåÅ[ÉÄ")] int AttackRecoveryFrame = 100;

    [SerializeField, ReadOnly] bool isAttack = false;
    [SerializeField, ReadOnly] int Count = 0;

    [ReadOnly,Tooltip("âΩòAåÇñ⁄")] static int nHit = 0;
    [SerializeField, Tooltip("ç≈ëÂòAåÇêî")] int nMaxHit = 2;
    public int GetHit() {return nHit;}
    public void AddHit()
    {
        nHit++;
        if(nHit>nMaxHit)
        {
            nHit = 0;
        }
        Debug.Log("òAåÇêî:" + nHit);
    }
    enum AttackState
    {
        None,Startup,Active,Recovery
    }

    AttackState state=AttackState.None;

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.started && isAttack ==false)
        {
            Debug.Log("çUåÇ");
            Count = AttackStartupFrame;
            state = AttackState.Startup;
            isAttack = true;
        }
        else if (context.started && nHit==2) 
        {
            Debug.Log("òAågçUåÇ");
            Count = AttackStartupFrame;
            state = AttackState.Startup;
            isAttack = true;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
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
