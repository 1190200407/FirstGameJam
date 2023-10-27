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
    /// �ж�����ˮ��֮���Ƿ��������ˮ��
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
    /// ����ˮ����Ⱦһ��������
    /// </summary>
    public void DrawWater(Water[] water)
    {
        int idx = 0;

        //�޳���ý���ˮ��,������ˮ�ε�λ�÷ֶ�
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
    /// ��spline�в����µĵ�
    /// </summary>
    /// <param name="spline"></param>
    /// <param name="idx">����±�</param>
    /// <param name="pos">���λ��</param>
    /// <param name="leftTagent">������</param>
    /// <param name="rightTagent">������</param>
    private void InsertPointInSpline(Spline spline, int idx, Vector3 pos, Vector3 tagent)
    {
        spline.InsertPointAt(idx, pos);
        spline.SetTangentMode(idx, ShapeTangentMode.Broken);
        spline.SetLeftTangent(idx, -tagent);
        spline.SetRightTangent(idx, tagent);
    }

    /// <summary>
    /// ˮ�������߷����ϵ����������ҵ�
    /// </summary>
    /// <param name="water">ˮ��</param>
    /// <param name="pos">����</param>
    /// <param name="quest">0Ϊ��1Ϊ��</param>
    /// <returns></returns>
    public Vector3 WaterEdgePosition(Water water, Vector3 pos, int quest) 
    {
        Vector3 dir = new Vector3(-pos.y, pos.x) * ((quest == 0) ? 1 : -1);
        return water.transform.position + dir * water.transform.localScale.x;
    }

    /// <summary>
    /// ��һ��ˮ��
    /// </summary>
    /// <param name="flow">ˮ��������</param>
    /// <param name="list">ˮ������������ˮ��</param>
    public void DrawAWaterFlow(GameObject flow, List<Water> list)
    {
        //��ˮ����״�����
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
    /// ���Ѿ��е�ˮ��������ѡ��һ������ˮ�������û��������
    /// </summary>
    /// <param name="idx">ˮ��������±�</param>
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
