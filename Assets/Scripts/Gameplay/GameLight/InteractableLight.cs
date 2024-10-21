using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class InteractableLight : MonoBehaviour
{   
    public List<Vector3> points;
    public Light2D light2D;
    public new PolygonCollider2D collider2D;
    public LayerMask shadowMask;

    #region Mirror
    public bool canReflect = true;
    [HideInInspector] public bool isMirrorLight = false;
    [HideInInspector] public Mirror reflectMirror;
    [HideInInspector] public InteractableLight reflectedLight;
    [HideInInspector] public int reflectedCnt = 0;
    #endregion

    protected virtual void Awake()
    {
        points = new List<Vector3>();
    }

    protected virtual void OnEnable()
    {
        light2D.enabled = true;
        collider2D.enabled = true;
    }

    protected virtual void OnDisable()
    {
        if (light2D == null || collider2D == null) return;
        light2D.enabled = false;
        collider2D.enabled = false;
    }

    protected virtual void Update()
    {
        if (light2D == null || collider2D == null) return;

        UpdateLightPoints();
    }

    public virtual void UpdateLightPoints()
    {
    }
}
