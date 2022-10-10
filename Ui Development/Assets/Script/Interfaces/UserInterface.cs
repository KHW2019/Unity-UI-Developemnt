using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using System;

public abstract class UserInterface : MonoBehaviour
{
    
    [Header("Reference")]
    public InventoryObject inventory;
    public PlayerInventory player;

    public Dictionary<GameObject, InventorySlot> itemsDisplayed = new Dictionary<GameObject, InventorySlot>();

    private void Start()
    {
        //restruct the inventory gui to make sure the item and slot numbers are current
        for (int i = 0; i < inventory.Container.Items.Length; i++)
        {
            inventory.Container.Items[i].parent = this;
        }
        CreateItemSlots();
        AddSlotEvent(gameObject, EventTriggerType.PointerEnter, delegate { ItemEnterUi(gameObject); });
        AddSlotEvent(gameObject, EventTriggerType.PointerExit, delegate { ItemExitUi(gameObject); });
    }

    // Update is called once per frame
    void Update()
    {

        UpdateSlots();
    }


    public void UpdateSlots()
    {


        // using it for list
        //for (int i = 0; i < inventory.Container.Items.Count; i++)
        //{

        //    InventorySlot slot = inventory.Container.Items[i];

        //    //if the item is already in the player inventory
        //    if (itemsDisplayed.ContainsKey(slot))
        //    {
        //        itemsDisplayed[slot].GetComponentInChildren<TextMeshProUGUI>().text = slot.amount.ToString("n0");
        //    }
        //    else
        //    {
        //        var obj = Instantiate(InventoryPerfab, Vector3.zero, Quaternion.identity, transform);
        //        obj.transform.GetChild(0).GetComponentInChildren<Image>().sprite = inventory.database.GetItem[slot.item.Id].UIDisplay;
        //        obj.GetComponent<RectTransform>().localPosition = GetPosition(i);
        //        obj.GetComponentInChildren<TextMeshProUGUI>().text = slot.amount.ToString("n0");
        //        itemsDisplayed.Add(slot, obj);
        //    }
        //}

        //function to Update icon in array 
        foreach (KeyValuePair<GameObject, InventorySlot> _slot in itemsDisplayed)
        {
            Image image = _slot.Key.transform.GetChild(0).GetComponentInChildren<Image>();
            TextMeshProUGUI textMeshPro = _slot.Key.GetComponentInChildren<TextMeshProUGUI>();

            if (_slot.Value.ID >= 0)
            {
                image.sprite = inventory.database.GetItem[_slot.Value.item.Id].UIDisplay;
                image.color = new Color(1, 1, 1, 1);
                textMeshPro.text = _slot.Value.amount == 1 ? "" : _slot.Value.amount.ToString("n0");
            }
            else
            {
                image.sprite = null;
                image.color = new Color(1, 1, 1, 0);
                textMeshPro.text = "";
            }
        }

    }

    public abstract void CreateItemSlots();

    //Add event system to new obj in the Inventory
    protected void AddSlotEvent(GameObject obj, EventTriggerType type, UnityAction<BaseEventData> action)
    {
        EventTrigger trigger = obj.GetComponent<EventTrigger>();
        var eventTrigger = new EventTrigger.Entry();
        eventTrigger.eventID = type;
        eventTrigger.callback.AddListener(action);
        trigger.triggers.Add(eventTrigger);
    }

    //All Events of pointer pointing at objects 
    //when pointed
    public void PointerEnterSlot(GameObject obj)
    {
        player.mouseItem.hoverObj = obj;
        if (itemsDisplayed.ContainsKey(obj))
        {
            player.mouseItem.hoverItem = itemsDisplayed[obj];
        }
    }

    //when exit point
    public void PointerExitSlot(GameObject obj)
    {
        player.mouseItem.hoverObj = null;
        player.mouseItem.hoverItem = null;

    }

    private void ItemEnterUi(GameObject gameObject)
    {
        player.mouseItem.UI = gameObject.GetComponent<UserInterface>();
    }

    private void ItemExitUi(GameObject gameObject)
    {
        player.mouseItem.UI = null;
    }

    //when start to drag
    public void PointerStartDrag(GameObject obj)
    {
        var mouseObj = new GameObject();
        var rt = mouseObj.AddComponent<RectTransform>();
        rt.sizeDelta = new Vector2(50, 50);
        mouseObj.transform.SetParent(transform.parent);

        if (itemsDisplayed[obj].ID >= 0)
        {
            var image = mouseObj.AddComponent<Image>();
            image.sprite = inventory.database.GetItem[itemsDisplayed[obj].ID].UIDisplay;
            image.raycastTarget = false;
        }
        player.mouseItem.obj = mouseObj;
        player.mouseItem.item = itemsDisplayed[obj];
    }

    //when dragging
    public void PointerDragging(GameObject obj)
    {
        if (player.mouseItem.obj != null)
        {
            player.mouseItem.obj.GetComponent<RectTransform>().position = Input.mousePosition;
        }
    }

    //when exit dragging
    public void PointerExitDrag(GameObject obj)
    {
        var itemOnMouse = player.mouseItem;
        var mouseHoverItem = itemOnMouse.hoverItem;
        var mouseHoverObj = itemOnMouse.hoverObj;
        var GetItemObject = inventory.database.GetItem;

        if (itemOnMouse.UI != null)
        {
            if (mouseHoverObj)
                if (mouseHoverItem.isSlotFree(GetItemObject[itemsDisplayed[obj].ID]) && (mouseHoverItem.item.Id <= -1 || (mouseHoverItem.item.Id >= 0 && itemsDisplayed[obj].isSlotFree(GetItemObject[mouseHoverItem.item.Id]))))
                    inventory.MoveItem(itemsDisplayed[obj], mouseHoverItem.parent.itemsDisplayed[itemOnMouse.hoverObj]);
        }
        else
        {
            inventory.RemoveItem(itemsDisplayed[obj].item);
        }

        Destroy(itemOnMouse.obj);
        itemOnMouse.item = null;
    }
}

public class MouseItem
{
    public UserInterface UI;
    public GameObject obj;
    public InventorySlot item;
    public InventorySlot hoverItem;
    public GameObject hoverObj;
}