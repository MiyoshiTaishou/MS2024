using Fusion;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class PulsatingCircle : NetworkBehaviour
{
    [Header("�X�P�[���ݒ�")]
    [SerializeField] private float minScale = 1.0f; // �ŏ��X�P�[��
    [SerializeField] private float maxScale = 2.0f; // �ő�X�P�[��
    [SerializeField] private float speed = 1.0f;    // �X�P�[���ω����x

    private float scaleDirection = 1.0f; // �X�P�[���̑�������
    private Vector3 initialScale;        // ���̃X�P�[��

    public void SetMaxScale(float _max) { maxScale = _max; }
    public void SetSpeed(float _speed) { speed = _speed; }

    public override void Spawned()
    {
        // ���̃X�P�[����ۑ�
        initialScale = transform.localScale;
    }

    public override void FixedUpdateNetwork()
    {
      
    }

    public override void Render()
    {
        // ���݂̃X�P�[��
        float currentScale = transform.localScale.x * 1.5f;

        // �X�P�[�����X�V
        currentScale += scaleDirection * speed * Time.deltaTime;

        // �X�P�[�����ő�܂��͍ŏ��ɒB����������𔽓]
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

        // �X�P�[����K�p
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
