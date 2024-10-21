using UnityEngine;

public abstract class BossActionData : ScriptableObject
{
    public string actionName; // ƒAƒNƒVƒ‡ƒ“–¼

    public abstract void InitializeAction(GameObject boss);
    public abstract bool ExecuteAction(GameObject boss);
}