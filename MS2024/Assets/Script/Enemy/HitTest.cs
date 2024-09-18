using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitTest : MonoBehaviour
{    
    [SerializeField]
    private float damage;
    [SerializeField]
    private Player player;
    public GameObject playerObj;
    private int CT;
     [SerializeField]
    private GameObject obj;

    void FixedUpdate() {
        Debug.Log(player.HP);
        Debug.Log(CT);
    }

    void OnTriggerEnter(Collider other) {
        if(CT <= 0){
            player.HP -= damage;
        }
        if(CT >= 59){
            CT = -1;
        }
        if(player.HP <= 0){
            Destroy(this.obj);
        }
        CT++;
    }
    void OnTriggerExit(Collider other) {
        CT = 0;
    }
}
