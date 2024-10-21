using UnityEngine;

public abstract class BossActionData : ScriptableObject
{
    public abstract void InitializeAction(GameObject boss);
    public abstract bool ExecuteAction(GameObject boss);
}