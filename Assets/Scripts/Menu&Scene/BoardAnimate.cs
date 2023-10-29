using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.VersionControl;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;


public class BoardAnimate : MonoBehaviour
{ 
    public CanvasGroup canvasGroup;
    void Start() 
    {
        canvasGroup.DOFade(1, 1).OnComplete(() =>
        {
            canvasGroup.DOFade(1, 8).OnComplete(() =>
            {
                canvasGroup.DOFade(0, 2);
            });
        });
       
    }

}
