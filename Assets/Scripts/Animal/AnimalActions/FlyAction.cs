using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class FlyAction : AnimalAction
{
    public float runSpeed;
    public float flyUpSpeed;
    public float runTime;
    public float flyUpTime;
    public int status;
    public float time;
    public float originalY;

    public override void OnActionStart()
    {
        flyUpTime = Random.Range(1f, 1.5f);
        flyUpSpeed = 7f;

        runTime = Random.Range(2f, 3.6f - GameCtr.instance.levelNum);
        runSpeed = 20f;

        status = 0;
        originalY = host.transform.position.y;
    }

    public override void Act()
    {
        time += Time.deltaTime;

        //×ªÏò
        if (Mathf.Abs(host.transform.localScale.x + host.Direction) > Mathf.Epsilon)
            host.transform.localScale = new Vector3(-host.Direction, 1f, 1f);

        if (status == 0)
        {
            host.transform.position += Vector3.up * flyUpSpeed * Time.deltaTime * 0.1f;
            if (time > flyUpTime)
            {
                status = 1;
                time = 0;
            }
        }
        else if (status == 1)
        {
            host.transform.position += Vector3.right * host.Direction * runSpeed * Time.deltaTime * 0.1f;
            host.walkDist += runSpeed * Time.deltaTime * 0.5f;
            if (time > runTime)
            {
                status = 2;
                time = 0;
            }
        }
        else if (status == 2)
        {
            host.transform.position -= Vector3.up * flyUpSpeed * Time.deltaTime * 0.1f;
            if (time > flyUpTime)
            {
                host.transform.position += Vector3.up * (originalY - host.transform.position.y);
                isEnd = true;
            }
        }
    }
}
