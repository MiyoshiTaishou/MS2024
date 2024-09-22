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
        //必要なスクリプトを呼び出し
        damagedArea = GetComponent<DamagedArea>();
        attackCircle.SetActive(false);
    }
    void Update() {
        //攻撃発生処理
        if (Input.GetKeyDown(KeyCode.Space)){
            PrepareAttack();
        }
        if(attackFlag && nowTime > delayEffect){
            ExecuteAttack();
        }
        nowTime += Time.deltaTime;

        //攻撃判定を削除
        if (!attackEffect.isPlaying && damagedArea != null){
            damagedArea.SetActive(false);
        }
    }
    
    void PrepareAttack(){
        // 攻撃予告処理
        attackCircle.SetActive(true);
        attackFlag = true;
        nowTime = 0;
    }

    void ExecuteAttack(){
        //攻撃処理
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
