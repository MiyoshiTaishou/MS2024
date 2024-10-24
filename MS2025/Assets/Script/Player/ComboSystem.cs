using Fusion;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ComboSystem : NetworkBehaviour
{
    TextMeshProUGUI text;
    [SerializeField, Tooltip("�R���{��������")] 
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
        Debug.Log("�R���{��" + Combo+"�˂��Ƃ̕�:"+sharenum.nCombo);
    }

    ShareNumbers sharenum;

    public override void Spawned()
    {
        sharenum=GetComponent<ShareNumbers>();
        Combo = 0;
        Count = 0;

        GameObject obj = GameObject.Find("MainGameUI");
        if(obj == null)
        {
            Debug.LogError("てきすとああああないよ");
        }
        obj = obj.transform.Find("Combo").gameObject;
        if (obj == null)
        {
            Debug.LogError("てきすといいいいないよ");
        }
        obj = obj.transform.Find("ComboCount").gameObject;
        if (obj == null)
        {
            Debug.LogError("てきすとううううないよ");
        }
        text = obj.GetComponent<TextMeshProUGUI>();
        if(!text)
        {
            Debug.LogError("てきすとないよ");
        }
        text.text = "ああああ";
    }

    public override void FixedUpdateNetwork()
    {
        Combo = sharenum.nCombo;
        Count--;

        text.alpha = Count / ComboKeepframe;

        if(Count <= 0) 
        {
            Combo = 0;
            sharenum.nCombo = Combo;
        }
    }
}
