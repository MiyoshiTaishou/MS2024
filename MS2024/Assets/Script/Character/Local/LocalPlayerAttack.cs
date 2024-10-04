using UnityEngine.InputSystem;
using UnityEngine;

public class LocalPlayerAttack : MonoBehaviour
{
    GameObject AttackArea;

    //�U������������܂ł̎���
    [SerializeField, Tooltip("�U���̔����t���[��")] public int AttackStartupFrame = 25;
    //�U���̌��ʎ���
    [SerializeField, Tooltip("�U���̎����t���[��")] public int AttackActiveFrame = 50;
    //�U���̍d������
    [SerializeField, Tooltip("�U���̍d���t���[��")] public int AttackRecoveryFrame = 100;
    [SerializeField, ReadOnly] public bool isAttack = false;
    [SerializeField, ReadOnly] public int AttackCount = 0;
    [SerializeField, ReadOnly] bool isLeftAttack = false;
    public void SetLeftAttack(bool _isleft) { isLeftAttack = _isleft; }
    //���A����
    static int nHit = 0;
    //�ő�A����
    int nMaxHit = 2;
    public int GetHit() { return nHit; }
    public void AddHit()
    {
        nHit++;
        if (nHit > nMaxHit)
        {
            nHit = 0;
        }
        Debug.Log("�A����:" + nHit);
    }
    enum AttackState
    {
        None, Startup, Active, Recovery
    }

    AttackState state = AttackState.None;

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            AttackArea = transform.Find("PlayerAttackArea").gameObject;
            Vector3 pos = AttackArea.transform.localPosition;
            float x=Mathf.Abs(pos.x);
            pos.x = isLeftAttack ? -x : x;
            AttackArea.transform.localPosition = pos;
            if (isAttack == false)
            {
                //Debug.Log("�U��");
                AttackCount = AttackStartupFrame;
                state = AttackState.Startup;
                isAttack = true;
            }
            else if (nHit == 2)
            {
                Debug.Log("�A�g�U��");
                AttackCount = AttackStartupFrame;
                state = AttackState.Startup;
                isAttack = true;

            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case AttackState.None:
                break;
            case AttackState.Startup:
                AttackCount--;
                if (AttackCount <= 0)
                {
                    state = AttackState.Active;
                    AttackArea.SetActive(true);
                    AttackCount = AttackActiveFrame;
                }
                break;
            case AttackState.Active:
                AttackCount--;
                if (AttackCount <= 0)
                {
                    state = AttackState.Recovery;
                    AttackArea.SetActive(false);
                    AttackCount = AttackRecoveryFrame;
                }
                break;
            case AttackState.Recovery:
                AttackCount--;
                if (AttackCount <= 0)
                {
                    state = AttackState.None;
                    isAttack = false;
                    AttackCount = 0;
                }
                break;
        }
    }
}
