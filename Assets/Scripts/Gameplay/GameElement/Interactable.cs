using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    private bool canInteract;
    public bool CanInteracct
    {
        set
        {
            canInteract = value;
        }
    }
    public bool isInteracting = false;
    public GameObject interactionHint;
    public SpriteRenderer spriteRenderer;

    protected virtual void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("NormalState") || other.gameObject.layer == LayerMask.NameToLayer("SoulState"))
        {
            interactionHint?.SetActive(false);
            if (LevelManager.Instance.player != null && LevelManager.Instance.player.encounterInteraction == this)
            {
                LevelManager.Instance.player.encounterInteraction = null;
            }
        }
    }
    
    protected virtual void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("NormalState"))
        {
            if (LevelManager.Instance.player.encounterInteraction == null)
            {
                LevelManager.Instance.player.encounterInteraction = this;
                interactionHint?.SetActive(true);
            }
            else if (LevelManager.Instance.player.encounterInteraction != this)
            {
                interactionHint?.SetActive(false);
            }
        }
    }

    public virtual void Interact()
    {
    }
}
