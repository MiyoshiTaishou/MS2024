using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knockback : MonoBehaviour
{
    private Rigidbody rb;

    void Start()
    {
        // Rigidbodyコンポーネントを取得
        rb = GetComponent<Rigidbody>();
    }

    public void ApplyKnockback(Vector3 sourcePosition, float knockbackForce)
    {
        // ノックバック方向の計算（キャラクターから攻撃の発生源に向けて）
        Vector3 knockbackDirection = ( sourcePosition).normalized;

        // Rigidbodyにノックバックの力を加える
        rb.AddForce(knockbackDirection * knockbackForce, ForceMode.Impulse);
    }
}
