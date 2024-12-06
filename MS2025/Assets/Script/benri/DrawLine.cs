using Fusion;
using UnityEngine;

public class DrawLine : NetworkBehaviour
{
    public Transform Startobj; // ���̎n�_�I�u�W�F�N�g
    public Transform Endobj; // ���̏I�_�I�u�W�F�N�g

    private LineRenderer lineRenderer;

    [SerializeField] float Width = 0.1f;


    [SerializeField,Networked] bool isTanuki { get; set; } = true;

    [SerializeField] Color TanukiColor;
    [SerializeField] Color kituneColor;



    [SerializeField] Material material;

    public void SetTanuki(bool _isTanuki) { isTanuki = _isTanuki; }

    public override void Spawned()
    {
        lineRenderer = GetComponent<LineRenderer>();

        // ���̊�{�ݒ�
        lineRenderer.positionCount = 2; // ���̓_��
        lineRenderer.startWidth = Width; // ���̎n�_��
        lineRenderer.endWidth = Width;   // ���̏I�_��
        lineRenderer.useWorldSpace = true;

    }

    public override void FixedUpdateNetwork()
    {
        // ���̎n�_�ƏI�_���X�V
        lineRenderer.SetPosition(0, Startobj.position);
        lineRenderer.SetPosition(1, Endobj.position);

        //Debug.Log("�X�^�[�g"+ Startobj.position + "�I�_"+ Endobj.position);
    }

    public override void Render()
    {
        if (isTanuki)
        {
            // Emission�J���[��ݒ�
            material.SetColor("_EmissionColor", TanukiColor);
            material.color = TanukiColor;
        }
        else 
        {
            // Emission�J���[��ݒ�
            material.SetColor("_EmissionColor", kituneColor);
            material.color = kituneColor;
        }
    }
}
