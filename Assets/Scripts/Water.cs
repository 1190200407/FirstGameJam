using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

/// <summary>
/// 控制水滴的移动和碰撞
/// </summary>
public class Water : MonoBehaviour
{
    public float surviveTime = 0f;

    private void Update()
    {
        if (surviveTime > 0f)
        {
            surviveTime -= Time.deltaTime;
            if (surviveTime < 0f)
            {
                Destroy(gameObject);
            }
        }
    }

    public void SetSpeed(float speed, Vector3 dir)
    {
        GetComponent<Rigidbody2D>().velocity = dir * speed;
    }

    public Vector3 GetSpeed()
    {
        return GetComponent<Rigidbody2D>().velocity;
    }

    /// <summary>
    /// <para>设速度为水滴的前方，求水滴中最边缘的点</para>
    /// <para>参数中，0为前方，1为左方，2为右方，3为后方</para>
    /// </summary>
    /// <param name="quest"></param>
    /// <returns></returns>
    public Vector3 MostEdgePosition(int quest)
    {
        return transform.position + GetTagent(quest) * 1.5f;
    }

    /// <summary>
    /// <para>以速度为参考，获得水滴切线</para>
    /// <para>0 是向前的切线</para>
    /// <para>1 是向左的切线</para>
    /// <para>2 是向右的切线</para
    /// <para>3 是向后的切线</para>
    /// </summary>
    /// <param name="quest"></param>
    /// <returns></returns>
    public Vector3 GetTagent(int quest)
    {
        Vector3 speedDir = GetComponent<Rigidbody2D>().velocity.normalized;
        Vector3 dir = Vector3.zero;
        switch (quest)
        {
            case 0:
                dir = speedDir;
                break;
            case 1:
                dir = new Vector3(-speedDir.y, speedDir.x);
                break;
            case 2:
                dir = new Vector3(speedDir.y, -speedDir.x);
                break;
            case 3:
                dir = -speedDir;
                break;
        }
        return dir * transform.localScale.x;
    }

    /// <summary>
    /// 测算两个水珠之间的距离
    /// </summary>
    /// <param name="other">另一滴水珠</param>
    /// <returns>水珠的距离</returns>
    public float DistTo(Water other)
    {
        return (transform.position - other.transform.position).magnitude;
    }

    /// <summary>
    /// 发生碰撞时，判断碰撞的物体为什么
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter2D(Collider2D other)
    {
        switch (other.tag)
        {
            case "Cat":
                GameCtr.instance.NowScore = math.min(GameCtr.instance.NowScore + 1, GameCtr.instance.GoalScore);
                break;
            case "Dog":
                GameCtr.instance.NowScore = math.max(GameCtr.instance.NowScore - 1, 0);
                break;
            case "Chick":
                break;
        }
        if (other.tag != "Water")
        {
            Destroy(gameObject);
        }
    }
}
