using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawAimLine : MonoBehaviour
{
    public LineRenderer lineRenderer;

    private bool _isAiming = false;
    public bool IsAiming
    {
        get
        {
            return _isAiming;
        }
        set
        {
            lineRenderer.enabled = value;
            _isAiming = value;
        }
    }

    [Header("瞄准点的数量")]
    public int aimPointsCount = 20;

    [Header("瞄准点的靠近程度")]
    public float aimPointsGap = 0.1f;

    [Header("线最远可延伸的距离")]
    public float aimPointMaxDistance = 50f;

    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    private void Update()
    {
        if (IsAiming)
        {
            List<Vector3> positions = new(); 

            //获取初始位置、初速度、重力
            WaterCtr waterCtr = GameCtr.instance.waterCtr;
            Vector3 initialPosition = waterCtr.transform.position;
            Vector3 initialSpeed = waterCtr.WaterInitialSpeed * waterCtr.WaterDir;
            float gravity = waterCtr.waterGravity;

            positions.Add(initialPosition);
            for (int i = 1; i <= aimPointsCount; i++)
            {
                float time = aimPointsGap * i;
                float timePow = time * time;

                float posX = initialPosition.x + initialSpeed.x * time;
                float posY = initialPosition.y + initialSpeed.y * time - 0.5f * Physics2D.gravity.magnitude * gravity * timePow;

                if (posY - initialPosition.y < 0 && 
                    Mathf.Pow(posX - initialPosition.x, 2) + Mathf.Pow(posY - initialPosition.y, 2) > aimPointMaxDistance)
                    break;

                positions.Add(new Vector3(posX, posY, initialPosition.z));
            }

            lineRenderer.positionCount = positions.Count;
            lineRenderer.SetPositions(positions.ToArray());
        }
    }
}
