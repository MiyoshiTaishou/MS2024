using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class DotCamera : MonoBehaviour
{
    public Camera targetCamera;
    public UniversalRenderPipelineAsset urpAsset;

    // Rendererの番号を設定 (URPアセット内での順番)
    public int rendererIndex1 = 0;
    public int rendererIndex2 = 1;

    void Update()
    {
        // 例としてキー入力でRendererを切り替え
        if (Input.GetKeyDown(KeyCode.M))
        {
            targetCamera.GetUniversalAdditionalCameraData().SetRenderer(rendererIndex1);
        }
        else if (Input.GetKeyDown(KeyCode.N))
        {
            targetCamera.GetUniversalAdditionalCameraData().SetRenderer(rendererIndex2);
        }
    }
}
