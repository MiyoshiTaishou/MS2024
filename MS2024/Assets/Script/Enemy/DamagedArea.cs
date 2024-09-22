using UnityEngine;
using System.Collections.Generic;

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

    [Header("マルチプレイ用デバッグ設定")]
    [Tooltip("各プレイヤーごとのisActive状態")]
    [SerializeField]
    public Dictionary<Collider, bool> playerActiveStates = new Dictionary<Collider, bool>();
    [Tooltip("各プレイヤーごとのクールダウンタイマー")]
    private Dictionary<Collider, float> playerCooldowns = new Dictionary<Collider, float>();

    //[Header("デバッグ設定")]
    //[Tooltip("デバッグ用の変数です")]
    //[SerializeField]
    //private Player player;

    private float nowTime;

    void Awake(){
        //コライダーのisTriggerの値をtrueにする
        Collider col = GetComponent<Collider>();
        if (col != null){
            col.isTrigger = true;
        }
    }

    void Start() {
        isActive = false;
    }

    void Update() {
        //Debug.Log("NT"+nowTime);

    }

    void OnTriggerStay(Collider other){
        // プレイヤーオブジェクトを取得
        Player player = other.GetComponent<Player>();
        Debug.Log("player.HP"+player.HP);
        if (player == null) return;

        if (!playerCooldowns.ContainsKey(other)){
            playerCooldowns[other] = 0f;
        }
        if (!playerActiveStates.ContainsKey(other)){
            playerActiveStates[other] = isActive;
        }

        float nowTime = playerCooldowns[other];

        //継続ダメージを行う処理
        if (isSustained){
            if (nowTime >= coolDown){
                player.HP -= damage;
                playerCooldowns[other] = 0f;
            }

            if (player.HP <= 0){
                return;
            }
        }
        //単体ダメージを行う処理
        else{
            if (playerActiveStates[other]){
                player.HP -= damage;
                playerActiveStates[other] = false;
            }
        }

        //各プレイヤーのクールダウンタイマーを更新
        playerCooldowns[other] += Time.deltaTime;
    }

    void OnTriggerExit(Collider other){
        //判定から抜けたら無敵時間をリセット
        if (playerCooldowns.ContainsKey(other)){
            playerCooldowns.Remove(other);
        }

        if (playerActiveStates.ContainsKey(other)){
            playerActiveStates.Remove(other);
        }
    }
    
    public void SetActive(bool flag){
        //ダメージを有効にするSetter
        isActive = flag;
    }
}
