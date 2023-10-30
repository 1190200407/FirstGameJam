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
            // ��ȡ���봥�����������λ��
            Vector2 triggerCenter = transform.position;
            Vector2 objectCenter = collision.bounds.center;

            // ����Ӵ��������ĵ��������ĵķ���
            Vector2 direction = (objectCenter - triggerCenter).normalized;

            // Ӧ�õ�����
            rb.velocity = direction * rb.velocity.magnitude * 0.7f;
        }
    }
}
