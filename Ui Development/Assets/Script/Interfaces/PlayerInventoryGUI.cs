using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class PlayerInventoryGUI : MonoBehaviour
{
   
    [Header("Reference")]
    public GameObject InventoryPerfab;
    public InventoryObject PlayerInventory;
    public MouseItem mouseItem = new MouseItem();

    [Header("Slot begins")]
    public int StartX;
    public int StartY;

    [Header("slot Settings")]
    public int XDistance;
    public int YDistance;
    public int MaxColumn;
    public int MaxRow;

    Dictionary<GameObject, InventorySlot> itemsDisplayed = new Dictionary<GameObject, InventorySlot>();

    private void Start()
    {
        CreateItemSlots();
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
                image.sprite = PlayerInventory.database.GetItem[_slot.Value.item.Id].UIDisplay;
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

    public void CreateItemSlots()
    {
        // use for list 
        //for (int i = 0; i < inventory.Container.Items.Count; i++)
        //{
        //    InventorySlot slot = inventory.Container.Items[i];

        //    var obj = Instantiate(InventoryPerfab, Vector3.zero, Quaternion.identity, transform);
        //    obj.transform.GetChild(0).GetComponentInChildren<Image>().sprite = inventory.database.GetItem[slot.item.Id].UIDisplay;
        //    obj.GetComponent<RectTransform>().localPosition = GetPosition(i);
        //    obj.GetComponentInChildren<TextMeshProUGUI>().text = slot.amount.ToString("n0");
        //    itemsDisplayed.Add(slot, obj);
        //}

        // use for array
        itemsDisplayed = new Dictionary<GameObject, InventorySlot>();
        for (int i = 0; i < PlayerInventory.Container.Items.Length; i++)
        {
            
            var obj = Instantiate(InventoryPerfab, Vector3.zero, Quaternion.identity, transform);
            obj.GetComponent<RectTransform>().localPosition = GetPosition(i);

            AddSlotEvent(obj, EventTriggerType.PointerEnter, delegate { PointerEnterSlot(obj); });
            AddSlotEvent(obj, EventTriggerType.PointerExit, delegate { PointerExitSlot(obj); });
            AddSlotEvent(obj, EventTriggerType.BeginDrag, delegate { PointerStartDrag(obj); });
            AddSlotEvent(obj, EventTriggerType.Drag, delegate { PointerDragging(obj); });
            AddSlotEvent(obj, EventTriggerType.EndDrag, delegate { PointerExitDrag(obj); });

            itemsDisplayed.Add(obj, PlayerInventory.Container.Items[i]);
        }
    }

    //Add event system to new obj in the Inventory
    private void AddSlotEvent(GameObject obj, EventTriggerType type,UnityAction<BaseEventData> action)
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
        mouseItem.hoverObj = obj;
        if (itemsDisplayed.ContainsKey(obj))
        {
            mouseItem.hoverItem = itemsDisplayed[obj];
        }
    }

    //when exit point
    public void PointerExitSlot(GameObject obj)
    {
        mouseItem.hoverObj = null;
        mouseItem.hoverItem = null;
  
    }

    //when start to drag
    public void PointerStartDrag(GameObject obj)
    {
        var mouseObj = new GameObject();
        var rt = mouseObj.AddComponent<RectTransform>();
        rt.sizeDelta = new Vector2(50,50);
        mouseObj.transform.SetParent(transform.parent);

        if (itemsDisplayed[obj].ID >= 0)
        {
            var image = mouseObj.AddComponent<Image>();
            image.sprite = PlayerInventory.database.GetItem[itemsDisplayed[obj].ID].UIDisplay;
            image.raycastTarget = false;
        }
        mouseItem.obj = mouseObj;
        mouseItem.item = itemsDisplayed[obj];
    }

    //when dragging
    public void PointerDragging(GameObject obj)
    {
        if (mouseItem.obj != null)
        {
            mouseItem.obj.GetComponent<RectTransform>().position = Input.mousePosition;
        }
    }

    //when exit dragging
    public void PointerExitDrag(GameObject obj)
    {
        if (mouseItem.hoverObj)
        {
            PlayerInventory.MoveItem(itemsDisplayed[obj], itemsDisplayed[mouseItem.hoverObj]);
        }
        else
        {
            PlayerInventory.RemoveItem(itemsDisplayed[obj].item);
        }
        Destroy(mouseItem.obj);
        mouseItem.item = null;
    }

    public Vector3 GetPosition(int i)
    {
        return new Vector3(StartX + (XDistance * (i % MaxColumn)), StartY + (-YDistance * (i / MaxColumn)), 0f);

    }
}

//public class MouseItem
//{
//    public GameObject obj;
//    public InventorySlot item;
//    public InventorySlot hoverItem;
//    public GameObject hoverObj;
//}