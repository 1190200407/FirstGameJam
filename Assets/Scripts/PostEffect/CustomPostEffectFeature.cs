using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class CustomPostEffectFeature : ScriptableRendererFeature
{
    public CustomPostFeatureSetting settings = new CustomPostFeatureSetting();    

    private CustomPostEffectPass effectPass;

    private List<Type> customPostTypeList = new List<Type>();

    public override void Create()
    {
        customPostTypeList = GetAllCustomType();
        effectPass = new CustomPostEffectPass(customPostTypeList, settings); 
    }

    private List<Type> GetAllCustomType()
    {
        IEnumerable<Type> customTypes = Assembly.GetExecutingAssembly().GetTypes().Where((t) => {
            return t.BaseType == typeof(CustomPostBase);
        });

        List<Type> rst = customTypes.ToList();
        rst.Sort((t1, t2) => {
            FieldInfo p1 = t1.GetField("priority", BindingFlags.Static | BindingFlags.Public);
            FieldInfo p2 = t2.GetField("priority", BindingFlags.Static | BindingFlags.Public);
            int v1 = p1 == null ? CustomPostBase.priority : (int)p1.GetValue(null);
            int v2 = p2 == null ? CustomPostBase.priority : (int)p2.GetValue(null);
            return v1 - v2;
        });
        return rst;
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        renderer.EnqueuePass(effectPass);
        effectPass.GetTempRT(in renderingData);
    }

    public override void SetupRenderPasses(ScriptableRenderer renderer, in RenderingData renderingData)
    {
        //获取主图像
        effectPass.SetUP(renderer.cameraColorTargetHandle);
    }
}

[Serializable]
public class CustomPostFeatureSetting
{
    public RenderPassEvent passEvent;
    public Material material;
}