using UnityEngine.InputSystem;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    //UŒ‚”ÍˆÍ
    [SerializeField, Tooltip("ƒpƒŠƒB‰Â‹‰»—p")] private GameObject AttackArea;

    //UŒ‚‚ª”­¶‚·‚é‚Ü‚Å‚ÌŠÔ
    [SerializeField, Tooltip("UŒ‚‚Ì”­¶ƒtƒŒ[ƒ€")] int AttackStartupFrame = 25;

    //UŒ‚‚ÌŒø‰ÊŠÔ
    [SerializeField, Tooltip("UŒ‚‚Ì‘±ƒtƒŒ[ƒ€")] int AttackActiveFrame = 50;

    //UŒ‚‚Ìd’¼ŠÔ
    [SerializeField,Tooltip("UŒ‚‚Ìd’¼ƒtƒŒ[ƒ€")] int AttackRecoveryFrame = 100;

    [SerializeField, ReadOnly] bool isAttack = false;
    [SerializeField, ReadOnly] int Count = 0;

    [SerializeField,ReadOnly,Tooltip("‰½˜AŒ‚–Ú")] int nHit = 0;

    enum AttackState
    {
        None,Startup,Active,Recovery
    }

    AttackState state=AttackState.None;

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.started && isAttack ==false)
        {
            Debug.Log("UŒ‚");
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
