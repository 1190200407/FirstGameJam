using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class JumpAction : AnimalAction
{
    public float dist;
    public float totTime;
    public float nowTime;
    public float jumpHeight;
    public Vector3 startPoint;
    public Vector3 endPoint;
    public override void OnActionStart()
    {
        dist = Random.Range(2f, 3.5f);
        startPoint = host.transform.position;
        endPoint = host.transform.position + dist * host.Direction * Vector3.right;

        jumpHeight = Random.Range(1.5f, 1.7f);
        totTime = dist / 3f;
        nowTime = 0;
    }

    public override void Act()
    {
        nowTime += Time.deltaTime;

        float t = nowTime / totTime;
        if (t < 1.0f)
        {
            float jumpY = Mathf.Sin(t * Mathf.PI) * jumpHeight;
            Vector3 jumpPosition = Vector3.Lerp(startPoint, endPoint, t);
            jumpPosition.y += jumpY;
            host.transform.position = jumpPosition;
        }
        else
        {
            host.transform.position = endPoint;
            isEnd = true;
        }
    }
}