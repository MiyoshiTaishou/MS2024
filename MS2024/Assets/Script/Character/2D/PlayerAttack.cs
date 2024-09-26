using UnityEngine.InputSystem;
using UnityEngine;
using Fusion;

public class PlayerAttack : NetworkBehaviour
{
    //パリィ範囲
    [SerializeField, Tooltip("パリィ可視化用")] private GameObject AttackArea;

    //パリィの効果時間
    [SerializeField, Tooltip("攻撃の持続フレーム")] int ParryActivetime = 100;

    [SerializeField, ReadOnly] bool isAttack = false;
    [SerializeField, ReadOnly] int Count = 0;



    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            Debug.Log("攻撃");
            AttackArea.SetActive(true);
            isAttack = true;
            Count = ParryActivetime;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public override void FixedUpdateNetwork()
    {
        if (Object.HasInputAuthority)
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
}
