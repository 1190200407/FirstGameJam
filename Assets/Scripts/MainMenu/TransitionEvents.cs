using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionEvents : MonoBehaviour
{
    public void OnCloseAnimtionEnd()
    {
        SceneLoadManager.Instance.OnCloseAnimtionEnd();
    }
}
