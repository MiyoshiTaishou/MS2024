using UnityEngine;

public class FloatingMovement : MonoBehaviour
{
    [SerializeField] private float horizontalAmplitude = 1.0f; // 横揺れの幅
    [SerializeField] private float horizontalFrequency = 1.0f; // 横揺れの速さ
    [SerializeField] private float verticalSpeed = 1.0f; // 上昇の速さ
    [SerializeField] private float fadeDuration = 2.0f; // 完全に透明になるまでの時間

    private Vector3 initialPosition;
    private SpriteRenderer spriteRenderer;
    private float time;
    private float fadeTimer = 0;

    void Start()
    {
        // 初期位置を保存
        initialPosition = transform.position;
        time = Time.time;

        // SpriteRendererを取得
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            Debug.LogError("SpriteRendererがアタッチされていません。");
        }
    }

    void Update()
    {
        // 時間に基づいて横揺れと上昇の計算
        float horizontalOffset = Mathf.Sin(Time.time * horizontalFrequency) * horizontalAmplitude;
        float verticalOffset = (Time.time - time) * verticalSpeed;

        // 新しい位置を設定
        transform.position = new Vector3(
            initialPosition.x + horizontalOffset,
            initialPosition.y + verticalOffset,
            initialPosition.z
        );

        // 徐々に透明にする
        if (spriteRenderer != null)
        {
            fadeTimer += Time.deltaTime;
            float alpha = Mathf.Lerp(1.0f, 0.0f, fadeTimer / fadeDuration);
            spriteRenderer.color = new Color(
                spriteRenderer.color.r,
                spriteRenderer.color.g,
                spriteRenderer.color.b,
                alpha
            );

            // 完全に透明になったらオブジェクトを削除
            if (alpha <= 0.0f)
            {
                Destroy(gameObject);
            }
        }
    }
}
