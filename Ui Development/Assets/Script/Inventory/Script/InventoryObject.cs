using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEditor;
using System.Runtime.Serialization;
using System;

public enum InterfaceType
{
    Inventory,
    Equipment,
    Chest
}

[CreateAssetMenu(fileName = "New Inventory", menuName = "Inventory System/Inventory")]
public class InventoryObject : ScriptableObject
{
    //calling
    public string savePath;
    public ItemSavingDatabase database;
    public InterfaceType TypeOfInterface;
    public Inventory Container;
    public InventorySlot[] GetItemFromSlot { get { return Container.InvenSlot;} }

    public bool AddItem(Item _item, int _amount)
    {
        //check is there aviable slot for the item
        if (EmptySlotCount <= 0)
            return false;
        InventorySlot slot = FindItemOnInventory(_item);
        if (!database.ItemObjects[_item.Id].stackable || slot == null)
        {
            SetEmptySlot(_item, _amount);
            return true;
        }
        slot.AddAmount(_amount);
        return true;
    }

    private InventorySlot FindItemOnInventory(Item _item)
    {
        for (int i = 0; i <GetItemFromSlot.Length; i++)
        {
            if(GetItemFromSlot[i].item.Id == _item.Id)
            {
                return GetItemFromSlot[i];
            }
        }
        return null;
    }

    public int EmptySlotCount
    {
        get
        {
            int counter = 0;
            for (int i = 0; i <GetItemFromSlot.Length; i++)
            {
                if (GetItemFromSlot[i].item.Id <= -1)
                    counter++;
            }
            return counter;
        }
    }

    public InventorySlot SetEmptySlot(Item _item, int _amount)
    {
        for (int i = 0; i <GetItemFromSlot.Length; i++)
        {
            if(GetItemFromSlot[i].item.Id <= -1)
            {
               GetItemFromSlot[i].UpdateSlot(_item, _amount);
                return GetItemFromSlot[i];
            }
        }
        //set yp a function if inventory is full
        return null;
    }

    public void SwapItems(InventorySlot item1, InventorySlot item2)
    {
        if (item2.isSlotFree(item1.itemData) && item1.isSlotFree(item2.itemData))
        {
            InventorySlot temp = new InventorySlot(item2.item, item2.amount);
            item2.UpdateSlot(item1.item, item1.amount);
            item1.UpdateSlot(temp.item, temp.amount);
        }
    }

    public void RemoveItem(Item _item)
    {
        for (int i = 0; i <GetItemFromSlot.Length; i++)
        {
            if(Container.InvenSlot[i].item == _item)
            {
               GetItemFromSlot[i].UpdateSlot( null, 0);
            }
        }
    }

    [ContextMenu("Save")]
    public void SaveItem()
    {
        IFormatter formatter = new BinaryFormatter();
        Stream stream = new FileStream(string.Concat(Application.persistentDataPath, savePath), FileMode.Create, FileAccess.Write);
        formatter.Serialize(stream, Container);
        stream.Close();
    }

    [ContextMenu("Load")]
    public void LoadItem()
    {
        if (File.Exists(string.Concat(Application.persistentDataPath, savePath)))
        {
            IFormatter formatter = new BinaryFormatter();
            Stream stream = new FileStream(string.Concat(Application.persistentDataPath, savePath), FileMode.Open, FileAccess.Read);
            Inventory newContainer = (Inventory)formatter.Deserialize(stream);
            for (int i = 0; i <GetItemFromSlot.Length; i++)
            {
               GetItemFromSlot[i].UpdateSlot(newContainer.InvenSlot[i].item, newContainer.InvenSlot[i].amount);
            }
            stream.Close();
        }
    }

    [ContextMenu("Clear")]
    public void clear()
    {
        Container.Clear();
    }
}

//Number of Slot In Inventory
[Serializable]
public class Inventory
{   
    public InventorySlot[] InvenSlot = new InventorySlot[25];
    public void Clear()
    {
        for (int i = 0; i < InvenSlot.Length; i++)
        {
            InvenSlot[i].removeItem();
        }
    }
}

// useing delegate to pass method as arugement to...
public delegate void SlotUpdated(InventorySlot _InvSlot);

[Serializable]
public class InventorySlot
{
    [Header("Slot Purpose")]
    public ItemType[] AllowedItems = new ItemType[0];

    //public var but not visable in editor
    [NonSerialized]
    public UserInterface parent;
    [NonSerialized]
    public GameObject slotDisplay;
    [NonSerialized]
    public SlotUpdated OnSlotAfterUpdate;
    [NonSerialized]
    public SlotUpdated OnSlotBeforeUpdate;

    [Header("Item Details")]
    public Item item = new Item();
    public int amount;
    
    public ItemData itemData
    {
        get
        {
            if(item.Id >= 0)
            {
                return parent.inventory.database.ItemObjects[item.Id];
            }
            return null;
        }
    }

    public InventorySlot()
    {
        UpdateSlot(new Item(), 0);
    }

    public InventorySlot(Item _item, int _amount)
    {
        UpdateSlot(_item, _amount);
    }

    public void UpdateSlot(Item _item, int _amount)
    {
        if (OnSlotBeforeUpdate != null)
            OnSlotBeforeUpdate.Invoke(this);

        item = _item;
        amount = _amount;

        if (OnSlotAfterUpdate != null)
            OnSlotAfterUpdate.Invoke(this);
    }

    public void removeItem()
    {
        UpdateSlot(new Item(), 0);
    }

    public void AddAmount(int value)
    {
        UpdateSlot(item, amount += value);
    }

    public bool isSlotFree(ItemData _itemData)
    {
        if(AllowedItems.Length <= 0 || _itemData == null || _itemData.itemData.Id < 0)
        {
            return true;
        }

        for (int i = 0; i < AllowedItems.Length; i++)
        {
            if (_itemData.type == AllowedItems[i])
            {
                return true;
            }
        }
        return false;
    }
}