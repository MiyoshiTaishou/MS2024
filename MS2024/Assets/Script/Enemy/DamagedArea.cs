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
    private bool flagBuffer;
    private bool flag;
    private bool isOnline = true;

    [Header("マルチプレイ用設定")]
    [Tooltip("各プレイヤーごとのisActive状態")]
    private Dictionary<Player, bool> playerActiveStates = new Dictionary<Player, bool>();
    private Dictionary<LocalPlayer, bool> localPlayerActiveStates = new Dictionary<LocalPlayer, bool>();
    [Tooltip("各プレイヤーごとのクールダウンタイマー")]
    private Dictionary<Player, float> playerCooldowns = new Dictionary<Player, float>();
    private Dictionary<LocalPlayer, float> localPlayerCooldowns = new Dictionary<LocalPlayer, float>();

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
        if (nowDelayTime >= delayTime && flag) {
            isActive = flagBuffer;
            flag = false;
            if (!isSustained) ResetDamageState();
        }
        //Debug.Log("player.HP"+player.HP);
        //Debug.Log("nowTime"+nowTime);
        //Debug.Log("playerCooldowns[player]"+playerCooldowns[player]/60);
        //Debug.Log("playerCooldowns[player]"+playerCooldowns[player]);
    }

    private void OnTriggerStay(Collider other){
        if (isOnline){
            Online_OnTriggerStay(other);
        }
        else {
            Local_OnTriggerStay(other);
        }
    }

    private void OnTriggerExit(Collider other){
        if (isOnline){
            Online_OnTriggerExit(other);
        }
        else {
            Local_OnTriggerExit(other);
        }
    }

    private void Online_OnTriggerStay(Collider other){
        // プレイヤーオブジェクトを取得
        Player player = other.GetComponent<Player>();
        if (player == null) {
            LocalPlayer localPlayer = other.GetComponent<LocalPlayer>();
            if (localPlayer != null) isOnline = false;
            return;
        }

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

    private void Local_OnTriggerStay(Collider other){
        // プレイヤーオブジェクトを取得
        LocalPlayer localPlayer = other.GetComponent<LocalPlayer>();
        if (localPlayer == null) return;

        if (!localPlayerCooldowns.ContainsKey(localPlayer)){
            localPlayerCooldowns[localPlayer] = 0f;
        }
        if (!localPlayerActiveStates.ContainsKey(localPlayer)){
            localPlayerActiveStates[localPlayer] = isActive;
        }
        float nowTime = localPlayerCooldowns[localPlayer];

        if (localPlayer.HP <= 0){
            return;
        }

        //継続ダメージを行う処理
        if (isActive && isSustained){
            if (nowTime >= coolDown){
                if ((damageType == DAMAGE_TYPE.PLAYER_1 && !localPlayer.isHost) ||
                    (damageType == DAMAGE_TYPE.PLAYER_2 &&  localPlayer.isHost))
                    return;
                if (!IsParry(other)) {
                    localPlayer.HP -= damage;
                    localPlayer.FlashReset();
                    localPlayerCooldowns[localPlayer] = 0f;
                }else{
                    localPlayerCooldowns[localPlayer] = 0f;
                }
            }
            //各プレイヤーのクールダウンタイマーを更新
            localPlayerCooldowns[localPlayer]++;
        }
        //単体ダメージを行う処理
        else if (isActive && localPlayerActiveStates[localPlayer]){
            if (localPlayerActiveStates[localPlayer]){
                if ((damageType == DAMAGE_TYPE.PLAYER_1 && !localPlayer.isHost) ||
                    (damageType == DAMAGE_TYPE.PLAYER_2 &&  localPlayer.isHost))
                    return;
                if (!IsParry(other)) {
                    localPlayer.HP -= damage;
                    localPlayer.FlashReset();
                    localPlayerActiveStates[localPlayer] = false;
                }else{
                    localPlayerActiveStates[localPlayer] = false;
                }
            }
        }
    }

    private void Online_OnTriggerExit(Collider other){
        //判定から抜けたら無敵時間をリセット
        Player player = other.GetComponent<Player>();
        if (playerCooldowns.ContainsKey(player)){
            playerCooldowns.Remove(player);
        }

        if (false && playerActiveStates.ContainsKey(player) && isSustained){
            playerActiveStates.Remove(player);
        }
    }

    private void Local_OnTriggerExit(Collider other){
        //判定から抜けたら無敵時間をリセット
        LocalPlayer localPlayer = other.GetComponent<LocalPlayer>();
        if (localPlayerCooldowns.ContainsKey(localPlayer)){
            localPlayerCooldowns.Remove(localPlayer);
        }

        if (false && localPlayerActiveStates.ContainsKey(localPlayer) && isSustained){
            localPlayerActiveStates.Remove(localPlayer);
        }
    }

    
    public void SetImmediateActive(bool flag){
        //ダメージを有効にするSetter
        isActive = flag;
        if (!isSustained) ResetDamageState();
    }

    public void SetDelayActive(bool flag, float delay) {
        //ダメージを有効にするSetter
        this.flag = true;
        flagBuffer = flag;
        delayTime = delay;
        nowDelayTime = 0;
    }

    private void ResetDamageState(){
        // すべてのプレイヤーのダメージ状態をリセット
        if (isOnline) {
            List<Player> keys = new List<Player>(playerActiveStates.Keys);
            foreach (var player in keys){
                playerActiveStates[player] = true;
            }
        }
        else {
            List<LocalPlayer> keys = new List<LocalPlayer>(localPlayerActiveStates.Keys);
            foreach (var player in keys){
                localPlayerActiveStates[player] = true;
            }
        }
    }

    private bool IsParry(Collider other) {
        var PObj = other.GetComponent<PlayerParry>();
        if(PObj == null) return false;
        return PObj.ParryCheck();
    }
}
