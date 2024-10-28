using Fusion;
using Fusion.Addons.Physics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knockback : NetworkBehaviour
{
    private Rigidbody rb;

    NetworkRigidbody3D rb3D;

    void Start()
    {
        // Rigidbody�R���|�[�l���g���擾
        rb3D = GetComponent<NetworkRigidbody3D>();
    }

    public void ApplyKnockback(Vector3 sourcePosition, float knockbackForce)
    {
        Debug.Log("�m�b�N�o�b�N");

        // �m�b�N�o�b�N�����̌v�Z�i�L�����N�^�[����U���̔������Ɍ����āj
        Vector3 knockbackDirection = ( sourcePosition).normalized;

        // Rigidbody�Ƀm�b�N�o�b�N�̗͂�������
        rb3D.Rigidbody.AddForce(knockbackDirection * knockbackForce, ForceMode.Impulse);
    }
}
