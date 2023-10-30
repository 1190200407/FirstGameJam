using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flower : MonoBehaviour
{
    public float attractionForce = 10f;

    private void OnTriggerStay2D(Collider2D other)
    {
        // ����Ƿ���Rigidbody2D������������ڶ���Χ
        Rigidbody2D rb = other.GetComponent<Rigidbody2D>();

        if (rb != null)
        {
            // ������������ķ���
            Vector2 direction = (transform.position - other.transform.position).normalized;

            // Ӧ��������
            rb.AddForce(direction * attractionForce * Time.fixedDeltaTime);
        }
    }
}
