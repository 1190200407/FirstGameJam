using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animal : MonoBehaviour
{
    public AnimalAction action;
    public bool isActing;
    public int CurFill { get; set; }
    public int Capa { get; set; }

    private void Update()
    {
        if (isActing)
        {
            action.Act();
            if (action.isEnd)
            {
                OnActionEnd();
            }
        }
    }

    /// <summary>
    /// ��ʼ��
    /// </summary>
    public virtual void Init()
    {
    }

    public void ChangeNowAction(AnimalAction act)
    {
        action = act;
        action.host = this;
        action.OnActionStart();
    }

    /// <summary>
    /// �������궯������Ӧ�Ĳ���
    /// </summary>
    public virtual void OnActionEnd()
    {
        action.OnActionEnd();
    }

    /// <summary>
    /// ˮ�ν���ʱ��Ӧ�Ĳ���
    /// </summary>
    public virtual void OnWaterEnter()
    {
        CurFill++;
        if (CurFill == Capa)
            OnWaterFull();
    }

    /// <summary>
    /// ˮ����ʱ��Ӧ�Ĳ���
    /// </summary>
    public virtual void OnWaterFull()
    {
    }
}
