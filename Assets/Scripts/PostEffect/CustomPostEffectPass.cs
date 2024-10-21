using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class CustomPostEffectPass : ScriptableRenderPass
{
    public const string ProfileName = "Custom Post Processing";
    ProfilingSampler _profilingSampler = new ProfilingSampler(ProfileName);

    private List<Type> customPostTypes;           

    RTHandle _cameraColorTgt;
    RTHandle _tempRT;
    
    public Material material;

    public CustomPostEffectPass(List<Type> types, CustomPostFeatureSetting setting)
    {
        this.renderPassEvent = setting.passEvent;
        customPostTypes = types;
        material = setting.material;
    }   

    public void GetTempRT(in RenderingData data) {
        RenderingUtils.ReAllocateIfNeeded(ref _tempRT, data.cameraData.cameraTargetDescriptor);
    }
    public void SetUP(RTHandle cameraColor) {
        _cameraColorTgt = cameraColor;
    }
    public override void OnCameraSetup(CommandBuffer cmd, ref RenderingData renderingData) {
        ConfigureInput(ScriptableRenderPassInput.Depth | ScriptableRenderPassInput.Normal | ScriptableRenderPassInput.Color);
        ConfigureTarget(_cameraColorTgt);
    }

    public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
    {
        if (renderingData.cameraData.renderType != CameraRenderType.Base)
            return;

        VolumeStack stack = VolumeManager.instance.stack;        
        CommandBuffer cmd = CommandBufferPool.Get(ProfileName);     
        using (new ProfilingScope(cmd, _profilingSampler))
        {
            foreach (Type type in customPostTypes)
            {
                CoreUtils.SetRenderTarget(cmd, _tempRT);

                CustomPostBase customPost = stack.GetComponent(type) as CustomPostBase;
                if (customPost != null 
                    && customPost.IsActive() 
                    && customPost.isRender.value 
                    && customPost.renderPassEvent == this.renderPassEvent)
                {
                    DoRender(customPost, _cameraColorTgt, ref cmd, ref renderingData);
                }
            }
        } 

        context.ExecuteCommandBuffer(cmd);
        cmd.Clear();
        cmd.Dispose();
    }

    public void DoRender(CustomPostBase customPost, RTHandle source, ref CommandBuffer cmd, ref RenderingData renderingData)
    {
        cmd.BeginSample(customPost.GetType().ToString());
        customPost.DoRenderCmd(ref source, ref cmd, ref renderingData);
        cmd.EndSample(customPost.GetType().ToString());        
    }

    public override void OnCameraCleanup(CommandBuffer cmd)
    {
        _tempRT?.Release();
    }
}