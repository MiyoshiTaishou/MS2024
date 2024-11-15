using Fusion;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Burst.Intrinsics;
using UnityEngine;
using UnityEngine.UI;

public class ComboSystem : NetworkBehaviour
{
    GameObject ComboCount;


    Image Number1;
    Image Number2;
    Image Number3;

    Image text2;
    Image image;
    [SerializeField, Tooltip("コンボ継続時間")] 
    int ComboKeepframe;
    [Networked] int Count { get; set; } = 0;
    public int GetCount() { return Count; }
    int Combo;
    public void AddCombo() 
    {
        Combo++;
        sharenum.nCombo = Combo;
        if(Combo>sharenum.maxCombo)
        {
            sharenum.maxCombo = Combo;
        }
        Count = ComboKeepframe;
        //text.text=Combo.ToString();
        Debug.Log("コンボ数" + Combo+"ガチのコンボ数"+sharenum.nCombo);
    }

    ShareNumbers sharenum;

    public override void Spawned()
    {
        sharenum=GetComponent<ShareNumbers>();
        Combo = 0;
        Count = 0;

        GameObject obj = GameObject.Find("MainGameUI");
        GameObject obj2;
        GameObject obj3;
        if(obj == null)
        {
            Debug.LogError("てきすとああああないよ");
        }
        obj = obj.transform.Find("Combo").gameObject;
        if (obj == null)
        {
            Debug.LogError("てきすといいいいないよ");
        }
        obj3 = obj.transform.Find("ComboImage").gameObject;
        obj2 = obj.transform.Find("Count").gameObject;
        ComboCount = obj.transform.Find("Combo").gameObject;

        if (ComboCount == null)
        {
            Debug.LogError("てきすとううううないよ");
        }
        Number1 = ComboCount.transform.Find("1").GetComponent<Image>();
        Number2 = ComboCount.transform.Find("10").GetComponent<Image>();
        Number3 = ComboCount.transform.Find("100").GetComponent<Image>();

        text2 = obj2.GetComponent<Image>();
        image = obj3.GetComponent<Image>();
        if(!Number1)
        {
            Debug.LogError("てきすとないよ");
        }
        Color color = image.color;
        color.a = 0;

        Number1.color = color;
        Number2.color = color;
        Number3.color = color;
        text2.color = color;
        image.color = color;
    }

    public override void FixedUpdateNetwork()
    {
        //Debug.Log("コンボカウント中α設定" + sharenum.nCombo);

        Combo = sharenum.nCombo;
        if (Count > 0)
        {
            Count--;
        }

        if (Combo > 0)
        {
            ComboCount.GetComponent<NumberChange>().DisplayNumber(sharenum.nCombo);
            Color color = image.color;
            color.a = (float)Count / ComboKeepframe;
            Debug.Log("カウント透明度" + color);

            Number1.color = color;
            Number2.color = color;
            Number3.color = color;
            text2.color = color;
            image.color = color;
        }

        if (Count <= 0)
        {
            Combo = 0;
            sharenum.nCombo = Combo;
        }
    }

    public override void Render()
    {
        if(Object.HasStateAuthority)
        {
            return;
        }
       // Debug.Log("コンボカウント中α設定クライアント" + sharenum.nCombo);


        Combo = sharenum.nCombo;
        if (Count > 0)
        {
            Count--;
        }

        if (Combo > 0)
        {
            ComboCount.GetComponent<NumberChange>().DisplayNumber(sharenum.nCombo);
            Color color = image.color;
            color.a = (float)Count / ComboKeepframe;
            Debug.Log("カウント透明度"+color);
            Number1.color = color;
            Number2.color = color;
            Number3.color = color;
            text2.color = color;
            image.color = color;
        }

        if (Count <= 0)
        {
            Combo = 0;
            sharenum.nCombo = Combo;
        }
    }
}
