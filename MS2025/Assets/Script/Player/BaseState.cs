using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �X�e�[�g�̐e�N���X
/// </summary>
public abstract class BaseState : NetworkBehaviour
{
    public abstract void EnterState(PlayerState state);
    public abstract void UpdateState(PlayerState state);
    public abstract void ExitState(PlayerState state);
}
