using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5.0f; // 移動速度を設定

    void Update()
    {
        // キーボード入力を取得
        float horizontal = Input.GetAxis("Horizontal"); // "A"や"D"キーで左・右
        float vertical = Input.GetAxis("Vertical"); // "W"や"S"キーで前・後

        // 入力に基づいて移動ベクトルを作成
        Vector3 movement = new Vector3(horizontal, 0, vertical) * speed * Time.deltaTime;

        // オブジェクトを移動
        transform.Translate(movement, Space.World);
    }
}
