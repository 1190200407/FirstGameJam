using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheaterLight : Interactable
{
    public FanshapedLight fanLight;
    public Transform pivotPoint;
    public GameObject rotateHint;
    [SerializeField] private bool isClockwise;
    [SerializeField] private float rotateSpeed = 5f;
    [SerializeField] private float minPivot = -15f;
    [SerializeField] private float maxPivot = 15f;
    [SerializeField] private float curPivot = 0f;

    protected override void OnTriggerStay2D(Collider2D other)
    {
        base.OnTriggerStay2D(other);
        if (isInteracting)
        {
            // 获取y轴输入
            float yInput = LevelManager.Instance.input.movementInput.y;
            
            float temp = curPivot;
            curPivot += Time.deltaTime * rotateSpeed * yInput;
            curPivot = Mathf.Clamp(curPivot, minPivot, maxPivot);
            float angleChange = curPivot - temp;
            if (isClockwise) angleChange = -angleChange;
            
            // 旋转物体
            Vector2 pivotPosition = pivotPoint.position;

            // 旋转 fanLight 的三个点
            if (fanLight != null)
            {
                fanLight.startPoint.position = MyMathUtil.RotatePoint(fanLight.startPoint.position, pivotPosition, angleChange);
                fanLight.cutPoint1.position = MyMathUtil.RotatePoint(fanLight.cutPoint1.position, pivotPosition, angleChange);
                fanLight.cutPoint2.position = MyMathUtil.RotatePoint(fanLight.cutPoint2.position, pivotPosition, angleChange);
            }
        }
    }

    public override void Interact()
    {
        isInteracting = !isInteracting;
        LevelManager.Instance.player.IsInteracting = isInteracting;
        rotateHint?.SetActive(isInteracting);
    }
}
