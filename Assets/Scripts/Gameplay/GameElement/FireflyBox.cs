using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireflyBox : MonoBehaviour
{
    private Rigidbody2D rb;
    public List<Firefly> fireflies;
    [SerializeField] private float speedThreshold = 5f;
    [SerializeField] private float breakThreshold = 1f;
    private bool isFalling = false;

    IEnumerator Start()
    {
        rb = GetComponent<Rigidbody2D>();
        yield return null;
        foreach (var firefly in fireflies)
        {
            firefly.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        if (rb.velocity.y < -speedThreshold)
        {
            isFalling = true;
        }

        if (isFalling && rb.velocity.y > -breakThreshold)
        {
            isFalling = false;
            Break();
        }
    }

    public void Break()
    {
        foreach (var firefly in fireflies)
        {
            firefly.gameObject.SetActive(true);
        }

        GameObject.Destroy(this.gameObject, 0.1f);
        MyEventSystem.Trigger(new DestroyObjectEvent { gameObject = this.gameObject });
    }
}
