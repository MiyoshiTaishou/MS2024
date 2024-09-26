using UnityEngine;

enum ACRION_TURN{
    MOVE,
    ATTACK,
}

struct Players{
    GameObject player;
    float distance;

}

public class BossAI : MonoBehaviour
{
    [Header("攻撃設定")]
    [Tooltip("最大同時攻撃数を決めます")]
    [SerializeField]
    private int MaxAttackCount;
    private Players[] players;
    private ACRION_TURN acrionTurn;

    void Start() {
        GameObject[] player = GameObject.FindGameObjectsWithTag("Player");
    }

    void Update() {
        switch (acrionTurn) {
            case ACRION_TURN.MOVE:

                break;
            case ACRION_TURN.ATTACK:
                break;
            default:
                acrionTurn = ACRION_TURN.MOVE;
                break;
        }
    }

    void PlayerSearch() {
        players.player = GameObject.FindGameObjectsWithTag("Player");
        
        foreach (var obj in players){
            obj.distance = Vector3.Distance(transform.position, obj.transform.position);
            Debug.Log("Distance: " + obj.distance);
        }
    }
}
