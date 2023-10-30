using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flower : MonoBehaviour
{
    public float attractionForce = 10f;

    private void OnTriggerStay2D(Collider2D other)
    {
        // 检测是否有Rigidbody2D组件的物体进入黑洞范围
        Rigidbody2D rb = other.GetComponent<Rigidbody2D>();

        if (rb != null)
        {
            // 计算吸引物体的方向
            Vector2 direction = (transform.position - other.transform.position).normalized;

            // 应用吸引力
            rb.AddForce(direction * attractionForce * Time.fixedDeltaTime);
        }
    }
}
