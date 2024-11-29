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

    // Start is called before the first frame update
    void Start()
    {

        alpha = this.GetComponent<UnityEngine.UI.Text>().color.a;
    }

    void Update()
    {
        Count++;
       
        if(Count==5)
        {
            Time.timeScale = 0.0f;
        }
        if (Count>=FadeInCount&&Count<FadeOutCount&&alphaCheck==false) 
        {
            FadeIn();
        }
        if(Count>=FadeOutCount)
        {
            FadeOut();
        }
    }

    public override void Render()
    {

    }

    void FadeIn()
    {
        Color color = this.GetComponent<UnityEngine.UI.Text>().color;
        Color outcolor = this.GetComponent<UnityEngine.UI.Outline>().effectColor;
        color.a = color.a <= 0 ? 1 : color.a + 0.01f;
        outcolor.a = outcolor.a <= 0 ? 1 : outcolor.a + 0.01f;
        this.GetComponent<UnityEngine.UI.Text>().color = color;
        this.GetComponent<UnityEngine.UI.Outline>().effectColor = outcolor;

        if (color.a >= 1)
        {
            alphaCheck = true;
        }

       
    }
    void FadeOut()
    {
        Color color = this.GetComponent<UnityEngine.UI.Text>().color;
        Color outcolor = this.GetComponent<UnityEngine.UI.Outline>().effectColor;
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
