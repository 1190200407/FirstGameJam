using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

[Serializable, VolumeComponentMenu("Custom_PostEffect/PostLightMask")]
public class PostLightMask : CustomPostBase
{
    public static new int priority = 1;

    public VolumeParameter<RenderTexture> lightTexture = new VolumeParameter<RenderTexture>();
    public VolumeParameter<RenderTexture> innerWorldTexture = new VolumeParameter<RenderTexture>();

    public const string ShaderName = "Custom/WorldBlendPostProcessing";
    private Material renderMat;

    public Material renderMaterial
    {
        get
        {
            if (renderMat == null)
                renderMat = new Material(Shader.Find(ShaderName));
            return renderMat;
        }
    }

    public override void DoRenderCmd(ref RTHandle source, ref CommandBuffer cmd, ref RenderingData renderingData)
    {
        CameraData cameraData = renderingData.cameraData;
        int width = cameraData.cameraTargetDescriptor.width;
        int height = cameraData.cameraTargetDescriptor.height;

        if (lightTexture.value != null && innerWorldTexture.value != null)
        {
            // 设置材质的纹理参数
            renderMaterial.SetTexture("_LightMask", lightTexture.value);
            renderMaterial.SetTexture("_InnerWorldTex", innerWorldTexture.value);

            // 创建临时渲染目标
            int tempTarget = Shader.PropertyToID("_TempTarget");
            cmd.GetTemporaryRT(tempTarget, width, height, 0, FilterMode.Bilinear);

            // 执行后处理 Blit
            cmd.Blit(source, tempTarget, renderMaterial);

            // 将结果重新写回原始渲染目标
            cmd.Blit(tempTarget, source);

            // 释放临时渲染目标
            cmd.ReleaseTemporaryRT(tempTarget);
        }
    }

    public override bool IsActive() => lightTexture.value != null && innerWorldTexture.value != null && renderMaterial != null;

    public override bool IsTileCompatible() => false;
}
