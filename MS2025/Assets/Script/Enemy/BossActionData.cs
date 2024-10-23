using UnityEngine;

public abstract class BossActionData : ScriptableObject
{
    public string actionName; // ƒAƒNƒVƒ‡ƒ“–¼

    public abstract void InitializeAction(GameObject boss,Transform player);
    public abstract bool ExecuteAction(GameObject boss);
}