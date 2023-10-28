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
    /// 初始化
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
    /// 动物做完动作后响应的操作
    /// </summary>
    public virtual void OnActionEnd()
    {
        action.OnActionEnd();
    }

    /// <summary>
    /// 水滴进入时响应的操作
    /// </summary>
    public virtual void OnWaterEnter()
    {
        CurFill++;
        if (CurFill == Capa)
            OnWaterFull();
    }

    /// <summary>
    /// 水滴满时响应的操作
    /// </summary>
    public virtual void OnWaterFull()
    {
    }
}
