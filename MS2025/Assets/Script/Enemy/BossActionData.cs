using UnityEngine;

public abstract class BossActionData : ScriptableObject
{
    public string actionName; // �A�N�V������

    public abstract void InitializeAction(GameObject boss);
    public abstract bool ExecuteAction(GameObject boss);
}