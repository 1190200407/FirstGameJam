using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MirrorMaker : GameItemScript
{
    public GameObject mirrorObj;
    private PlayerMovement mov;
    private InputHandler input;
    private PlayerAnimator anim;
    private Vector3 rightDir = new Vector3(0.5f, 0f, 0f);
    private Vector3 originalScl;

    public override void OnCreate()
    {
        mirrorObj = GameObject.Instantiate(data.relativePrefab);
        mirrorObj.transform.position = new Vector3(-100f, 0f, 0f);
        if (!LevelManager.Instance.testMode) mirrorObj.transform.SetParent(LevelManager.Instance.levelMap.transform);
        mirrorObj.gameObject.SetActive(false);
        originalScl = mirrorObj.transform.localScale;
        MyEventSystem.Trigger(new CreateObjectEvent { gameObject = mirrorObj });

        mov = LevelManager.Instance.player;
        input = LevelManager.Instance.input;
        anim = LevelManager.Instance.animator;
    }

    public override void OnActivate()
    {
        anim.Throw();
        mirrorObj.gameObject.SetActive(true);
        if (mov.IsFacingRight)
        {
            mirrorObj.transform.position = mov.transform.position + rightDir;
            mirrorObj.transform.localScale = originalScl;

            // 如果按住下则向下旋转
            if (input.movementInput.y < 0)
            {
                mirrorObj.transform.rotation = Quaternion.Euler(0, 0, -30);
            }
            else
            {
                mirrorObj.transform.rotation = Quaternion.Euler(0, 0, 30);
            }
        }
        else
        {
            mirrorObj.transform.position = mov.transform.position - rightDir;
            mirrorObj.transform.localScale = new Vector3(-originalScl.x, originalScl.y, originalScl.z);

            // 如果按住下则向下旋转
            if (input.movementInput.y < 0)
            {
                mirrorObj.transform.rotation = Quaternion.Euler(0, 0, 30);
            }
            else
            {
                mirrorObj.transform.rotation = Quaternion.Euler(0, 0, -30);
            }
        }
    }
}
