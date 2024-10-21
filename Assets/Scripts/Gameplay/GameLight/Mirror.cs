using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Mirror : MonoBehaviour
{
    public Dictionary<InteractableLight, InteractableLight> reflections = new Dictionary<InteractableLight, InteractableLight>();
    private ShadowCaster2D shadowCaster;
    public Transform cutPoint1;
    public Transform cutPoint2;

    private void Awake()
    {
        shadowCaster = GetComponent<ShadowCaster2D>();
    }

    public InteractableLight GetOrAddReflection(InteractableLight light)
    {
        //反射超过5次的光不能再反射
        if (light.reflectedCnt > 5) return null;
        if (reflections.ContainsKey(light)) return reflections[light];

        InteractableLight newLight = Instantiate(light, light.transform.parent);
        newLight.isMirrorLight = true;
        newLight.reflectedLight = light;
        newLight.reflectedCnt = light.reflectedCnt + 1;
        newLight.reflectMirror = this;

        reflections.Add(light, newLight);
        return newLight;
    }

    void OnApplicationQuit()
    {
        foreach (var entry in reflections)
        {
            DestroyImmediate(entry.Value);
        }
    }
}
