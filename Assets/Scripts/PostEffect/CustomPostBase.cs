using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public abstract class CustomPostBase : VolumeComponent, IPostProcessComponent
{
    public static int priority = 1000;

    public RenderPassEvent renderPassEvent = RenderPassEvent.AfterRenderingPostProcessing;

    public BoolParameter isRender = new BoolParameter(false);

    public abstract bool IsActive();

    public abstract bool IsTileCompatible();

    public abstract void DoRenderCmd(ref RTHandle source, ref CommandBuffer cmd, ref RenderingData renderingData);
}