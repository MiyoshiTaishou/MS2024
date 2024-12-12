using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClientText : NetworkBehaviour
{
    [SerializeField] Image target;
    Image myimage;
    // Start is called before the first frame update
    public override void Spawned()
    {
        myimage = GetComponent<Image>();
    }

    // Update is called once per frame
    public override void FixedUpdateNetwork()
    {
        myimage.color = target.color;
    }
}
