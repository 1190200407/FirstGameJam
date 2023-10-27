using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

/// <summary>
/// 水流发射的控制器
/// </summary>
public class WaterCtr : MonoBehaviour
{
    public bool IsOpen { get; private set; }

    [Header("水流绘制器")]
    public WaterRenderer waterRenderer;
    [Header("所有水滴的父物体")]
    public GameObject waterParent;

    [Header("水滴的预制体")]
    public GameObject waterPrefab;
    [Header("水滴生成的时间间隔")]
    public float waterMakingInternal = 0.1f;
    [Header("水滴加速度")]
    public float waterAcceleration = 0.05f;
    [Header("水滴最小初速度")]
    public float waterMinInitialSpeed = 5f;
    [Header("水滴最大初速度")]
    public float waterMaxInitialSpeed = 10f;

    private float waterInitialSpeed = 0f;
    public Coroutine waterMakingProcess;

    public IEnumerator MakeWater()
    {
        while (IsOpen)
        {
            waterInitialSpeed = Mathf.Min(waterInitialSpeed + waterAcceleration, waterMaxInitialSpeed);
            GameObject newDrop = Instantiate(waterPrefab, transform.position, Quaternion.identity, waterParent.transform);
            newDrop.GetComponent<Water>().SetSpeed(waterInitialSpeed, transform.right);
            yield return new WaitForSeconds(waterMakingInternal);
        }
    }

    public void StartPouring()
    {
        IsOpen = true;
        waterMakingProcess = StartCoroutine(MakeWater());
    }

    public void EndPouring()
    {
        IsOpen = false;
        StopCoroutine(waterMakingProcess);
        waterInitialSpeed = waterMinInitialSpeed;
    }
}
