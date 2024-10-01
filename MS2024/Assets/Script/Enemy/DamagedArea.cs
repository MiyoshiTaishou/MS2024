using UnityEngine;
using System.Collections.Generic;

enum DAMAGE_TYPE{
    ALL,
    PLAYER_1,
    PLAYER_2
}

[RequireComponent(typeof(Collider))]
public class DamagedArea : MonoBehaviour
{    
    [Header("ダメージ設定")]
    [Tooltip("ダメージを与えるかを決めます")]
    public bool isActive;
    [Tooltip("単体ダメージか継続ダメージを決めます")]
    public bool isSustained;
    [Tooltip("ダメージの与える対象を決めます ")]
    [SerializeField]
    private DAMAGE_TYPE damageType;
    [Tooltip("与えるダメージを決めます")]
    [SerializeField]
    private float damage;
    [Tooltip("継続ダメージの与える間隔を決めます(1/60秒間隔)\n今後廃止予定")]
    [SerializeField]
    private int coolDown;

    [Header("マルチプレイ用設定")]
    [Tooltip("各プレイヤーごとのisActive状態")]
    private Dictionary<Player, bool> playerActiveStates = new Dictionary<Player, bool>();
    [Tooltip("各プレイヤーごとのクールダウンタイマー")]
    private Dictionary<Player, float> playerCooldowns = new Dictionary<Player, float>();

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
        //Debug.Log("player.HP"+player.HP);
        //Debug.Log("nowTime"+nowTime);
        //Debug.Log("playerCooldowns[player]"+playerCooldowns[player]/60);
        //Debug.Log("playerCooldowns[player]"+playerCooldowns[player]);
    }

    void OnTriggerStay(Collider other){
        // プレイヤーオブジェクトを取得
        Player player = other.GetComponent<Player>();
        if (player == null) return;

        if (!playerCooldowns.ContainsKey(player)){
            playerCooldowns[player] = 0f;
        }
        if (!playerActiveStates.ContainsKey(player)){
            playerActiveStates[player] = isActive;
        }
        float nowTime = playerCooldowns[player];

        if (player.HP <= 0){
            return;
        }

        //継続ダメージを行う処理
        if (isActive && isSustained){
            if (nowTime >= coolDown){
                if ((damageType == DAMAGE_TYPE.PLAYER_1 && !player.isHost) ||
                    (damageType == DAMAGE_TYPE.PLAYER_2 &&  player.isHost))
                    return;
                if (!IsParry()) {
                    player.HP -= damage;
                    player.FlashReset();
                    playerCooldowns[player] = 0f;
                }
            }
            //各プレイヤーのクールダウンタイマーを更新
            playerCooldowns[player]++;
        }
        //単体ダメージを行う処理
        else if (isActive && playerActiveStates[player]){
            if (playerActiveStates[player]){
                if ((damageType == DAMAGE_TYPE.PLAYER_1 && !player.isHost) ||
                    (damageType == DAMAGE_TYPE.PLAYER_2 &&  player.isHost))
                    return;
                if (!IsParry()) {
                    player.HP -= damage;
                    player.FlashReset();
                    playerActiveStates[player] = false;
                }
            }
        }
    }

    void OnTriggerExit(Collider other){
        Player player = other.GetComponent<Player>();
        //判定から抜けたら無敵時間をリセット
        if (playerCooldowns.ContainsKey(player)){
            playerCooldowns.Remove(player);
        }

        if (false && playerActiveStates.ContainsKey(player) && isSustained){
            playerActiveStates.Remove(player);
        }
    }
    
    public void SetActive(bool flag){
        //ダメージを有効にするSetter
        isActive = flag;
        if (!isSustained) ResetDamageState();
    }

    void ResetDamageState(){
        // すべてのプレイヤーのダメージ状態をリセット
        List<Player> keys = new List<Player>(playerActiveStates.Keys);
        foreach (var player in keys){
            playerActiveStates[player] = true;
        }
    }

    bool IsParry() {
        var playerState = GameObject.Find("Player2D(Clone)").GetComponent<PlayerState>();
        var currentState = playerState.GetNumState();
        if (currentState is PlayerParry) {
            var parry = (PlayerParry)currentState;
            return parry.ParryCheck();
        }
        return false;
    }
}
