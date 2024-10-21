using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class LightMaskRenderFeature : ScriptableRendererFeature
{
    class LightMaskRenderPass : ScriptableRenderPass
    {
        private RenderTargetIdentifier source { get; set; }
        //private RenderTargetHandle tempTexture;
        private Material material;

        public LightMaskRenderPass(Material material)
        {
            this.material = material;
            //tempTexture.Init("_TempTexture");
        }

        public void Setup(RenderTargetIdentifier source)
        {
            this.source = source;
        }

        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            CommandBuffer cmd = CommandBufferPool.Get("LightMaskPass");
            RenderTextureDescriptor opaqueDesc = renderingData.cameraData.cameraTargetDescriptor;

            // Create a temporary render texture
            //cmd.GetTemporaryRT(tempTexture.id, opaqueDesc);

            // Apply the custom material (our shader)
            //Blit(cmd, source, tempTexture.Identifier(), material);
            //Blit(cmd, tempTexture.Identifier(), source);

            context.ExecuteCommandBuffer(cmd);
            CommandBufferPool.Release(cmd);
        }

        public override void FrameCleanup(CommandBuffer cmd)
        {
            //cmd.ReleaseTemporaryRT(tempTexture.id);
        }
    }

    [System.Serializable]
    public class Settings
    {
        public Material material = null;
    }

    public Settings settings = new Settings();
    LightMaskRenderPass renderPass;

    public override void Create()
    {
        renderPass = new LightMaskRenderPass(settings.material);
        renderPass.renderPassEvent = RenderPassEvent.AfterRenderingTransparents; // Choose when to execute the pass
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        //renderPass.Setup(renderer.cameraColorTarget);
        renderer.EnqueuePass(renderPass);
    }
}