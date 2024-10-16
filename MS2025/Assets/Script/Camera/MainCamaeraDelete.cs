using Fusion;
using UnityEngine;

public class MainCamaeraDelete : NetworkBehaviour
{
    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    public void RPC_DisableMainCamera()
    {
        // メインカメラを無効化
        if (Camera.main != null)
        {
            Camera.main.gameObject.SetActive(false);
        }
    }
}
