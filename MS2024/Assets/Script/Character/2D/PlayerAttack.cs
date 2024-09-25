using UnityEngine.InputSystem;
using UnityEngine;
using Fusion;

public class PlayerAttack : NetworkBehaviour
{
    //パリィ範囲
    [SerializeField, Tooltip("パリィ可視化用")] private GameObject AttackArea;

    //パリィの効果時間
    [SerializeField, Tooltip("攻撃の持続フレーム")] int ParryActivetime = 300;

    int Count = 0;

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            Debug.Log("攻撃");
            AttackArea.SetActive(true);
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
        if (AttackArea.active == false)
        {
            Count--;
        }
        if(Count <= 0) 
        {
            AttackArea.SetActive(false);
        }
    }
}
