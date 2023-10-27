using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.U2D;

public class WaterRenderer : MonoBehaviour
{
    public GameObject waterFlowPrefab;
    public GameObject waterParent;
    public List<GameObject> waterFlows = new List<GameObject>();

    private void Update()
    {
        DrawWater(waterParent.GetComponentsInChildren<Water>());
    }

    /// <summary>
    /// 判断两个水珠之间是否可以连成水柱
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    public bool IsSameFlow(Water a, Water b)
    {
        Vector3 pa = a.MostEdgePosition(1);
        Vector3 pb = b.MostEdgePosition(2);
        Vector3 pc = a.MostEdgePosition(2);
        Vector3 pd = b.MostEdgePosition(1);

        pa += (pa - pb).normalized * 0.5f;
        pb += (pb - pa).normalized * 0.5f;
        pc += (pc - pd).normalized * 0.5f;
        pd += (pd - pc).normalized * 0.5f;

        return MathUtil.TryGetIntersectPoint(pa, pb, pc, pd, out Vector3 pos);
    }


    /// <summary>
    /// 根据水滴渲染一条抛物线
    /// </summary>
    public void DrawWater(Water[] water)
    {
        int idx = 0;

        //剔除离得近的水滴,并根据水滴的位置分段
        List<Water> list = new List<Water>();
        foreach (Water w in water)
        {
            if (list.Count == 0)
            {
                list.Add(w); 
            }
            else if (w.DistTo(list[list.Count - 1]) >= 3f || !IsSameFlow(w, list[list.Count - 1]))
            {
                MakeAWaterFlow(idx++, new List<Water>(list));
                list.Clear();
                list.Add(w); 
            }
            else if (w.DistTo(list[list.Count - 1]) > 0.2f)
            {
                list.Add(w);
            }
        }
        if (list.Count > 0)
        {
            MakeAWaterFlow(idx++, new List<Water>(list));
        }

        for (int i = idx; i < waterFlows.Count; i++)
        {
            waterFlows[i].SetActive(false);
        }
    }

    /// <summary>
    /// 在spline中插入新的点
    /// </summary>
    /// <param name="spline"></param>
    /// <param name="idx">点的下标</param>
    /// <param name="pos">点的位置</param>
    /// <param name="leftTagent">左切线</param>
    /// <param name="rightTagent">右切线</param>
    private void InsertPointInSpline(Spline spline, int idx, Vector3 pos, Vector3 tagent)
    {
        spline.InsertPointAt(idx, pos);
        spline.SetTangentMode(idx, ShapeTangentMode.Broken);
        spline.SetLeftTangent(idx, -tagent);
        spline.SetRightTangent(idx, tagent);
    }

    /// <summary>
    /// 水滴在切线方向上的最左点或最右点
    /// </summary>
    /// <param name="water">水滴</param>
    /// <param name="pos">切线</param>
    /// <param name="quest">0为左，1为右</param>
    /// <returns></returns>
    public Vector3 WaterEdgePosition(Water water, Vector3 pos, int quest) 
    {
        Vector3 dir = new Vector3(-pos.y, pos.x) * ((quest == 0) ? 1 : -1);
        return water.transform.position + dir * water.transform.localScale.x;
    }

    /// <summary>
    /// 画一道水流
    /// </summary>
    /// <param name="flow">水流的物体</param>
    /// <param name="list">水流包含的所有水滴</param>
    public void DrawAWaterFlow(GameObject flow, List<Water> list)
    {
        //给水滴形状描个边
        Spline spline = flow.GetComponent<SpriteShapeController>().spline;
        spline.Clear();

        InsertPointInSpline(spline, 0, list[0].MostEdgePosition(0), list[0].GetTagent(1));

        for (int i = 0; i < list.Count; i++)
        {
            InsertPointInSpline(spline, i + 1, list[i].MostEdgePosition(1), list[i].GetTagent(3));
        }

        InsertPointInSpline(spline, list.Count + 1, list[list.Count - 1].MostEdgePosition(3), list[list.Count - 1].GetTagent(2));

        for (int i = list.Count - 1; i >= 0; i--)
        {
            InsertPointInSpline(spline, 2 * list.Count + 1 - i, list[i].MostEdgePosition(2), list[i].GetTagent(0));
        }
    }

    /// <summary>
    /// 在已经有的水流物体中选择一个绘制水流，如果没有则生成
    /// </summary>
    /// <param name="idx">水流物体的下标</param>
    /// <param name="list"></param>
    public void MakeAWaterFlow(int idx, List<Water> list)
    {
        if (waterFlows.Count <= idx)
        {
            waterFlows.Add(Instantiate(waterFlowPrefab, transform));
        }
        GameObject flow = waterFlows[idx];
        DrawAWaterFlow(flow, list);
        flow.SetActive(true);
    }
}
