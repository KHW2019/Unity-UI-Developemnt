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

    public Dictionary<GameObject, InventorySlot> slotsOnInterface = new Dictionary<GameObject, InventorySlot>();

    private void Start()
    {
        //reconstruct the inventory gui to make sure the item and slot numbers are correct
        for (int i = 0; i < inventory.GetItemFromSlot.Length; i++)
        {
            inventory.GetItemFromSlot[i].parent = this;
            inventory.GetItemFromSlot[i].OnSlotAfterUpdate += OnSlotUpdate;
        }

        CreateItemSlots();
        AddSlotEvent(gameObject, EventTriggerType.PointerEnter, delegate { ItemEnterUi(gameObject); });
        AddSlotEvent(gameObject, EventTriggerType.PointerExit, delegate { ItemExitUi(gameObject); });
    }

    //Update is called once per frame
    void Update()
    {
        //slotsOnInterface.UpdateSlotDisplay();
    }

    public void OnSlotUpdate(InventorySlot _InvSlot)
    {
        Image image = _InvSlot.slotDisplay.transform.GetChild(0).GetComponentInChildren<Image>();
        TextMeshProUGUI textMeshPro = _InvSlot.slotDisplay.GetComponentInChildren<TextMeshProUGUI>();

        if (_InvSlot.item.Id >= 0)
        {
            image.sprite = _InvSlot.itemData.UIDisplay;
            image.color = new Color(1, 1, 1, 1);
            textMeshPro.text = _InvSlot.amount == 1 ? "" : _InvSlot.amount.ToString("n0");
        }
        else
        {
            image.sprite = null;
            image.color = new Color(1, 1, 1, 0);
            textMeshPro.text = "";
        }
    }

    public abstract void CreateItemSlots();

    public GameObject CreateTempItem(GameObject obj)
    {
        GameObject tempItem = null;
        if (slotsOnInterface[obj].item.Id >= 0)
        {
            tempItem = new GameObject();
            var rt = tempItem.AddComponent<RectTransform>();
            rt.sizeDelta = new Vector2(50, 50);
            tempItem.transform.SetParent(transform.parent);
            var image = tempItem.AddComponent<Image>();
            image.sprite = slotsOnInterface[obj].itemData.UIDisplay;
            image.raycastTarget = false;
        }
        return tempItem;
    }

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
        MouseData.slotHoveredOver = obj;
    }

    //when exit point
    public void PointerExitSlot(GameObject obj)
    {
        MouseData.slotHoveredOver = null;
    }

    //when item inside the ui area
    private void ItemEnterUi(GameObject gameObject)
    {
        MouseData.InterfaceMouseIsOver = gameObject.GetComponent<UserInterface>();
    }

    //when item is outside the ui area
    private void ItemExitUi(GameObject gameObject)
    {
        MouseData.InterfaceMouseIsOver = null;
    }

    //when start to drag
    public void PointerStartDrag(GameObject obj)
    {
        MouseData.tempItemBeingDragged = CreateTempItem(obj);
    }

    //when dragging
    public void PointerDragging(GameObject obj)
    {
        if (MouseData.tempItemBeingDragged != null)
        {
            MouseData.tempItemBeingDragged.GetComponent<RectTransform>().position = Input.mousePosition;
        }
    }

    //when exit dragging
    public void PointerExitDrag(GameObject obj)
    {
        Destroy(MouseData.tempItemBeingDragged);
        if(MouseData.InterfaceMouseIsOver == null)
        {
            slotsOnInterface[obj].removeItem();
            return;
        }
        if (MouseData.slotHoveredOver)
        {
            InventorySlot mouseHoverSlotData = MouseData.InterfaceMouseIsOver.slotsOnInterface[MouseData.slotHoveredOver];
            inventory.SwapItems(slotsOnInterface[obj], mouseHoverSlotData);
        }
    }
}

public static class MouseData
{
    public static UserInterface InterfaceMouseIsOver;
    public static GameObject tempItemBeingDragged;
    public static GameObject slotHoveredOver;
}

//public static class ExtensionMethod
//{
//    public static void UpdateSlotDisplay(this Dictionary<GameObject, InventorySlot> _slotsOnInterface)
//    {
//        foreach (KeyValuePair<GameObject, InventorySlot> _slot in _slotsOnInterface)
//        {
//            Image image = _slot.Key.transform.GetChild(0).GetComponentInChildren<Image>();
//            TextMeshProUGUI textMeshPro = _slot.Key.GetComponentInChildren<TextMeshProUGUI>();

//            if (_slot.Value.item.Id >= 0)
//            {
//                image.sprite = _slot.Value.itemData.UIDisplay;
//                image.color = new Color(1, 1, 1, 1);
//                textMeshPro.text = _slot.Value.amount == 1 ? "" : _slot.Value.amount.ToString("n0");
//            }
//            else
//            {
//                image.sprite = null;
//                image.color = new Color(1, 1, 1, 0);
//                textMeshPro.text = "";
//            }
//        }
//    }
//}