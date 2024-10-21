using UnityEngine;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine.Rendering.Universal;
using System;

public class MyMathUtil
{
    public static float FastPow(float a, int x)
    {
        float ans = 1f;
        while (x != 0)
        {
            if ((x & 1) == 1)
                ans *= a;
            a = a * a;
            x = x >> 1;
        }
        return ans;
    }

    public static Vector2[] ConvertToVector2Array(List<Vector3> list)
    {
        Vector2[] array = new Vector2[list.Count];
        for (int i = 0; i < list.Count; i++)
        {
            array[i] = list[i];
        }
        return array;
    }

    public static bool IsLineOverlappingRay(Vector2 linePoint1, Vector2 linePoint2, Vector2 rayOrigin, Vector2 direction)
    {
        float denominator = (linePoint1.y - rayOrigin.y) * direction.normalized.x - (linePoint1.x - rayOrigin.x)* direction.normalized.y;
        return Mathf.Abs(denominator) < 0.0001f;
    }

    
    /// <summary>
    /// 计算直线与射线的交点
    /// </summary>
    public static Vector3? IntersectLineWithRay(Vector2 linePoint1, Vector2 linePoint2, Vector2 rayOrigin, Vector2 direction)
    {
        // 计算直线的方向向量
        Vector2 lineDir = linePoint2 - linePoint1;

        // 射线的方向与直线的方向向量是否平行
        float denominator = lineDir.y * direction.x - lineDir.x * direction.y;
        if (Mathf.Abs(denominator) < 0.0001f)
        {
            // 共线，无交点
            return null;
        }

        // 计算直线和射线的交点
        float t = ((linePoint1.x - rayOrigin.x) * direction.y - (linePoint1.y - rayOrigin.y) * direction.x) / denominator;
        float u = ((linePoint1.x - rayOrigin.x) * lineDir.y - (linePoint1.y - rayOrigin.y) * lineDir.x) / denominator;

        // u >= 0 确保是射线上前向的点，而不是射线起点的反向点
        if (u >= 0)
        {
            // 交点的坐标
            Vector2 intersectionPoint = linePoint1 + t * lineDir;
            return intersectionPoint;
        }

        // 否则无交点
        return null;
    }

    /// <summary>
    /// 计算线段与射线的交点
    /// </summary>
    public static Vector3? IntersectLineSegmentWithRay(Vector2 linePoint1, Vector2 linePoint2, Vector2 rayOrigin, Vector2 direction)
    {
        // 计算线段的方向向量
        Vector2 lineDir = linePoint2 - linePoint1;

        // 射线的方向与线段的方向向量是否平行
        float denominator = lineDir.y * direction.x - lineDir.x * direction.y;
        if (Mathf.Abs(denominator) < 0.0001f)
        {
            // 共线，无交点
            return null;
        }

        // 计算线段和射线的交点
        float t = ((linePoint1.x - rayOrigin.x) * direction.y - (linePoint1.y - rayOrigin.y) * direction.x) / denominator;
        float u = ((linePoint1.x - rayOrigin.x) * lineDir.y - (linePoint1.y - rayOrigin.y) * lineDir.x) / denominator;

        // u >= 0 确保是射线上前向的点，而不是射线起点的反向点
        // 0 <= t <= 1 确保交点在线段范围内
        if (t >= 0 && t <= 1 && u >= 0)
        {
            // 交点的坐标
            Vector2 intersectionPoint = linePoint1 + t * lineDir;
            return intersectionPoint;
        }

        // 否则无交点
        return null;
    }

    /// <summary>
    /// 判断点是否在扇形区域内
    /// </summary>
    public static bool IsPointInFanArea(Vector2 point, Vector2 origin, float radius, float rotationZ, float angle, out float normalizedAngle)
    {
        // 计算点与扇形中心的距离
        Vector2 direction = point - origin;
        float distance = direction.magnitude;

        // 如果点的距离超过半径，则不在扇形内
        if (distance > radius)
        {
            normalizedAngle = 0f;
            return false;
        }

        // 计算点相对于扇形中心的角度
        float angleToPoint = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // 归一化角度到 0 到 360 度之间
        normalizedAngle = (angleToPoint - rotationZ + 360) % 360;

        return normalizedAngle <= angle;
    }

    /// <summary>
    /// 判断射线与多边形的相交关系，并返回相交点列表
    /// </summary>
    public static Vector2? GetRayPolygonIntersections(Vector2 rayOrigin, Vector2 rayDirection, ShadowCaster2D shadowCaster)
    {
        float closestDist = float.MaxValue;
        Vector2? closestPoint = null;

        // 遍历多边形的每一条边，检测相交
        for (int i = 0; i < shadowCaster.shapePath.Length; i++)
        {
            Vector2 lineStart = shadowCaster.transform.TransformPoint(shadowCaster.shapePath[i]);
            Vector2 lineEnd = shadowCaster.transform.TransformPoint(shadowCaster.shapePath[(i + 1) % shadowCaster.shapePath.Length]);

            Vector2? intersectionPoint = IntersectLineSegmentWithRay(lineStart, lineEnd, rayOrigin, rayDirection);

            if (intersectionPoint.HasValue)
            {
                float dist = Vector2.Distance(rayOrigin, intersectionPoint.Value);
                // 两个点不是同一点
                if (Mathf.Abs(dist - closestDist) > 0.001f)
                {
                    if (dist < closestDist)
                    {
                        closestPoint = intersectionPoint.Value;
                        closestDist = dist;
                    }
                }
            }
        }

        return closestPoint;
    }

