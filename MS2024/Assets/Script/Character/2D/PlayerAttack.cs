using UnityEngine.InputSystem;
using UnityEngine;
using Fusion;

public class PlayerAttack : NetworkBehaviour
{
    //UŒ‚”»’èobj
    [SerializeField, Tooltip("ƒpƒŠƒB‰ÂŽ‹‰»—p")] private GameObject AttackArea;

    //UŒ‚‚Ì”­¶ƒtƒŒ[ƒ€
    [SerializeField, Tooltip("UŒ‚‚Ì”­¶ƒtƒŒ[ƒ€")] int AttackStartuptime = 50;

    //UŒ‚‚ÌŽ‘±ŽžŠÔ
    [SerializeField, Tooltip("UŒ‚‚ÌŽ‘±ƒtƒŒ[ƒ€")] int AttackActivetime = 300;

    //UŒ‚”ÍˆÍ
    [SerializeField, Tooltip("UŒ‚”ÍˆÍ")]
    Vector3 scale= Vector3.one;

    [SerializeField, ReadOnly] int Count = 0;
    [SerializeField, ReadOnly] bool isAttack=false;
    [SerializeField, ReadOnly] int AttackCount;

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (AttackArea.active == false&&context.started)
        {
            Debug.Log("UŒ‚");
            Count = AttackStartuptime;
            isAttack = true;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        AttackArea.transform.localScale = scale;
    }

    // Update is called once per frame
    void Update()
    {
        AttackArea.transform.localScale = scale;

        if (isAttack && Count>0)
        {
            Count--;
        }
        else if(AttackArea.active == false && Count <= 0 &&isAttack)
        {
            AttackArea.SetActive(true);
            Count = AttackActivetime;
        }
        if(AttackArea.active == true && Count <= 0) 
        {
            AttackArea.SetActive(false);
            isAttack = false;
        }
    }
}
