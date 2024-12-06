using Fusion;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ComboSystem : NetworkBehaviour
{
    GameObject Combonum;

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

    GameObject change;

    public void AddCombo() 
    {
        Combo++;
        sharenum.nCombo = Combo;
        if(sharenum.nCombo > sharenum.maxCombo)
        {
            sharenum.maxCombo = sharenum.nCombo;
        }
        Count = ComboKeepframe;
        //text.text=Combo.ToString();
        Debug.Log("コンボ数" + Combo+"ガチのコンボ数"+sharenum.nCombo);
        //change.GetComponent<ChangeBossAction>().combo = Combo;
        //change.GetComponent<ChangeBossAction>().RPC_Cange();
        if (Combo>=1&&change.GetComponent<ChangeBossAction>().TextNo == 0)
        {
            change.GetComponent<ChangeBossAction>().TextNo = 1;
        }

    }

    ShareNumbers sharenum;

    public override void Spawned()
    {
        sharenum=GetComponent<ShareNumbers>();
        Combo = 0;
        Count = 0;
        change = GameObject.Find("ChangeAction");
        GameObject obj = GameObject.Find("MainGameUI");
        GameObject obj2;
        GameObject obj3;
        if(obj == null)
        {
            Debug.LogError("てきすとああああないよ");
        }
        Combonum = obj.transform.Find("Combo").gameObject;
        if (obj == null)
        {
            Debug.LogError("てきすといいいいないよ");
        }
        obj3 = Combonum.transform.Find("ComboImage").gameObject;
        obj2 = Combonum.transform.Find("Count").gameObject;
        //Combonum = obj.transform.Find("Combo").gameObject;

        if (Combonum == null)
        {
            Debug.LogError("てきすとううううないよ");
        }
        Number1 = Combonum.transform.Find("1").GetComponent<Image>();
        Number2 = Combonum.transform.Find("10").GetComponent<Image>();
        Number3 = Combonum.transform.Find("100").GetComponent<Image>();

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

    public override void Render()
    {
        Combo = sharenum.nCombo;
        if (Count > 0)
        {
            Count--;
        }

        if(Combo >0)
        {
            Color color = image.color;
            Combonum.GetComponent<NumberChange>().DisplayNumber(sharenum.nCombo);
            color.a = (float)Count / ComboKeepframe;
            Number1.color = color;
            Number2.color = color;
            Number3.color = color;
            text2.color = color;
            image.color = color;
        }

        if(Count <= 0) 
        {
            Combo = 0;
            sharenum.nCombo = Combo;
        }
    }
}
