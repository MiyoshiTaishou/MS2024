using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkStatManager : NetworkBehaviour
{
    /// <summary>
    /// ���������l�������Ă��鎫��
    /// </summary>
    private Dictionary<string, INetworkStat> stats = new Dictionary<string, INetworkStat>();

    /// <summary>
    /// ���̏������ĂԂƎ����ɓ��������l���ǉ������
    /// </summary>
    /// <param name="key"></param>
    /// <param name="initialValue"></param>
    /// <param name="type"></param>
    public void AddStat(string key, object initialValue, string type)
    {
        if (stats.ContainsKey(key)) return;

        INetworkStat newStat = type switch
        {
            "int" => new IntNetworkStat(),
            "float" => new FloatNetworkStat(),
            _ => null
        };

        if (newStat != null)
        {
            newStat.Initialize(key, initialValue);
            stats[key] = newStat;
        }
        else
        {
            Debug.LogError("Unsupported type");
        }
    }

    public void SetStatValue(string key, object newValue)
    {
        if (stats.ContainsKey(key))
        {
            stats[key].SetValue(newValue);
        }
    }

    public object GetStatValue(string key)
    {
        if (stats.ContainsKey(key))
        {
            return stats[key].GetValue();
        }
        return null;
    }
}