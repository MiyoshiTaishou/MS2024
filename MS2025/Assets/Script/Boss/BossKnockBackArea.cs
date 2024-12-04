using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossKnockBackArea : NetworkBehaviour
{
    [SerializeField, Header("ノックバックする強さ")]
    private float KnockBackPower = 1.0f;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("ノックバック");
            //エネミーに当たったら弾き飛ばされる処理
            Vector3 vec = this.transform.position - other.gameObject.transform.position;
            vec.y = 0;

            other.GetComponent<Rigidbody>().AddForce(vec * KnockBackPower, ForceMode.Impulse);
        }
    }
}
