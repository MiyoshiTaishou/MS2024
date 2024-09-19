using System.Collections;
using System.Collections.Generic;
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
    private int CT;
    private int CD;
    [Tooltip("デバッグ用の変数です")]
     [SerializeField]
    private GameObject obj;

    void Awake()
    {
        Collider col = GetComponent<Collider>();
        if (col != null)
        {
            col.isTrigger = true;
        }
    }

    void Start() {
        isActive=true;    
    }

    void FixedUpdate() {
        Debug.Log("player.HP"+player.HP);
        Debug.Log("CT"+CT);
        Debug.Log("CD"+CD);
        //gameObject.SetActive (isActive);
    }

    void OnTriggerEnter(Collider other) {
        if(isSustained){
            if(CT <= 1){
                player.HP -= damage;
            }
            if(CT >= coolDown){
                CT = 0;
            }
            if(player.HP <= 0){
                isActive=false;
                //Destroy(this.obj);
            }
            CT++;
        }else{
            isActive=false;
            //Destroy(this.obj);
        }
    }
    void OnTriggerExit(Collider other) {
        //CT = 0;
    }
}
