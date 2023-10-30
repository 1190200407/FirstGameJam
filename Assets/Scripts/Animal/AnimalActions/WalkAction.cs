using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WalkAction : AnimalAction
{
    public float time = 0f;
    public float speed = 0f;
    private float turnCheckTime = 0f;
    public override void OnActionStart()
    {
        host.PlayAnim("Walk");

        int levelNum = GameCtr.instance.levelNum;
        speed = Setting.walkSpeed;
        time = Random.Range(Setting.walkMinTime, Setting.walkMaxTime - levelNum);
        turnCheckTime = 0f;
    }

    public override void Act()
    {
        //ת��
        if (Mathf.Abs(host.transform.localScale.x + host.Direction) > Mathf.Epsilon)
            host.transform.localScale = new Vector3(-host.Direction, 1f, 1f);

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