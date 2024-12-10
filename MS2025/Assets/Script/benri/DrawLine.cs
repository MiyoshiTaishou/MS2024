using Fusion;
using UnityEngine;

public class DrawLine : NetworkBehaviour
{
    [Networked] public Vector3 Startobj { get; set; } // 線の始点オブジェクト
    [Networked] public Vector3 Endobj { get; set; } // 線の終点オブジェクト

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

        // 線の基本設定
        lineRenderer.positionCount = 2; // 線の点数
        lineRenderer.startWidth = Width; // 線の始点幅
        lineRenderer.endWidth = Width;   // 線の終点幅
        lineRenderer.useWorldSpace = true;

    }

    public override void FixedUpdateNetwork()
    {


        //Debug.Log("スタート"+ Startobj.position + "終点"+ Endobj.position);
    }

    public override void Render()
    {
        // 線の始点と終点を更新
        lineRenderer.SetPosition(0, Startobj);
        lineRenderer.SetPosition(1, Endobj);
        if (isTanuki)
        {
            // Emissionカラーを設定
            material.SetColor("_EmissionColor", TanukiColor);
            material.color = TanukiColor;
        }
        else 
        {
            // Emissionカラーを設定
            material.SetColor("_EmissionColor", kituneColor);
            material.color = kituneColor;
        }
    }
}
