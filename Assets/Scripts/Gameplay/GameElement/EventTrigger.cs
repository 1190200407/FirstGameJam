using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventTrigger : MonoBehaviour
{
    private Collider2D cld;
    private bool isInTrigger = false;

    void Awake()
    {
        cld = GetComponent<Collider2D>();
    }

    protected virtual void Update()
    {
        if (cld.OverlapPoint(LevelManager.Instance.player.transform.position))
        {
            if (!isInTrigger)
            {
                OnPlayerEnter();
                isInTrigger = true;
            }
            else
            {
                OnPlayerStay();
            }
        }
        else
        {
            if (isInTrigger)
            {
                OnPlayerExit();
                isInTrigger = false;
            }
        }
    }

    public virtual void OnPlayerEnter()
    {
    }

    public virtual void OnPlayerStay()
    { 
    }

    public virtual void OnPlayerExit()
    {
    }
}
