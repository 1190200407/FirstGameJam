using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FleaAction : AnimalAction
{

    public float time = 0f;
    public float speed = 0f;

    public override void OnActionStart()
    {
        host.PlayAnim("Flea");

        speed = 80f;
        time = 5f;
    }

    public override void Act()
    {
        //移动
        host.Direction = host.transform.position.x > 0f ? 1 : -1;
        //转向
        if (Mathf.Abs(host.transform.localScale.x + host.Direction) > Mathf.Epsilon)
            host.transform.localScale = new Vector3(-host.Direction, 1f, 1f);
        host.transform.position += Vector3.right * host.Direction * speed * Time.deltaTime * 0.1f;

        //减少计时
        time -= Time.deltaTime;
        if (time <= 0f)
        {
            isEnd = true;
        }
    }

    public override void OnActionEnd()
    {
        Object.Destroy(host.gameObject);
    }
}
