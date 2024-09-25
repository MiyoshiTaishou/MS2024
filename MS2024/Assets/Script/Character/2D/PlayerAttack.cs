using UnityEngine.InputSystem;
using UnityEngine;
using Fusion;

public class PlayerAttack : NetworkBehaviour
{
    //U»èobj
    [SerializeField, Tooltip("pBÂ»p")] private GameObject AttackArea;

    //UÌ­¶t[
    [SerializeField, Tooltip("UÌ­¶t[")] int AttackStartuptime = 50;

    //UÌ±Ô
    [SerializeField, Tooltip("UÌ±t[")] int AttackActivetime = 300;

    //UÍÍ
    [SerializeField, Tooltip("UÍÍ")]
    Vector3 scale= Vector3.one;

    [SerializeField, ReadOnly] int Count = 0;
    [SerializeField, ReadOnly] bool isAttack=false;
    [SerializeField, ReadOnly] int AttackCount;

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (AttackArea.active == false&&context.started)
        {
            Debug.Log("U");
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
