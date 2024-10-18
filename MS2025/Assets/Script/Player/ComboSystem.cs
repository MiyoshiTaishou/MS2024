using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComboSystem : NetworkBehaviour
{
    [SerializeField, Tooltip("コンボ持続時間")] 
    int ComboKeepframe;
    int Count = 0;
    public int GetCount() { return Count; }
    int Combo;
    public void AddCombo() 
    {
        Combo++;
        sharenum.nCombo = Combo;
        Count = ComboKeepframe;
        Debug.Log("コンボ数" + Combo+"ねっとの方:"+sharenum.nCombo);
    }

    ShareNumbers sharenum;

    public override void Spawned()
    {
        sharenum=GetComponent<ShareNumbers>();
        Combo = 0;
        Count = 0;
    }

    public override void FixedUpdateNetwork()
    {
        Combo = sharenum.nCombo;
        Count--;
        if(Count <= 0) 
        {
            Combo = 0;
            sharenum.nCombo = Combo;
        }
    }
}
