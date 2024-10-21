using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrazyShadowCaster : MonoBehaviour
{
    public Transform startPoint;
    public Transform endPoint;

    private Vector3 startPos;
    private Vector3 endPos;
    private Vector3 moveDir;

    public float moveSpeed;
    public float waitTime;

    private float waitTimer = 0;

    void Start()
    {
        startPos = startPoint.position;
        endPos = endPoint.position;
        moveDir = (endPos - startPos).normalized;
        transform.position = startPos;
    }

    void Update()
    {
        if (waitTimer > 0f)
        {
            waitTimer -= Time.deltaTime;
            if (waitTimer <= 0f)
            {
                transform.position = startPos;
            }
        }
        else
        {
            transform.position += moveDir * moveSpeed * Time.deltaTime;
            if (Vector3.Dot(transform.position - startPos, transform.position - endPos) >= 0f)
            {
                waitTimer = waitTime;
            }
        }
    }
}
