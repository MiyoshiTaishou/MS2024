using UnityEngine.InputSystem;
using UnityEngine;

public class LocalPlayerAttack : MonoBehaviour
{
    GameObject AttackArea;
    private Animator animator;
    AudioSource audioSource;
    AudioManager audioManager;

    //çUåÇÇ™î≠ê∂Ç∑ÇÈÇ‹Ç≈ÇÃéûä‘
    [SerializeField, Tooltip("çUåÇÇÃî≠ê∂ÉtÉåÅ[ÉÄ")] public int AttackStartupFrame = 25;
    //çUåÇÇÃå¯â éûä‘
    [SerializeField, Tooltip("çUåÇÇÃéùë±ÉtÉåÅ[ÉÄ")] public int AttackActiveFrame = 50;
    //çUåÇÇÃçdíºéûä‘
    [SerializeField, Tooltip("çUåÇÇÃçdíºÉtÉåÅ[ÉÄ")] public int AttackRecoveryFrame = 100;
    [SerializeField, ReadOnly] public bool isAttack = false;
    [SerializeField, ReadOnly] public int AttackCount = 0;
    [SerializeField, ReadOnly] bool isLeftAttack = false;
    public void SetLeftAttack(bool _isleft) { isLeftAttack = _isleft; }

    bool isBuddyAttack = false;

    //âΩòAåÇñ⁄
    static int nHit = 0;
    //ç≈ëÂòAåÇêî
    const int nMaxHit = 2;
    public int GetHit() { return nHit; }
    public void AddHit()
    {
        nHit++;
        if (nHit > nMaxHit || isBuddyAttack )
        {
            nHit = 0;
            isBuddyAttack= false;
        }
        Debug.Log("òAåÇêî:" + nHit);
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
            if (nHit == 0)
            {
                animator.Play("APlayerAtack1");
            }
            else if (nHit == 1)
            {
                animator.Play("APlayerAtack2");
            }
            else if(isBuddyAttack) 
            {
                animator.Play("APlayerAtack3");
            }
            AttackArea = transform.Find("PlayerAttackArea").gameObject;
            Vector3 pos = AttackArea.transform.localPosition;
            float x=Mathf.Abs(pos.x);
            pos.x = isLeftAttack ? -x : x;
            //AttackArea.transform.localPosition = pos;
            if (isAttack == false)
            {
                //Debug.Log("çUåÇ");
                AttackCount = AttackStartupFrame;
                state = AttackState.Startup;
                isAttack = true;
            }
            else if (isBuddyAttack&& state ==AttackState.Recovery)
            {
                Debug.Log("òAågçUåÇ");
                AttackCount = AttackStartupFrame;
                state = AttackState.Startup;
                isAttack = true;

            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        audioSource = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();//ÉAÉjÉÅÅ[É^Å[
    }

    // Update is called once per frame
    void Update()
    {
        if (nHit == nMaxHit) 
        {
            isBuddyAttack = true;
        }
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
                    audioManager.PlaySE(audioSource, AudioManager.SESoundData.SE.Attack);
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
                    //ÉAÉjÉÅÅ[ÉVÉáÉìèIóπ
                    AnimatorStateInfo landAnimStateInfo = GetComponent<Animator>().GetCurrentAnimatorStateInfo(0);
                    if (landAnimStateInfo.IsName("APlayerAtack1") && landAnimStateInfo.normalizedTime >= 1.0f)
                        animator.Play("APlayerIdle");
                    if (landAnimStateInfo.IsName("APlayerAtack2") && landAnimStateInfo.normalizedTime >= 1.0f)
                        animator.Play("APlayerIdle");
                    if (landAnimStateInfo.IsName("APlayerAtack3") && landAnimStateInfo.normalizedTime >= 1.0f)
                        animator.Play("APlayerIdle");
                }
                break;
        }
    }
}
