using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShareNumbers : NetworkBehaviour
{
    [Networked] public int CurrentHP { get; set; }
    [Networked] public int nCombo { get; set; }
    [Networked] public int maxCombo { get; set; }

    public void AddCombo()
    {
        nCombo++;
        if (nCombo >= maxCombo)
        {
            nCombo = 0;
        }
        Debug.Log("òAåÇêî:" + nCombo);
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
