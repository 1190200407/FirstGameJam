using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RunAction : AnimalAction
{
    public float time = 0f;
    public float speed = 0f;
    private float turnCheckTime = 0f;
    public override void OnActionStart()
    {
        int levelNum = GameCtr.instance.levelNum;
        speed = Setting.runSpeed;
        time = Random.Range(Setting.runMinTime, Setting.runMaxTime - levelNum);
        turnCheckTime = 0f;
    }

    public override void Act()
    {
        //�ƶ�
        host.transform.position += Vector3.right * host.Direction * speed * Time.deltaTime * 0.1f;
        host.walkDist += speed * Time.deltaTime * 0.5f;

        //���ټ�ʱ
        time -= Time.deltaTime;
        if (time <= 0f)
        {
            isEnd = true;
        }

        //�ж�ת��
        turnCheckTime += Time.deltaTime;
        if (turnCheckTime > Setting.turnCheckTime)
        {
            host.TurnCheck();
            turnCheckTime -= Setting.turnCheckTime;
        }
    }

    public override void OnActionEnd()
    {
    }
}
