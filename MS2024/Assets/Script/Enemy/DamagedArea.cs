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

    private float delayTime;
    private float nowDelayTime;
    private bool isBuffer;

    [Header("マルチプレイ用設定")]
    [Tooltip("各プレイヤーごとのisActive状態")]
    private Dictionary<Player, bool> playerActiveStates = new Dictionary<Player, bool>();
    [Tooltip("各プレイヤーごとのクールダウンタイマー")]
    private Dictionary<Player, float> playerCooldowns = new Dictionary<Player, float>();

    private void Awake(){
        //コライダーのisTriggerの値をtrueにする
        Collider col = GetComponent<Collider>();
        if (col != null){
            col.isTrigger = true;
        }
    }

    private void Start() {
        isActive = false;
    }

    private void Update() {
        nowDelayTime += Time.deltaTime;
        if (nowDelayTime >= delayTime) {
            isActive = isBuffer;
            if (!isSustained) ResetDamageState();
        }
        //Debug.Log("player.HP"+player.HP);
        //Debug.Log("nowTime"+nowTime);
        //Debug.Log("playerCooldowns[player]"+playerCooldowns[player]/60);
        //Debug.Log("playerCooldowns[player]"+playerCooldowns[player]);
    }

    private void OnTriggerStay(Collider other){
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
                if (!IsParry(other)) {
                    player.HP -= damage;
                    player.FlashReset();
                    playerCooldowns[player] = 0f;
                }else{
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
                if (!IsParry(other)) {
                    player.HP -= damage;
                    player.FlashReset();
                    playerActiveStates[player] = false;
                }else{
                    playerActiveStates[player] = false;
                }
            }
        }
    }

    private void OnTriggerExit(Collider other){
        Player player = other.GetComponent<Player>();
        //判定から抜けたら無敵時間をリセット
        if (playerCooldowns.ContainsKey(player)){
            playerCooldowns.Remove(player);
        }

        if (false && playerActiveStates.ContainsKey(player) && isSustained){
            playerActiveStates.Remove(player);
        }
    }
    
    public void SetImmediateActive(bool flag){
        //ダメージを有効にするSetter
        isActive = flag;
        if (!isSustained) ResetDamageState();
    }

    public void SetDelayActive(bool flag, float delay) {
        //ダメージを有効にするSetter
        isBuffer = flag;
        delayTime = delay;
    }

    private void ResetDamageState(){
        // すべてのプレイヤーのダメージ状態をリセット
        List<Player> keys = new List<Player>(playerActiveStates.Keys);
        foreach (var player in keys){
            playerActiveStates[player] = true;
        }
    }

    private bool IsParry(Collider other) {
        var PObj = other.GetComponent<PlayerParry>();
        if(PObj == null) return false;
        return PObj.ParryCheck();
    }
}
