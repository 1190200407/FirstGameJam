using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

/// <summary>
/// ˮ������Ŀ�����
/// </summary>
public class WaterCtr : MonoBehaviour
{
    public bool IsOpen { get; private set; }

    [Header("ˮ��������")]
    public WaterRenderer waterRenderer;
    [Header("����ˮ�εĸ�����")]
    public GameObject waterParent;

    [Header("ˮ�ε�Ԥ����")]
    public GameObject waterPrefab;
    [Header("ˮ�����ɵ�ʱ����")]
    public float waterMakingInternal = 0.1f;
    [Header("ˮ�μ��ٶ�")]
    public float waterAcceleration = 0.05f;
    [Header("ˮ����С���ٶ�")]
    public float waterMinInitialSpeed = 5f;
    [Header("ˮ�������ٶ�")]
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
