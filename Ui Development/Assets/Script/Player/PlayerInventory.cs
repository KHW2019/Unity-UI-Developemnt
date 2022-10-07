using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    [Header("Reference")]
    public InventoryObject inventory;
    public MouseItem mouseItem = new MouseItem();

    [Header("Input")]
    public KeyCode SaveItem = KeyCode.Alpha9;
    public KeyCode LoadItem = KeyCode.Alpha0;


    private void Update()
    {
        if (Input.GetKeyDown(SaveItem))
        {
            inventory.SaveItem();
            Debug.Log("Item has been Saved");
        }
        if (Input.GetKeyDown(LoadItem))
        {
            inventory.LoadItem();
            Debug.Log("Item has been load");
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        var item = other.GetComponent<CollectItem>();
        if (item)
        {
            inventory.AddItem(new Item(item.item), 1);
            Destroy(other.gameObject);
        }
    }

    //it will clear player inventory when application is close
    public void OnApplicationQuit()
    {
        inventory.Container.Items = new InventorySlot[25];
    }
}
