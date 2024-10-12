using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ステートの親クラス
/// </summary>
public abstract class BaseState : NetworkBehaviour
{
    public abstract void EnterState(PlayerState state);
    public abstract void UpdateState(PlayerState state);
    public abstract void ExitState(PlayerState state);
}
