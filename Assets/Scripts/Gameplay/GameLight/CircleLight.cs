using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class CircleLight : InteractableLight
{
    public Transform lightStartPoint;

    private float _lightRadius = 2f;    // 光的半径
    public float LightRadius
    {
        set
        {
            _lightRadius = value;
            light2D.pointLightInnerRadius = _lightRadius;
            light2D.pointLightOuterRadius = _lightRadius;
            _edgeAngleStep = Mathf.Asin(2 * mergeThreshold / _lightRadius) * Mathf.Rad2Deg;
        }
    }

    [SerializeField] private float _edgeAngleStep = 1f;
    private float mergeThreshold = 0.05f;

    private Dictionary<Vector2, float> pointToAngle = new Dictionary<Vector2, float>(); // 点到角度的映射
    private List<Vector2> visibleDirections = new List<Vector2>();  // 可见方向列表
    private List<Vector2> tempPoints = new List<Vector2>();         // 临时保存可见点

    private float rotateAngle = 0.05f;  // 射线旋转角度
    
    void Start()
    {
        canReflect = false;
        light2D.lightType = Light2D.LightType.Point;
    }

    public override void UpdateLightPoints()
    {
        // 获取所有 ShadowCaster2D
        List<ShadowCaster2D> shadowCasters = LevelManager.Instance.ShadowCasters;

        // 1. 遍历所有 ShadowCaster2D 的多边形点，不进行范围检测
        Vector2 rayOrigin = lightStartPoint.position;
        pointToAngle.Clear();
        visibleDirections.Clear();
        Vector2 direction, intersection1, intersection2;
        
        ProcessDirection(Vector2.right, 0f);
        foreach (var shadowCaster in shadowCasters)
        {
            Vector3[] points = shadowCaster.shapePath;
            for (int i = 0; i < points.Length; i++)
            {
                Vector2 worldPoint = shadowCaster.transform.TransformPoint(points[i]);
                direction = (worldPoint - rayOrigin).normalized;

                if (MyMathUtil.IsPointInCircle(rayOrigin, _lightRadius, worldPoint))
                {
                    // 对每个点发射上下旋转一定角度的三条射线
                    ProcessDirection(direction, (Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + 360f) % 360f);
                }
                
                Vector2 worldPoint2 = shadowCaster.transform.TransformPoint(points[(i + 1) % points.Length]);
                int cnt = MyMathUtil.GetCircleLineIntersections(rayOrigin, _lightRadius, worldPoint, worldPoint2, out intersection1, out intersection2);
                if (cnt >= 1)
                {
                    direction = (intersection1 - rayOrigin).normalized;
                    ProcessDirection(direction, (Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + 360f) % 360f);
                }
                if (cnt == 2)
                {
                    direction = (intersection2 - rayOrigin).normalized;
                    ProcessDirection(direction, (Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + 360f) % 360f);
                }
            }
        }

        // 2. 对所有方向进行排序
        visibleDirections.Sort((p1, p2) => pointToAngle[p1].CompareTo(pointToAngle[p2]));

        // 3. 对每个方向发射射线，并记录击中的点
        bool hitBlock;
        tempPoints.Clear();
        // int j = 0;
        // Color[] colors = {Color.red, Color.yellow, Color.green, Color.cyan , Color.blue};
        for (int i = 0; i < visibleDirections.Count; i++)
        {
            //Debug.DrawLine(rayOrigin, rayOrigin + visibleDirections[i] * 20f, colors[(j++) % colors.Length]);
            Vector2 lightPoint = ScanRay(rayOrigin, visibleDirections[i], shadowCasters, out hitBlock);
            if (!hitBlock)
            {
                //如果没撞到墙，就到下一个判定点前画一段圆弧
                Vector2 dir = visibleDirections[i];
                float angle = pointToAngle[dir];
                float targetAngle = pointToAngle[visibleDirections[(i + 1) % visibleDirections.Count]];
                while (angle < targetAngle && angle < 360f)
                {
                    tempPoints.Add(rayOrigin + dir * _lightRadius);
                    dir = MyMathUtil.RotateVector(dir, _edgeAngleStep);
                    angle += _edgeAngleStep;
                }
            }
            else
                tempPoints.Add(lightPoint);
        }

        // 4. 合并靠近的点
        MergeClosePoints();
        
        if (points.Count < 3) return; 
        collider2D.SetPath(0, MyMathUtil.ConvertToVector2Array(points));
    }

    private void ProcessDirection(Vector2 dir, float angle)
    {
        Vector2 temp = MyMathUtil.RotateVector(dir, -rotateAngle);  // 向下旋转
        if (!pointToAngle.ContainsKey(temp))
        {
            visibleDirections.Add(temp);
            pointToAngle.Add(temp, angle >= rotateAngle ? angle - rotateAngle : angle - rotateAngle + 360f);
        }

        temp = dir;  // 原始方向
        if (!pointToAngle.ContainsKey(temp))
        {
            visibleDirections.Add(temp);
            pointToAngle.Add(temp, angle);
        }

        temp = MyMathUtil.RotateVector(dir, rotateAngle);  // 向上旋转
        if (!pointToAngle.ContainsKey(temp))
        {
            visibleDirections.Add(temp);
            pointToAngle.Add(temp, angle + rotateAngle <= 360f ? angle + rotateAngle : angle + rotateAngle - 360f);
        }
    }

    /// <summary>
    /// 发射射线判断结果，返回与遮挡物相交的最近点
    /// </summary>
    private Vector2 ScanRay(Vector2 rayOrigin, Vector2 rayDirection, List<ShadowCaster2D> shadowCasters, out bool hit)
    {
        float closestHitDist = _lightRadius;  // 射线最大距离为光的半径
        Vector2 closestHitPoint = rayOrigin + rayDirection * _lightRadius; // 默认射到圆形光的边缘

        foreach (var shadowCaster in shadowCasters)
        {
            Vector2? intersection = MyMathUtil.GetRayPolygonIntersections(rayOrigin, rayDirection, shadowCaster);
            if (intersection.HasValue)
            {
                float dist = Vector2.Distance(rayOrigin, intersection.Value);
                if (dist < closestHitDist)
                {
                    closestHitDist = dist;
                    closestHitPoint = intersection.Value;
                }
            }
        }

        hit = closestHitDist != _lightRadius;
        return closestHitPoint;
    }

    private void MergeClosePoints()
    {
        points.Clear();
        if (tempPoints.Count == 0) return;

        Vector2 lastPosition = tempPoints[0];
        Vector2 clusterSum = tempPoints[0];
        int clusterCnt = 1;

        for (int i = 1; i < tempPoints.Count; i++)
        {
            //加入点簇
            if (Vector2.Distance(tempPoints[i], lastPosition) < mergeThreshold)
            {
                clusterCnt++;
                clusterSum += tempPoints[i];
            }
            else
            {
                //合并上一个点簇，生成新的点簇
                points.Add(clusterSum / clusterCnt);

                clusterCnt = 1;
                clusterSum = tempPoints[i];
            }

            lastPosition = tempPoints[i];
        }
        points.Add(clusterSum / clusterCnt);
    }
}
