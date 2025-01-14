using UnityEngine;

public abstract class BossActionData : ScriptableObject
{
    public string actionName; // アクション名

    public abstract void InitializeAction(GameObject boss,Transform player);
    public abstract bool ExecuteAction(GameObject boss, Transform player);
}