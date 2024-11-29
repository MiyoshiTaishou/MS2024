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

    private float alpha;             //�p�l����alpha�l�擾�ϐ�
    private bool fadein;          //�t�F�[�h�C���̃t���O�p�ϐ�
    private int Count;

    // Start is called before the first frame update
    void Start()
    {
       
        alpha = this.GetComponent<UnityEngine.UI.Text>().color.a;
    }

    void Update()
    {
       if(Count<60)
        {
            Count++;
        }
        else 
        {
            Time.timeScale = 0.0f;
            FadeIn();
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
            Time.timeScale = 1.0f;
            this.gameObject.SetActive(false);
        }
    }
}
