using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : NetworkBehaviour
{
    private BaseState currentState;  

    public override void Spawned()
    {
        //currentState = new PlayerMove();
        currentState.EnterState(this);
    }

    public override void FixedUpdateNetwork()
    {
        Debug.Log(currentState.ToString());
        currentState.UpdateState(this);
    }

    public void ChangeState(BaseState state)
    {
        currentState.ExitState(this);

        currentState = state;
        currentState.EnterState(this);
    }
}
