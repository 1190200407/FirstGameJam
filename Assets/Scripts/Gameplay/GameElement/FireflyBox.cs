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
    private Dictionary<Firefly, Vector2> fireflyPosDict;

    IEnumerator Start()
    {
        rb = GetComponent<Rigidbody2D>();
        fireflyPosDict = new Dictionary<Firefly, Vector2>();
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
            StartCoroutine(Break());
        }
    }

    public IEnumerator Break()
    {
        AudioManager.Instance.PlaySfx("玻璃罐碎裂");

        foreach (var firefly in fireflies)
        {
            fireflyPosDict.Add(firefly, firefly.transform.position);
            firefly.transform.position = transform.position + new Vector3(Random.Range(-0.3f, 0.3f), Random.Range(-0.3f, 0.3f));
        }
        yield return null;
        foreach (var firefly in fireflies)
        {
            firefly.gameObject.SetActive(true);
            firefly.MoveTo(fireflyPosDict[firefly]);
        }

        GameObject.Destroy(this.gameObject);
        MyEventSystem.Trigger(new DestroyObjectEvent { gameObject = this.gameObject });
    }
}
