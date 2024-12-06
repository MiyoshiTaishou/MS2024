using Fusion;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using static System.Net.Mime.MediaTypeNames;

public class FadeInText : NetworkBehaviour
{

    private float alpha;             //パネルのalpha値取得変数
    private bool fadein;          //フェードインのフラグ用変数
    public int Count;
    private bool alphaCheck;
    [SerializeField]int FadeInCount=50;
    private int FadeOutCount = 300;
    Color color;
    Color outcolor;

    // Start is called before the first frame update
    void Start()
    {
        color = this.GetComponent<UnityEngine.UI.Text>().color;
        outcolor = this.GetComponent<UnityEngine.UI.Outline>().effectColor;
        alpha = this.GetComponent<UnityEngine.UI.Text>().color.a;
    }
    public override void Render()
    {
        
    }


    private void Update()
    {
        Count++;
        if (Count == 5)
        {
            Time.timeScale = 0.0f;
        }
        if (Count >= FadeInCount && Count < FadeOutCount && alphaCheck == false)
        {
            FadeIn();
        }
        if (Count >= FadeOutCount)
        {
            FadeOut();
        }
    }

    public override void FixedUpdateNetwork()
    {
      
    
    }

    //[Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    void FadeIn()
    {

        color.a = color.a <= 0 ? 1 : color.a + 0.01f;
        outcolor.a = outcolor.a <= 0 ? 1 : outcolor.a + 0.01f;
        this.GetComponent<UnityEngine.UI.Text>().color = color;
        this.GetComponent<UnityEngine.UI.Outline>().effectColor = outcolor;

        if (color.a >= 1)
        {
            alphaCheck = true;
        }

       
    }

    //[Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    void FadeOut()
    {

        color.a = color.a <= 0 ? 1 : color.a - 0.01f;
        outcolor.a = outcolor.a <= 0 ? 1 : outcolor.a - 0.01f;
        this.GetComponent<UnityEngine.UI.Text>().color = color;
        this.GetComponent<UnityEngine.UI.Outline>().effectColor = outcolor;
        if (color.a <= 0)
        {
            Time.timeScale = 1.0f;
            this.gameObject.SetActive(false);

        }
    }
}
