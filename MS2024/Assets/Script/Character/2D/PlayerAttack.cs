using UnityEngine.InputSystem;
using UnityEngine;
using Fusion;

public class PlayerAttack : NetworkBehaviour
{
    //�p���B�͈�
    [SerializeField, Tooltip("�p���B�����p")] private GameObject AttackArea;

    //�p���B�̌��ʎ���
    [SerializeField, Tooltip("�U���̎����t���[��")] int ParryActivetime = 300;

    int Count = 0;

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            Debug.Log("�U��");
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
