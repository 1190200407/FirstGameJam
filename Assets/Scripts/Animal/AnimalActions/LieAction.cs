using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LieAction : AnimalAction
{
    public float time = 0f;
    public override void OnActionStart()
    {
        host.PlayAnim("Lie");

        int levelNum = GameCtr.instance.levelNum;
        float minTime = Setting.lieMinTime;
        float maxTime = Setting.lieMaxTime;
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
