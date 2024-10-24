using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoTheaterLight : TheaterLight
{
    public float waitTime = 2f;
    private float waitTimer;
    private float rotateDir;

    void Start()
    {
        rotateDir = 1f;
    }

    void Update()
    {
        if (waitTimer > 0)
        {
            waitTimer -= Time.deltaTime;
            return;
        }

        //自动旋转
        Move(Time.deltaTime * rotateDir);
        if (curPivot == minPivot)
        {
            rotateDir = 1f;
            waitTimer = waitTime;
        }
        else if (curPivot == maxPivot)
        {
            rotateDir = -1f;
            waitTimer = waitTime;
        }
    }

    protected override void OnTriggerStay2D(Collider2D other)
    {
        //取消原来的判断
        return;
    }
}
