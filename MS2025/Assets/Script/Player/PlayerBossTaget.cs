using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBossTaget : NetworkBehaviour
{


    [SerializeField] GameObject taget;
    [Networked,SerializeField] public bool isTaget { get; set; } = false;


    // Start is called before the first frame update
    public override void Spawned()
    {

    }

    // Update is called once per frame
    public override void Render()
    {
        if(isTaget)
        {
            taget.SetActive(true);
        }
        else
        {
            taget.SetActive(false);

        }
    }
}
