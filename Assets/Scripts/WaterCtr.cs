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
    [Header("水滴重力")]
    public float waterGravity = 1f;
    [Header("水滴的生存时间")]
    public float waterSurviveTime = 5f;
    [Header("等待多少秒开始加速")]
    public float waterWaitForSpeedUpTime = 1f;
    [Header("等待多少秒开始减速")]
    public float waterWaitForSpeedDownTime = 0.2f;

    public float WaterInitialSpeed {  get; private set; }
    public Vector3 WaterDir => GetComponent<SpriteRenderer>().flipX ? -transform.right : transform.right;

    private float acc = 0f;

    private void Start()
    {
        WaterInitialSpeed = waterMinInitialSpeed;
    }

    public IEnumerator MakeWater()
    {
        while (IsOpen && !GameCtr.instance.energyBar.isEmpty)
        {
            GameObject newDrop = Instantiate(waterPrefab, transform.position, Quaternion.identity, waterParent.transform);
            newDrop.GetComponent<Water>().surviveTime = waterSurviveTime;
            newDrop.GetComponent<Water>().SetSpeed(WaterInitialSpeed, WaterDir);
            newDrop.GetComponent<Rigidbody2D>().gravityScale = waterGravity;
            yield return new WaitForSeconds(waterMakingInternal);
        }
    }

    public IEnumerator WaterSpeedUp()
    {
        yield return new WaitForSeconds(waterWaitForSpeedUpTime);

        acc = 0f;
        while (IsOpen)
        {
            yield return null;
            acc += Time.deltaTime * waterAcceleration;
            WaterInitialSpeed = Mathf.Min(WaterInitialSpeed + acc * Time.deltaTime, waterMaxInitialSpeed);
        }
    }

    public IEnumerator WaterSpeedDown()
    {
        yield return new WaitForSeconds(waterWaitForSpeedDownTime);

        while (!IsOpen)
        {
            yield return null;
            acc = Mathf.Max(acc - Time.deltaTime * waterAcceleration, 0.1f);
            WaterInitialSpeed = Mathf.Max(WaterInitialSpeed - acc * Time.deltaTime, waterMinInitialSpeed);
        }
    }

    public void StartPouring()
    {
        IsOpen = true;
        StopAllCoroutines();
        StartCoroutine(MakeWater());
        StartCoroutine(WaterSpeedUp());
    }

    public void EndPouring()
    {
        IsOpen = false;
        StopAllCoroutines();
        StartCoroutine(WaterSpeedDown());
    }
}
