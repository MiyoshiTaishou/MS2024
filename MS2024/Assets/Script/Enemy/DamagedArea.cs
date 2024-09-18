using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagedArea : MonoBehaviour
{    
    public bool isActive;
    [SerializeField]
    private float damage;
    [SerializeField]
    private Player player;
    // public GameObject playerObj;
    private int CT;
    private int CD;
     [SerializeField]
    private GameObject obj;

    void Start() {
        //isActive=true;    
    }
    void FixedUpdate() {
    //    Debug.Log(player.HP);
    //    Debug.Log(CT);
        //gameObject.SetActive (isActive);

        if(CD > 180){
            isActive=true;
        }
        CD++;
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
