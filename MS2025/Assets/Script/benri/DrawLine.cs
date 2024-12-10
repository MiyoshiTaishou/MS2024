using Fusion;
using UnityEngine;

public class DrawLine : NetworkBehaviour
{
    [Networked] public Vector3 Startobj { get; set; } // ���̎n�_�I�u�W�F�N�g
    [Networked] public Vector3 Endobj { get; set; } // ���̏I�_�I�u�W�F�N�g

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


        //Debug.Log("�X�^�[�g"+ Startobj.position + "�I�_"+ Endobj.position);
    }

    public override void Render()
    {
        // ���̎n�_�ƏI�_���X�V
        lineRenderer.SetPosition(0, Startobj);
        lineRenderer.SetPosition(1, Endobj);
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
