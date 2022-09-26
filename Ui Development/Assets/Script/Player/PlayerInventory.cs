using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public InventoryObject Inventory;

    public void OnTriggerEnter(Collider other)
    {
        var item = other.GetComponent<CollectItem>();
        if (item)
        {
            Inventory.AddItem(item.item, 1);
            Destroy(other.gameObject);
        }
    }

    public void OnApplicationQuit()
    {
        Inventory.Container.Clear();
    }
}
