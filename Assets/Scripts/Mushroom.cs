using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mushroom : MonoBehaviour
{
    public float bounceForce = 2f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Rigidbody2D rb))
        {
            // 获取进入触发器的物体的位置
            Vector2 triggerCenter = transform.position;
            Vector2 objectCenter = collision.bounds.center;

            // 计算从触发器中心到物体中心的方向
            Vector2 direction = (objectCenter - triggerCenter).normalized;

            // 应用弹射力
            rb.velocity = direction * rb.velocity.magnitude * 0.7f;
        }
    }
}
