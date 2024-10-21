using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using DG.Tweening;

public class Firefly : MonoBehaviour
{
    protected CircleLight circleLight;
    protected Light2D light2D;
    [SerializeField] protected float radius;

    [SerializeField] protected float moveSpeed;
    [SerializeField] protected Ease moveEase;

    public virtual void Start()
    {
        circleLight = LevelManager.Instance.MakeCircleLight();
        light2D = GetComponent<Light2D>();

        circleLight.LightRadius = radius;
        Transform startPoint = circleLight.lightStartPoint;
        startPoint.SetParent(transform.GetChild(0));
        startPoint.localPosition = Vector3.zero;
        light2D.pointLightInnerRadius = radius;
        light2D.pointLightOuterRadius = radius;
    }

    public void SetRadius(float radius)
    {
        this.radius = radius;
        circleLight.LightRadius = radius;
        light2D.pointLightInnerRadius = radius;
        light2D.pointLightOuterRadius = radius;
    }
}
