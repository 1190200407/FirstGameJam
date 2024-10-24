using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class FanshapedLight : InteractableLight
{
    public Transform startPoint;
    [Header("截断直线")]
    public Transform cutPoint1;
    public Transform cutPoint2;

    [SerializeField]
    private float radius = 20f;

    private float rotation = 0f;
    private float spotAngle = 30f;
    private float rotateAngle = 0.05f;

    private Dictionary<Vector2, float> pointToAngle = new Dictionary<Vector2, float>();
    private List<Vector2> visibleDirections = new List<Vector2>();

    private List<Vector2> tempPoints = new List<Vector2>();
    private List<Mirror> inLightMirrors = new List<Mirror>();
    private List<Mirror> tempMirrors = new List<Mirror>();

    protected override void OnDisable()
    {
        base.OnDisable();
        //把反射光也去掉
        foreach (var mirror in inLightMirrors)
        {
            if (mirror != null && mirror.reflections[this] != null)
                mirror.reflections[this].enabled = false;
        }
    }

    private void ProcessDirection(Vector2 dir, float angle)
    {
        Vector2 temp = MyMathUtil.RotateVector(dir, -rotateAngle);
        if (!pointToAngle.ContainsKey(temp))
        {
            visibleDirections.Add(temp);
            pointToAngle.Add(temp, angle - rotateAngle);
        }

        temp = dir;
        if (!pointToAngle.ContainsKey(temp))
        {
            visibleDirections.Add(temp);
            pointToAngle.Add(temp, angle);
        }

        temp = MyMathUtil.RotateVector(dir, rotateAngle);
        if (!pointToAngle.ContainsKey(temp))
        {
            visibleDirections.Add(temp);
            pointToAngle.Add(temp, angle + rotateAngle);
        }
    }

    public override void UpdateLightPoints()
    {
        //如果cutPoint1 和 cutPoint2靠得太近，就不要改变
        if (Vector2.Distance(cutPoint1.position, cutPoint2.position) < 0.1f) return;

        // 获取所有 ShadowCaster2D
        List<ShadowCaster2D> shadowCasters = LevelManager.Instance.ShadowCasters;

        // 1. 根据cutPoint 计算 rotation 和 spotAngle
        rotation = Mathf.Atan2(cutPoint1.position.y - startPoint.position.y, cutPoint1.position.x - startPoint.position.x) * Mathf.Rad2Deg;
        spotAngle = Mathf.Atan2(cutPoint2.position.y - startPoint.position.y, cutPoint2.position.x - startPoint.position.x) * Mathf.Rad2Deg;
        spotAngle = (spotAngle - rotation + 360f) % 360f;
        
        // 2. 遍历所有 ShadowCaster2D 的多边形点，检查是否在扇形内
        pointToAngle.Clear();
        visibleDirections.Clear();
        Vector2 direction;
        foreach (var shadowCaster in shadowCasters)
        {
            Vector3[] points = shadowCaster.shapePath;
            for (int i = 0; i < points.Length; i++)
            {
                if (shadowCaster == null) continue;
                Vector2 worldPoint = shadowCaster.transform.TransformPoint(points[i]);
                float normalizedAngle;
                if (MyMathUtil.IsPointInFanArea(worldPoint, startPoint.position, radius, rotation, spotAngle, out normalizedAngle))
                {
                    direction = (worldPoint - (Vector2)startPoint.position).normalized;
                    ProcessDirection(direction, normalizedAngle);
                }
            }
        }       
        // 3. 添加边界点
        direction = new Vector2(Mathf.Cos(rotation * Mathf.Deg2Rad), Mathf.Sin(rotation * Mathf.Deg2Rad));
        ProcessDirection(direction, 0);
        
        direction = new Vector2(Mathf.Cos((rotation + spotAngle) * Mathf.Deg2Rad), Mathf.Sin((rotation + spotAngle) * Mathf.Deg2Rad));
        ProcessDirection(direction, spotAngle);

        // 4. 根据 normalizedAngle 对点进行排序
        visibleDirections.Sort((p1, p2) => {
            return pointToAngle[p1].CompareTo(pointToAngle[p2]);
        });

        // 5. 对每个点包括两条边界进行射线检测，检测时发射上下旋转0.05度后的射线和射线本身，连接三个点
        tempPoints.Clear();
        tempMirrors.Clear();
        tempPoints.Add(cutPoint1.position);

        float lastAngle = float.NegativeInfinity;
        Mirror hitMirror = null, tempMirror;
        foreach (Vector2 dir in visibleDirections)
        {
            if (Mathf.Abs(lastAngle - pointToAngle[dir]) < rotateAngle / 5f) continue; //跳过角度接近的点
            Vector2 lightPoint = ScanRay(startPoint.position, dir, shadowCasters, out tempMirror);
            // 更新镜像光
            if (canReflect && tempMirror != null && tempMirror != reflectMirror)
            {
                FanshapedLight light = tempMirror.GetOrAddReflection(this) as FanshapedLight;
                if (light != null)
                {
                    // 撞到了新的镜子，更新反射光的光源和起始点
                    if (hitMirror != tempMirror)
                    {
                        hitMirror = tempMirror;
                        tempMirrors.Add(tempMirror);

                        light.cutPoint2.position = lightPoint;
                        light.startPoint.position = MyMathUtil.MirrorPoint(startPoint.position, tempMirror.cutPoint1.position, tempMirror.cutPoint2.position);
                        light.enabled = true;
                    }
                    // 撞到了原来的镜子，更新反射光的终止点
                    else
                    {
                        light.cutPoint1.position = lightPoint;
                    }
                }
            }
            else
            {    
                hitMirror = null;
            }
            lastAngle = pointToAngle[dir];
        }

        for (int i = inLightMirrors.Count - 1; i >= 0; i--)
            if (!tempMirrors.Contains(inLightMirrors[i]))
            {
                inLightMirrors[i].reflections[this].enabled = false;
                inLightMirrors.RemoveAt(i);
            }
        
        for (int i = 0; i < tempMirrors.Count; i++)
            if (!inLightMirrors.Contains(tempMirrors[i]))
                inLightMirrors.Add(tempMirrors[i]);
        
        tempPoints.Add(cutPoint2.position);

        // 6.把靠近的点合并为一个点
        MergeClosePoints();
        
        // 7.更新光和碰撞体
        if (points.Count < 3)
        {
            light2D.enabled = false;
            collider2D.enabled = false;
            return;
        }
        light2D.enabled = true;
        collider2D.enabled = true;
        light2D.SetShapePath(points.ToArray());
        collider2D.SetPath(0, MyMathUtil.ConvertToVector2Array(points));
    }

    /// <summary>
    /// 发射射线判断结果，如果擦到端点但是不撞到多边形，连接点并继续扫描，直到碰到最大距离，或者撞到多边形，然后把最后的点也连上
    /// </summary>
    public Vector2 ScanRay(Vector2 rayOrigin, Vector2 rayDirection, List<ShadowCaster2D> shadowCasters, out Mirror hitMirror)
    {
        // Step 1: 计算射线与cutPoint1、cutPoint2组成的线段的交点
        Vector2? intersection = MyMathUtil.IntersectLineWithRay(cutPoint1.position, cutPoint2.position, rayOrigin, rayDirection);

        // 如果射线与线段相交，则从交点开始发射射线
        if (intersection.HasValue)
        {
            // 将射线的起点更新为交点
            rayOrigin = intersection.Value;
        }

        float closeHitDist = float.MaxValue;
        Vector2 closestHitPoint = Vector2.zero;
        ShadowCaster2D hitCaster = null;

        foreach (var shadowCaster in shadowCasters)
        {
            if (shadowCaster == null) continue;
            if (reflectMirror != null && shadowCaster.gameObject == reflectMirror.gameObject) continue;
            intersection = MyMathUtil.GetRayPolygonIntersections(rayOrigin, rayDirection, shadowCaster);
            
            if (intersection.HasValue)
            {
                float dist = Vector2.Distance(rayOrigin, intersection.Value);
                if (dist < closeHitDist)
                {
                    closeHitDist = dist;
                    closestHitPoint = intersection.Value;
                    hitCaster = shadowCaster;
                }
            }
        }
        
        hitMirror = null;
        Vector2 lightPoint;
        if (hitCaster != null)
        {
            lightPoint = closestHitPoint;

            if (hitCaster.TryGetComponent(out Mirror mirror) && mirror.enabled)
            {
                hitMirror = mirror;
            }
        }
        else
        {
            lightPoint = rayOrigin + rayDirection * radius;
        }
        tempPoints.Add(lightPoint);
        return lightPoint;
    }

    private void MergeClosePoints(float mergeThreshold = 0.05f)
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

    private Vector2 MergeClusterPoints(List<Vector2> cluster)
    {
        Vector2 averagePoint = Vector2.zero;
        foreach (Vector2 point in cluster)
        {
            averagePoint += point;
        }
        return averagePoint / cluster.Count;
    }
}
