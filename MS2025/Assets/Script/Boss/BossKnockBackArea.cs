using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossKnockBackArea : NetworkBehaviour
{
    [SerializeField, Header("�m�b�N�o�b�N���鋭��")]
    private float KnockBackPower = 1.0f;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("�m�b�N�o�b�N");
            //�G�l�~�[�ɓ���������e����΂���鏈��
            Vector3 vec = this.transform.position - other.gameObject.transform.position;
            vec.y = 0;

            other.GetComponent<Rigidbody>().AddForce(vec * KnockBackPower, ForceMode.Impulse);
        }
    }
}
