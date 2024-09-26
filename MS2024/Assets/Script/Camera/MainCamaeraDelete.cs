using Fusion;
using UnityEngine;

public class MainCamaeraDelete : NetworkBehaviour
{
    [Rpc(RpcSources.All, RpcTargets.All)]
    public void RPC_DisableMainCamera()
    {
        // ���C���J�����𖳌���
        if (Camera.main != null)
        {
            Camera.main.gameObject.SetActive(false);
        }
    }
}
