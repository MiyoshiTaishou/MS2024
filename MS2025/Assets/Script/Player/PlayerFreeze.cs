using Fusion;
using UnityEngine;

public class PlayerFreeze : NetworkBehaviour
{
    [Networked] private bool isFreeze { get; set; }
    public bool GetIsFreeze() { return isFreeze; }
    [Networked] private int freezeFrame { get; set; }

    // Start is called before the first frame update
    public override void Spawned()
    {
    }

    public override void FixedUpdateNetwork()
    {
        if(freezeFrame > 0) 
        {
            freezeFrame--;
        }
        if(freezeFrame == 0)
        {
            isFreeze = false;
        }
    }

    public void Freeze(int _frame)
    {
        freezeFrame=_frame;
        isFreeze=true;
    }

}
