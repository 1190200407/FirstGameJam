using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class Enemy : MonoBehaviour
{
    public Volume globalVolume;
    public float effectDistance = 10f; // 开始变暗的距离
    public float dieDistance = 1f;     // 最暗的距离
    private AIPath path;
    private LiftGammaGain liftGammaGain;
    private bool hasLiftGammaGain;

    void Awake()
    {
        globalVolume = FindObjectOfType<Volume>();

        // 尝试获取 Volume 里的 LiftGammaGain 组件
        if (globalVolume.profile.TryGet(out liftGammaGain))
        {
            hasLiftGammaGain = true;
        }

        path = GetComponent<AIPath>(); // 获取 AIPath 组件
    }

    void Update()
    {
        if (!hasLiftGammaGain) return; // 如果没有找到 LiftGammaGain，直接返回

        // 获取敌人到目标（通常是玩家）的剩余距离
        float remainingDistance = path.remainingDistance;

        // 计算 Gain 的新值
        if (remainingDistance <= dieDistance)
        {
            liftGammaGain.gain.Override(new Vector4(0, 0, 0, 0));
            LevelManager.Instance.input.enabled = false;
            StartCoroutine(RestartCoroutine());
        }
        else if (remainingDistance <= effectDistance)
        {
            // 根据距离线性插值 Gain 的值
            float t = Mathf.InverseLerp(effectDistance, dieDistance, remainingDistance);
            float newGain = Mathf.Lerp(1f, 0.2f, t); // 1表示正常亮度，0表示最暗

            // 设置 LiftGammaGain 的 Gain 参数，RGB 通道都使用同样的值
            liftGammaGain.gain.Override(new Vector4(newGain, newGain, newGain, 1));
        }
        else
        {
            liftGammaGain.gain.Override(new Vector4(1, 1, 1, 1));
        }
    }

    private IEnumerator RestartCoroutine(float waitTime = 1f)
    {
        yield return new WaitForSecondsRealtime(waitTime);
        LevelManager.Instance.Restart();
    }
}
