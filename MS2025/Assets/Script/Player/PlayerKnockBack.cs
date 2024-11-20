using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerKnockBack : NetworkBehaviour
{
    [SerializeField, Header("ノックバックする強さ")]
    private float KnockBackPower = 1.0f;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("ノックバック");
            //エネミーに当たったら弾き飛ばされる処理
            Vector3 vec = this.transform.position - collision.gameObject.transform.position;
            vec.y = 0;

            this.GetComponent<Rigidbody>().AddForce(-vec * KnockBackPower, ForceMode.Impulse);
        }
    }  
}
