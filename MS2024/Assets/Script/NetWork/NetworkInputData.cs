using Fusion;
using UnityEngine;

public struct NetworkInputData : INetworkInput
{
    public Vector3 direction;
    public NetworkButtons buttons;
}

// 入力のボタンの種類は、列挙型(enum)で定義しておく
public enum NetworkInputButtons
{
    Jump
}
