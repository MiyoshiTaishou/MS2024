using UnityEngine;

public class FloatingMovement : MonoBehaviour
{
    [SerializeField] private float horizontalAmplitude = 1.0f; // ���h��̕�
    [SerializeField] private float horizontalFrequency = 1.0f; // ���h��̑���
    [SerializeField] private float verticalSpeed = 1.0f; // �㏸�̑���
    [SerializeField] private float fadeDuration = 2.0f; // ���S�ɓ����ɂȂ�܂ł̎���

    private Vector3 initialPosition;
    private SpriteRenderer spriteRenderer;
    private float time;
    private float fadeTimer = 0;

    void Start()
    {
        // �����ʒu��ۑ�
        initialPosition = transform.position;
        time = Time.time;

        // SpriteRenderer���擾
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            Debug.LogError("SpriteRenderer���A�^�b�`����Ă��܂���B");
        }
    }

    void Update()
    {
        // ���ԂɊ�Â��ĉ��h��Ə㏸�̌v�Z
        float horizontalOffset = Mathf.Sin(Time.time * horizontalFrequency) * horizontalAmplitude;
        float verticalOffset = (Time.time - time) * verticalSpeed;

        // �V�����ʒu��ݒ�
        transform.position = new Vector3(
            initialPosition.x + horizontalOffset,
            initialPosition.y + verticalOffset,
            initialPosition.z
        );

        // ���X�ɓ����ɂ���
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

            // ���S�ɓ����ɂȂ�����I�u�W�F�N�g���폜
            if (alpha <= 0.0f)
            {
                Destroy(gameObject);
            }
        }
    }
}
