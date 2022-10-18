using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    [Header("Reference")]
    public InventoryObject inventory;
    public InventoryObject Equipment;
    

    [Header("Input")]
    public KeyCode SaveItem = KeyCode.Alpha9;
    public KeyCode LoadItem = KeyCode.Alpha0;

    public Attribute[] ListOfAttributes;
    

    private void Start()
    { 
        
        for (int i = 0; i < ListOfAttributes.Length; i++)
        {
            ListOfAttributes[i].SetParent(this);
        }

        for (int i = 0; i < Equipment.GetItemFromSlot.Length; i++)
        {
            Equipment.GetItemFromSlot[i].OnSlotBeforeUpdate += OnSlotBeforeUpdate;
            Equipment.GetItemFromSlot[i].OnSlotAfterUpdate += OnSlotAfterUpdate;
        }

    }

    public void OnSlotBeforeUpdate(InventorySlot _InvSlot)
    {
        if(_InvSlot.itemData == null)
        {
            return;
        }
        switch (_InvSlot.parent.inventory.TypeOfInterface)
        {
            case InterfaceType.Inventory:
                break;
            case InterfaceType.Equipment:
                print(string.Concat("Removed", _InvSlot.itemData, "on", _InvSlot.parent.inventory.TypeOfInterface, ", Allowed Items:", String.Join(",", _InvSlot.AllowedItems)));

                for (int i = 0; i < _InvSlot.item.buffs.Length; i++)
                {
                    for (int j = 0; j < ListOfAttributes.Length; j++)
                    {
                        if (ListOfAttributes[j].type == _InvSlot.item.buffs[i].attribute)
                        {
                            ListOfAttributes[j].value.ReMoveModifer(_InvSlot.item.buffs[i]);
                        }
                    }
                }

                break;
            case InterfaceType.Chest:
                break;
            default:
                break;
        }
    }

    public void OnSlotAfterUpdate(InventorySlot _InvSlot)
    {
        if (_InvSlot.itemData == null)
        {
            return;
        }
        switch (_InvSlot.parent.inventory.TypeOfInterface)
        {
            case InterfaceType.Inventory:
                break;
            case InterfaceType.Equipment:
                print(string.Concat("Placed", _InvSlot.itemData, "on", _InvSlot.parent.inventory.TypeOfInterface, ", Allowed Items:", String.Join(",", _InvSlot.AllowedItems)));

                for (int i = 0; i < _InvSlot.item.buffs.Length; i++)
                {
                    for (int j = 0; j < ListOfAttributes.Length; j++)
                    {
                        if (ListOfAttributes[j].type == _InvSlot.item.buffs[i].attribute)
                        {
                            ListOfAttributes[j].value.AddModifer(_InvSlot.item.buffs[i]);
                        }
                    }
                }
                break;
            case InterfaceType.Chest:
                break;
            default:
                break;
        }
    }

    

    private void Update()
    {
        if (Input.GetKeyDown(SaveItem))
        {
            inventory.SaveItem();
            Equipment.SaveItem();
            Debug.Log("Item has been Saved");
        }
        if (Input.GetKeyDown(LoadItem))
        {
            inventory.LoadItem();
            Equipment.LoadItem();
            Debug.Log("Item has been load");
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        var item = other.GetComponent<CollectItem>();

        if (item)
        {
            Item _item = new Item(item.item);

            if(inventory.AddItem(_item, 1))
            {
                Destroy(other.gameObject);
            }
        }
    }

    public void AttributeModified(Attribute attribute)
    {
        Debug.Log(string.Concat(attribute.type, " was updated! Value is now ", attribute.value.ModifiedValue));
    }

    //it will clear player inventory when application is close
    public void OnApplicationQuit()
    {
        inventory.clear();
        Equipment.clear();
    }
}

