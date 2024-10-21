using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Candle : MonoBehaviour
{
    public bool isLit = false;
    private CircleLight circleLight;
    [SerializeField] private SpriteRenderer fire;
    [SerializeField] private float radius;
    public bool litAtStart = true;

    public IEnumerator Start()
    {
        yield return null;
        circleLight = LevelManager.Instance.MakeCircleLight();

        circleLight.LightRadius = radius;
        Transform startPoint = circleLight.lightStartPoint;
        startPoint.SetParent(fire.transform);
        startPoint.localPosition = Vector3.zero;

        if (!litAtStart)
        {
            circleLight.enabled = false;
            fire.enabled = false;
        }
        isLit = litAtStart;
    }

    public void Light()
    {
        if (!isLit)
        {
            isLit = true;
            circleLight.enabled = true;
            fire.enabled = true;
            MyEventSystem.Trigger(new LightCandleEvent { candle = this });
        }
    }

    public void PutOut()
    {
        if (isLit)
        {
            isLit = false;
            circleLight.enabled = false;
            fire.enabled = false;
            MyEventSystem.Trigger(new PutoutCandleEvent { candle = this });
        }
    }

    public void SetRadius(float radius)
    {
        this.radius = radius;
        circleLight.LightRadius = radius;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<Match>() != null)
            Light();
    }
}
