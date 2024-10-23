using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Match : MonoBehaviour
{
    public CircleLight light2D;

    float surviveTime = 5f;
    bool isDead = false;
    public float radius = 1.5f;
    public float destroyTime = 10f;

    void OnEnable()
    {
        light2D = LevelManager.Instance.MakeCircleLight();
        Transform startPoint = light2D.lightStartPoint;
        startPoint.SetParent(transform);
        startPoint.localPosition = Vector3.zero;
        light2D.LightRadius = radius;
    }

    public void PutOut()
    {
        destroyTime -= surviveTime;
        surviveTime = 0f;
    }

    void Update()
    {
        destroyTime -= Time.deltaTime;
        if (isDead && light2D != null)
        {
            radius = Mathf.Lerp(radius, 0, 5 * Time.deltaTime);

            if (radius < 0.1f)
            {
                Destroy(light2D.gameObject);
                light2D = null;
            }
            else
            {
                light2D.LightRadius = radius;
            }
        }
        else
        {
            surviveTime -= Time.deltaTime;
            if (surviveTime <= 0f)
            {
                isDead = true;
            }
            else
            {
                //TODO 火光摇曳
            }
        }
    }
}
