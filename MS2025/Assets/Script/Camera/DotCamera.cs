using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class DotCamera : MonoBehaviour
{
    public Camera targetCamera;
    public UniversalRenderPipelineAsset urpAsset;

    // Renderer�̔ԍ���ݒ� (URP�A�Z�b�g���ł̏���)
    public int rendererIndex1 = 0;
    public int rendererIndex2 = 1;

    void Update()
    {
        // ��Ƃ��ăL�[���͂�Renderer��؂�ւ�
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
