using UnityEngine.InputSystem;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    //�U���͈�
    [SerializeField, Tooltip("�p���B�����p")] private GameObject AttackArea;

    //�U������������܂ł̎���
    [SerializeField, Tooltip("�U���̔����t���[��")] int AttackStartupFrame = 5;

    //�U���̌��ʎ���
    [SerializeField, Tooltip("�U���̎����t���[��")] int AttackActiveFrame = 50;

    //�U���̍d������
    [SerializeField,Tooltip("�U���̍d���t���[��")] int AttackRecoveryFrame = 10;

    [SerializeField, ReadOnly] bool isAttack = false;
    [SerializeField, ReadOnly] int Count = 0;

    [SerializeField,ReadOnly,Tooltip("���A����")] int nHit = 0;

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            Debug.Log("�U��");
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
