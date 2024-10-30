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
        // Rigidbodyコンポーネントを取得
        rb3D = GetComponent<NetworkRigidbody3D>();
    }

    public void ApplyKnockback(Vector3 sourcePosition, float knockbackForce)
    {
        Debug.Log("ノックバック");

        // ノックバック方向の計算（キャラクターから攻撃の発生源に向けて）
        Vector3 knockbackDirection = ( sourcePosition).normalized;

        // Rigidbodyにノックバックの力を加える
        rb3D.Rigidbody.AddForce(knockbackDirection * knockbackForce, ForceMode.Impulse);
    }
}
