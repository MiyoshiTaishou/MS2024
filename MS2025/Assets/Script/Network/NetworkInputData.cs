using Fusion;
using UnityEngine;

public struct NetworkInputData : INetworkInput
{
    public Vector3 Direction;
    public NetworkButtons Buttons;
}

// ���͂̃{�^���̎�ނ́A�񋓌^(enum)�Œ�`���Ă���
public enum NetworkInputButtons
{
    Jump,
    Attack,
    Parry,
    Special,
}