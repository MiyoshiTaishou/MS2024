using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShareNumbers : NetworkBehaviour
{
    [Networked] public int CurrentHP { get; set; }
    [Networked] public int nCombo { get; set; }
    public int maxCombo { get; set; }

    public void AddCombo()
    {
        nCombo++;
        if (nCombo >= maxCombo)
        {
            nCombo = 0;
        }
        Debug.Log("連撃数:" + nCombo);
    }

    public override void Spawned()
    {
        maxCombo= 3;
        nCombo = 0;
        CurrentHP=3;
        Debug.Log("プレイヤーのHPとか初期化");
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
