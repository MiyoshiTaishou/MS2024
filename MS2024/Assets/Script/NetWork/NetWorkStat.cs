using Fusion;
using UnityEngine;

public interface INetworkStat
{
    void Initialize(string key, object initialValue);
    void SetValue(object newValue);
    object GetValue();
}

public class IntNetworkStat : NetworkBehaviour, INetworkStat
{
    [Networked] public int Value { get; set; }

    public void Initialize(string key, object initialValue)
    {
        Value = (int)initialValue;
    }

    public void SetValue(object newValue)
    {
        if (Object.HasStateAuthority)
        {
            Value = (int)newValue;
        }
    }

    public object GetValue()
    {
        return Value;
    }
}

public class FloatNetworkStat : NetworkBehaviour, INetworkStat
{
    [Networked] public float Value { get; set; }

    public void Initialize(string key, object initialValue)
    {
        Value = (float)initialValue;
    }

    public void SetValue(object newValue)
    {
        if (Object.HasStateAuthority)
        {
            Value = (float)newValue;
        }
    }

    public object GetValue()
    {
        return Value;
    }
}
