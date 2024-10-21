using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetItemTrigger : MonoBehaviour
{
    public GameItemData gameItem;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("NormalState"))
        {
            other.gameObject.GetComponent<PlayerInventory>().GetItem(gameItem);
            Destroy(this.gameObject);
        }
    }
}
