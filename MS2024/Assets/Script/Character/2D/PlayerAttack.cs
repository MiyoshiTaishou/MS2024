using UnityEngine.InputSystem;
using UnityEngine;
using Fusion;

public class PlayerAttack : NetworkBehaviour
{
    //�p���B�͈�
    [SerializeField, Tooltip("�p���B�����p")] private GameObject AttackArea;

    //�p���B�̌��ʎ���
    [SerializeField, Tooltip("�U���̎����t���[��")] int ParryActivetime = 100;

    [SerializeField, ReadOnly] bool isAttack = false;
    [SerializeField, ReadOnly] int Count = 0;
    [SerializeField, ReadOnly] bool isOwner=false;


    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.started&&isOwner)
        {
            Debug.Log("�U��");
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
        isOwner = Object.InputAuthority == Runner.LocalPlayer;
        if (isOwner)
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
