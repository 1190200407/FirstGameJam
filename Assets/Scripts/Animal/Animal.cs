using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

/// <summary>
/// �������ű�
/// �ص��Ǹ�дInit��OnActionEnd��OnWaterEnter��OnWaterFull
/// </summary>
public class Animal : MonoBehaviour
{
    public AnimalSetting Setting => GameCtr.instance.anmMgr.settings[tag];

    public AnimalAction action;
    public bool isActing = false;
    public bool isLeaving = false;

    private int _direction = 1;
    public int Direction
    {
        get { return _direction; }
        set 
        {
            _direction = value;
            walkDist = 0f;
        }
    }
    public float walkDist = 0f;

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
        ChangeNowAction(GetAnAction());
    }

    public void ChangeNowAction(AnimalAction act)
    {
        Debug.Log(act.GetType().Name);
        action = act;
        if (act == null) return;
        action.host = this;
        action.OnActionStart();
        isActing = true;
    }

    /// <summary>
    /// ����һ�������ж�
    /// </summary>
    /// <returns></returns>
    public virtual AnimalAction GetAnAction()
    {
        return null;
    }

    /// <summary>
    /// �������궯������Ӧ�Ĳ���
    /// </summary>
    public virtual void OnActionEnd()
    {
        action.OnActionEnd();
        ChangeNowAction(GetAnAction());
    }

    /// <summary>
    /// �ж��Ƿ�Ҫת��
    /// </summary>
    public void TurnCheck()
    {
        if (walkDist > Setting.trunCheckDist && UnityEngine.Random.value < Setting.trunCheckProb)
        {
            Direction = -Direction;
        }
    }

    /// <summary>
    /// ˮ�ν���ʱ��Ӧ�Ĳ���
    /// </summary>
    public virtual void OnWaterEnter()
    {
        if (isLeaving) return;
        CurFill++;
        if (CurFill == Capa)
            OnWaterFull();

        GameCtr.instance.NowScore = math.clamp(GameCtr.instance.NowScore + Setting.waterEnterScore, 0, GameCtr.instance.GoalScore);
    }

    /// <summary>
    /// ˮ����ʱ��Ӧ�Ĳ���
    /// </summary>
    public virtual void OnWaterFull()
    {
        isLeaving = true;
        GameCtr.instance.NowScore = math.clamp(GameCtr.instance.NowScore + Setting.waterFullScore, 0, GameCtr.instance.GoalScore);
    }
}
