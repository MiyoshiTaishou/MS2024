using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PixelateRenderFeature : ScriptableRendererFeature
{
    class PixelateRenderPass : ScriptableRenderPass
    {
        private Material pixelateMaterial;
        private RenderTargetHandle temporaryRT;

        public PixelateRenderPass(Material material)
        {
            this.pixelateMaterial = material;
            temporaryRT.Init("_TemporaryPixelateRT");
        }

        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            CommandBuffer cmd = CommandBufferPool.Get("Pixelate Effect");

            // カメラのターゲットを取得
            RenderTargetIdentifier cameraColorTarget = renderingData.cameraData.renderer.cameraColorTarget;

            // カメラのターゲット解像度を取得
            RenderTextureDescriptor opaqueDesc = renderingData.cameraData.cameraTargetDescriptor;
            opaqueDesc.depthBufferBits = 0;
            cmd.GetTemporaryRT(temporaryRT.id, opaqueDesc);

            // ピクセル化のエフェクトを適用
            Blit(cmd, cameraColorTarget, temporaryRT.Identifier(), pixelateMaterial);
            Blit(cmd, temporaryRT.Identifier(), cameraColorTarget);

            context.ExecuteCommandBuffer(cmd);
            CommandBufferPool.Release(cmd);
        }

        public override void FrameCleanup(CommandBuffer cmd)
        {
            cmd.ReleaseTemporaryRT(temporaryRT.id);
        }
    }

    [System.Serializable]
    public class PixelateSettings
    {
        public Material pixelateMaterial = null;
    }

    public PixelateSettings settings = new PixelateSettings();
    private PixelateRenderPass pixelateRenderPass;

    public override void Create()
    {
        pixelateRenderPass = new PixelateRenderPass(settings.pixelateMaterial)
        {
            renderPassEvent = RenderPassEvent.AfterRenderingTransparents
        };
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        if (settings.pixelateMaterial != null)
        {
            // cameraColorTargetを直接参照せず、レンダリングデータ内で取得
            renderer.EnqueuePass(pixelateRenderPass);
        }
    }
}
