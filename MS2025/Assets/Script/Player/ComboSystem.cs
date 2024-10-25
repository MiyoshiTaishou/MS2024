using Fusion;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ComboSystem : NetworkBehaviour
{
    TextMeshProUGUI text;
    TextMeshProUGUI text2;
    Image image;
    [SerializeField, Tooltip("コンボ継続時間")] 
    int ComboKeepframe;
    int Count = 0;
    public int GetCount() { return Count; }
    int Combo;
    public void AddCombo() 
    {
        Combo++;
        sharenum.nCombo = Combo;
        Count = ComboKeepframe;
        text.text=Combo.ToString();
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
        obj2 = obj.transform.Find("ComboCountText").gameObject;
        obj = obj.transform.Find("ComboCount").gameObject;

        if (obj == null)
        {
            Debug.LogError("てきすとううううないよ");
        }
        text = obj.GetComponent<TextMeshProUGUI>();
        text2 = obj2.GetComponent<TextMeshProUGUI>();
        image = obj3.GetComponent<Image>();
        if(!text)
        {
            Debug.LogError("てきすとないよ");
        }
        text.alpha = 0;
        text2.alpha = 0;
        Color color = image.color;
        color.a = 0;
        image.color = color;
    }

    public override void FixedUpdateNetwork()
    {
        Combo = sharenum.nCombo;
        if (Count > 0)
        {
            Count--;
        }

        if(Combo >0)
        {
            text.alpha = (float)Count / ComboKeepframe;
            text2.alpha = (float)Count / ComboKeepframe;
            Color color = image.color;
            color.a = (float)Count / ComboKeepframe;
            image.color = color;
        }

        if(Count <= 0) 
        {
            Combo = 0;
            sharenum.nCombo = Combo;
        }
    }
}
