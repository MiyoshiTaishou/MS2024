using Fusion;
using UnityEngine;

public struct NetworkInputData : INetworkInput
{
    public Vector3 direction;
    public NetworkButtons buttons;
}

// ���͂̃{�^���̎�ނ́A�񋓌^(enum)�Œ�`���Ă���
public enum NetworkInputButtons
{
    Jump
}
