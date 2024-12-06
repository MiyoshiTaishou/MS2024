using Fusion;
using UnityEngine;

public class PulsatingCircle : NetworkBehaviour
{
    [Header("スケール設定")]
    [SerializeField] private float minScale = 0.5f; // 最小スケール
    [SerializeField] private float maxScale = 2.0f; // 最大スケール
    [SerializeField] private float speed = 1.0f;    // スケール変化速度

    private float scaleDirection = 1.0f; // スケールの増減方向
    private Vector3 initialScale;        // 元のスケール

    public void SetMaxScale(float _max) { maxScale = _max; }

    public override void Spawned()
    {
        // 元のスケールを保存
        initialScale = transform.localScale;
    }

    public override void FixedUpdateNetwork()
    {
      
    }

    public override void Render()
    {
        // 現在のスケール
        float currentScale = transform.localScale.x;

        // スケールを更新
        currentScale += scaleDirection * speed * Time.deltaTime;

        // スケールが最大または最小に達したら方向を反転
        if (currentScale >= maxScale)
        {
            currentScale = maxScale;
            scaleDirection = -1.0f;
        }
        else if (currentScale <= minScale)
        {
            currentScale = minScale;
            scaleDirection = 1.0f;
        }

        // スケールを適用
        transform.localScale = new Vector3(currentScale, currentScale, 1.0f);
    }
}
