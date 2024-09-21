using UnityEngine;

public class AttackEffect : MonoBehaviour
{
    [Header("表示エフェクト設定")]
    [Tooltip("攻撃予告エフェクトを決めます")]
    [SerializeField]
    private GameObject attackCircle;
    [Tooltip("攻撃エフェクトを決めます")]
    [SerializeField]
    private ParticleSystem attackEffect;
    [Tooltip("攻撃発生までの遅延時間を決めます")]
    [SerializeField]
    private float delayEffect;
    private bool attackFlag;
    private float nowTime;
    private DamagedArea damagedArea;

    private void Start(){
        damagedArea = GetComponent<DamagedArea>();
    }
    void Update() {
        //this.gameObject.SetActive(false);
        //attackCircle.SetActive(true);
        if (Input.GetKeyDown(KeyCode.Space)){
            PrepareAttack();
        }
        if(attackFlag && nowTime > delayEffect){
            ExecuteAttack();
        }
        nowTime += Time.deltaTime;

        if (!attackEffect.isPlaying && damagedArea != null){
            damagedArea.SetActive(false);
        }
    }
    
    void PrepareAttack(){
        // 攻撃予告エフェクトを表示
        attackCircle.SetActive(true);
        attackFlag = true;
        nowTime = 0;
    }

    void ExecuteAttack(){
        attackCircle.SetActive(false);
        attackEffect.Play();
        if (damagedArea != null){
            damagedArea.SetActive(true);
        }
        else{
            Debug.LogError("DamagedArea script not found!");
        }
        attackFlag = false;
    }
}
