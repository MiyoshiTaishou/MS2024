using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knockback : MonoBehaviour
{
    private Rigidbody rb;

    void Start()
    {
        // Rigidbody�R���|�[�l���g���擾
        rb = GetComponent<Rigidbody>();
    }

    public void ApplyKnockback(Vector3 sourcePosition, float knockbackForce)
    {
        // �m�b�N�o�b�N�����̌v�Z�i�L�����N�^�[����U���̔������Ɍ����āj
        Vector3 knockbackDirection = ( sourcePosition).normalized;

        // Rigidbody�Ƀm�b�N�o�b�N�̗͂�������
        rb.AddForce(knockbackDirection * knockbackForce, ForceMode.Impulse);
    }
}
