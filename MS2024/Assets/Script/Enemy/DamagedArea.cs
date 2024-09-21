using UnityEngine;

[RequireComponent(typeof(Collider))]
public class DamagedArea : MonoBehaviour
{    
    [Header("ダメージ設定")]
    [Tooltip("ダメージを与えるかを決めます")]
    public bool isActive;
    [Tooltip("単体ダメージか継続ダメージを決めます")]
    public bool isSustained;
    [Tooltip("与えるダメージを決めます")]

    [SerializeField]
    private float damage;
    [Tooltip("継続ダメージの与える間隔を決めます(1/60秒間隔)")]
    [SerializeField]
    private int coolDown;

    [Header("デバッグ設定")]
    [Tooltip("デバッグ用の変数です")]
    [SerializeField]
    private Player player;

    // public GameObject playerObj;
    private float nowTime;
    [Tooltip("当たり判定を持つオブジェクトを決めます")]
     [SerializeField]
    //private GameObject obj;

    void Awake(){
        Collider col = GetComponent<Collider>();
        if (col != null)
        {
            col.isTrigger = true;
        }
    }

    void Start() {
        isActive = true;
    }

    //void FixedUpdate() {
    void Update() {
        //Debug.Log("player.HP"+player.HP);
        //Debug.Log("NT"+nowTime);
        //gameObject.SetActive (isActive);

    }

    void OnTriggerStay(Collider other) {
        if(isActive && isSustained){
            if(nowTime == 1){
                player.HP -= damage;
            }
            else if(nowTime >= coolDown){
                nowTime = 0;
            }

            if(player.HP <= 0){
                return;
            }
        }
        else if(isActive){
            player.HP -= damage;
            isActive = false;
        }
        nowTime += Time.deltaTime;
    }

    void OnTriggerExit(Collider other) {
        nowTime = 0;
    }

    public void SetActive(bool flag){
        isActive = flag;
    }
}
