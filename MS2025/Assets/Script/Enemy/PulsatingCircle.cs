using Fusion;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class PulsatingCircle : NetworkBehaviour
{
    [Header("スケール設定")]
    [SerializeField] private float minScale = 1.0f; // 最小スケール
    [SerializeField] private float maxScale = 2.0f; // 最大スケール
    [SerializeField] private float speed = 1.0f;    // スケール変化速度

    private float scaleDirection = 1.0f; // スケールの増減方向
    private Vector3 initialScale;        // 元のスケール

    public void SetMaxScale(float _max) { maxScale = _max; }
    public void SetSpeed(float _speed) { speed = _speed; }

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
        float currentScale = transform.localScale.x * 1.5f;

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

        transform.position = new Vector3(transform.position.x, 2f, transform.position.z);
    }

    [Rpc(RpcSources.All, RpcTargets.All)]
    public void RPC_Active(bool _active)
    {
       gameObject.SetActive(_active);
    }

    [Rpc(RpcSources.All, RpcTargets.All)]
    public void RPC_Scale(float _scale)
    {
        SetMaxScale(_scale);
    }

    [Rpc(RpcSources.All, RpcTargets.All)]
    public void RPC_Spedd(float _speed)
    {
        SetSpeed(_speed);
    }
}
