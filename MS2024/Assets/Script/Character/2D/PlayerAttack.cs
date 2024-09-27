using UnityEngine.InputSystem;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    //UŒ‚”ÍˆÍ
    [SerializeField, Tooltip("ƒpƒŠƒB‰Â‹‰»—p")] private GameObject AttackArea;

    //UŒ‚‚ª”­¶‚·‚é‚Ü‚Å‚ÌŠÔ
    [SerializeField, Tooltip("UŒ‚‚Ì”­¶ƒtƒŒ[ƒ€")] int AttackStartupFrame = 5;

    //UŒ‚‚ÌŒø‰ÊŠÔ
    [SerializeField, Tooltip("UŒ‚‚Ì‘±ƒtƒŒ[ƒ€")] int AttackActiveFrame = 50;

    //UŒ‚‚Ìd’¼ŠÔ
    [SerializeField,Tooltip("UŒ‚‚Ìd’¼ƒtƒŒ[ƒ€")] int AttackRecoveryFrame = 10;

    [SerializeField, ReadOnly] bool isAttack = false;
    [SerializeField, ReadOnly] int Count = 0;

    [SerializeField,ReadOnly,Tooltip("‰½˜AŒ‚–Ú")] int nHit = 0;

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            Debug.Log("UŒ‚");
            AttackArea.SetActive(true);
            isAttack = true;
            Count = AttackActiveFrame;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (AttackArea.active == true)
        {
            Count--;
        }
        if (Count <= 0)
        {
            AttackArea.SetActive(false);
        }
    }
}
