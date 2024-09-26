using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum BOSS_TURN{
    MOVE,
    ATTACK,
}

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
    private GameObject player;
    private List<GameObject> foundObjects = new List<GameObject>();
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
        switch (bossTurn){
            case BOSS_TURN.MOVE:
                distance = Vector3.Distance(transform.position, transform.position);
                Debug.Log("Distance: " + distance);
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
        GameObject[] allObjects = GameObject.FindGameObjectsWithTag("Player");
        foundObjects.Clear();
        foreach (GameObject obj in allObjects) {
            foundObjects.Add(obj);
        }
    }
}
