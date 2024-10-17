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

            // �J�����̃^�[�Q�b�g���擾
            RenderTargetIdentifier cameraColorTarget = renderingData.cameraData.renderer.cameraColorTarget;

            // �J�����̃^�[�Q�b�g�𑜓x���擾
            RenderTextureDescriptor opaqueDesc = renderingData.cameraData.cameraTargetDescriptor;
            opaqueDesc.depthBufferBits = 0;
            cmd.GetTemporaryRT(temporaryRT.id, opaqueDesc);

            // �s�N�Z�����̃G�t�F�N�g��K�p
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
            // cameraColorTarget�𒼐ڎQ�Ƃ����A�����_�����O�f�[�^���Ŏ擾
            renderer.EnqueuePass(pixelateRenderPass);
        }
    }
}
