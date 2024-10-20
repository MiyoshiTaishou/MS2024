using UnityEngine;

public abstract class BossActionData : ScriptableObject
{
    public string actionName; // アクション名

    public abstract void InitializeAction(GameObject boss);
    public abstract bool ExecuteAction(GameObject boss);
}