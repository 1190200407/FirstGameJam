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
    [Header("ˮ������")]
    public float waterGravity = 1f;
    [Header("ˮ�ε�����ʱ��")]
    public float waterSurviveTime = 5f;
    [Header("�ȴ������뿪ʼ����")]
    public float waterWaitForSpeedUpTime = 1f;
    [Header("�ȴ������뿪ʼ����")]
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
