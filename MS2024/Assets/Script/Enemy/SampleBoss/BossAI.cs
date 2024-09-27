using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum BOSS_TURN{
    MOVE,
    ATTACK,
}

// struct PlayerObj {
//     GameObject playerObj;
//     float distance;
// }

public class BossAI : MonoBehaviour
{
    [Header("移動設定")]
    [Tooltip("移動速度を決めます")]
    [SerializeField]
    private float moveSpeed;
    [Header("攻撃設定")]
    [Tooltip("攻撃技を決めます")]
    [SerializeField]
    private GameObject[] attackSkill;
    [Tooltip("最大同時攻撃数を決めます")]
    [SerializeField]
    private int skillCount;
    private BOSS_TURN bossTurn;
    private List<GameObject> playerObjects = new List<GameObject>();
    private GameObject targetPlayer;
    private string targetTag = "Player";

    private void Start(){
        //player = GetComponent<Player>();
    }

    private void Update(){
        Transform objTransform = transform;
        Vector3 test = objTransform.position;

        if (Input.GetKeyDown(KeyCode.Space))
            PlayerSearch();

        // Vector3 pos = player.position;
        // Debug.log(pos);
        // float angle = skillCount / 360;
        float distance;
        foreach (GameObject obj in playerObjects) {
            // 距離を計算
            Vector3 pointA = transform.position; // 自分の位置
            Vector3 pointB = obj.transform.position; // ターゲットの位置
            distance = (pointB - pointA).magnitude; // ベクトルの長さで距離を計算

            // デバッグ用の線を描画
            Debug.DrawLine(pointA, pointB, Color.red); // 自分からターゲットへ赤い線
            Debug.Log("Distance: " + distance);
        }

        switch (bossTurn){
            case BOSS_TURN.MOVE:
                break;
            case BOSS_TURN.ATTACK:
                Debug.Log("Attack");
                break;
            default:
                bossTurn = BOSS_TURN.MOVE;
                break;
        }
    }

    private void PlayerSearch() {
        Debug.LogError("プレイヤーサーチ");
        GameObject[] allObjects = GameObject.FindGameObjectsWithTag("Player");
        playerObjects.Clear();
        foreach (GameObject obj in allObjects) {
            playerObjects.Add(obj);
        }
    }
}
