using Fusion;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using static System.Net.Mime.MediaTypeNames;

public class OnFadeInText : NetworkBehaviour
{

    private float alpha;             //パネルのalpha値取得変数
    private bool fadein;          //フェードインのフラグ用変数
    public int Count;
    public GameObject Boss;
    private bool alphaCheck;

    // Start is called before the first frame update
    void Start()
    {

        alpha = this.GetComponent<UnityEngine.UI.Text>().color.a;
    }

    void Update()
    {
        if (Count < 50)
        {
            Count++;
        }
        else if (alphaCheck == false && Count == 50)
        {
            FadeOut();
        }
        else if (alphaCheck == true)
        {
            Time.timeScale = 0.0f;
            FadeIn();
        }

        if(Count==10)
        {
            Boss.GetComponent<BossAI>().enabled = false;
        }
    }

    public override void Render()
    {

    }

    void FadeIn()
    {
        Color color = this.GetComponent<UnityEngine.UI.Text>().color;
        Color outcolor = this.GetComponent<UnityEngine.UI.Outline>().effectColor;
        color.a = color.a <= 0 ? 1 : color.a - 0.01f;
        outcolor.a = outcolor.a <= 0 ? 1 : outcolor.a - 0.01f;
        this.GetComponent<UnityEngine.UI.Text>().color = color;
        this.GetComponent<UnityEngine.UI.Outline>().effectColor = outcolor;
        if (color.a <= 0)
        {
            Boss.GetComponent<TutoriarBossAI>().enabled = true;
            Time.timeScale = 1.0f;
            this.gameObject.SetActive(false);

        }
    }

    void FadeOut()
    {
        Color color = this.GetComponent<UnityEngine.UI.Text>().color;
        Color outcolor = this.GetComponent<UnityEngine.UI.Outline>().effectColor;
        color.a = color.a <= 0 ? 1 : color.a + 0.02f;
        outcolor.a = outcolor.a <= 0 ? 1 : outcolor.a + 0.02f;
        this.GetComponent<UnityEngine.UI.Text>().color = color;
        this.GetComponent<UnityEngine.UI.Outline>().effectColor = outcolor;
        if (color.a >= 1)
        {
            alphaCheck = true;


        }
    }
}
