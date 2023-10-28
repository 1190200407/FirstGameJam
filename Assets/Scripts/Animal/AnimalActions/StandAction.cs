using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StandAction : AnimalAction
{
    public float time = 0f;
    public override void OnActionStart()
    {
        int levelNum = GameCtr.instance.levelNum;
        float minTime = Setting.standMinTime;
        float maxTime = Setting.standMaxTime;
        time = Random.Range(minTime, maxTime - levelNum);
    }

    public override void Act()
    {
        time -= Time.deltaTime;
        if (time <= 0f)
        {
            isEnd = true;
        }
    }

    public override void OnActionEnd()
    {
    }
}