    /// <summary>
    /// 旋转向量
    /// </summary>
    public static Vector2 RotateVector(Vector2 vector, float angleInDegrees)
    {
        // 将角度转换为弧度
        float angleInRadians = angleInDegrees * Mathf.Deg2Rad;

        // 计算旋转后的新向量
        float cosAngle = Mathf.Cos(angleInRadians);
        float sinAngle = Mathf.Sin(angleInRadians);

        float newX = vector.x * cosAngle - vector.y * sinAngle;
        float newY = vector.x * sinAngle + vector.y * cosAngle;

        return new Vector2(newX, newY);
    }

    public static bool IsPointInCircle(Vector2 circleCenter, float radius, Vector2 point)
    {
        return Vector2.SqrMagnitude(circleCenter - point) <= radius * radius;
    }

    public static int GetCircleLineIntersections(Vector2 circleCenter, float radius, Vector2 lineStart, Vector2 lineEnd, out Vector2 intersection1, out Vector2 intersection2)
    {
        intersection1 = Vector2.zero;
        intersection2 = Vector2.zero;

        // 线段的向量
        Vector2 d = lineEnd - lineStart;
        Vector2 f = lineStart - circleCenter;

        float a = Vector2.Dot(d, d);
        float b = 2 * Vector2.Dot(f, d);
        float c = Vector2.Dot(f, f) - radius * radius;

        float discriminant = b * b - 4 * a * c;

        // 没有交点
        if (discriminant < 0)
        {
            return 0;
        }

        // 计算交点
        discriminant = Mathf.Sqrt(discriminant);

        // t1 和 t2 是线段上的比例系数，用于求交点
        float t1 = (-b - discriminant) / (2 * a);
        float t2 = (-b + discriminant) / (2 * a);

        // 检查 t1 是否在 [0, 1] 范围内，表示交点在线段上
        int cnt = 0;
        if (t1 >= 0 && t1 <= 1)
        {
            intersection1 = lineStart + t1 * d;
            cnt++;
        }

        // 检查 t2 是否在 [0, 1] 范围内，表示交点在线段上
        if (Mathf.Abs(t1 - t2) > 0.01f && t2 >= 0 && t2 <= 1)
        {
            if (cnt == 1)
                intersection2 = lineStart + t2 * d;
            else
                intersection1 = lineStart + t2 * d;
            cnt++;
        }

        return cnt;
    }

    // 计算点对于另一个点的反向
    public static Vector2 ReversePoint(Vector2 point, Vector2 pivot)
    {
        return point + (pivot - point) * 2f;
    }

    // 计算点在线段上的镜像点
    public static Vector2 MirrorPoint(Vector2 point, Vector2 linePoint1, Vector2 linePoint2)
    {
        // 计算线段的方向向量
        Vector2 lineDir = (linePoint2 - linePoint1).normalized;

        // 计算点到线段的垂直向量
        Vector2 pointToLine = point - linePoint1;
        float projectionLength = Vector2.Dot(pointToLine, lineDir);
        Vector2 projectionPoint = linePoint1 + projectionLength * lineDir;

        // 计算垂直向量，并计算镜像点
        Vector2 perpendicular = point - projectionPoint;
        Vector2 mirroredPoint = point - 2 * perpendicular;

        return mirroredPoint;
    }

    // 计算角度在镜子上的反射
    public static float MirrorRotation(float angle, Vector2 linePoint1, Vector2 linePoint2)
    {
        // 计算线段的方向向量并转换为角度
        Vector2 lineDir = (linePoint2 - linePoint1).normalized;
        float lineAngle = Mathf.Atan2(lineDir.y, lineDir.x) * Mathf.Rad2Deg;

        // 计算相对于线段法线的角度
        float relativeAngle = angle - lineAngle;

        // 镜像后的角度 = 线段角度 - 相对角度
        float mirroredAngle = lineAngle - relativeAngle;

        return mirroredAngle;
    }

    // 旋转给定点 pivot，角度 angle 为逆时针方向，单位为度
    public static Vector2 RotatePoint(Vector2 point, Vector2 pivot, float angle)
    {
        // 将角度转换为弧度
        float radian = angle * Mathf.Deg2Rad;

        // 计算旋转前的相对位置
        Vector2 direction = point - pivot;

        // 应用2D旋转矩阵
        float cosAngle = Mathf.Cos(radian);
        float sinAngle = Mathf.Sin(radian);
        
        Vector2 rotatedDirection = new Vector2(
            direction.x * cosAngle - direction.y * sinAngle,
            direction.x * sinAngle + direction.y * cosAngle
        );

        // 计算旋转后的新位置
        return pivot + rotatedDirection;
    }
}