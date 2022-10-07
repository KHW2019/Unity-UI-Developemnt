using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

//Dynamic Class is use to update frequently when new items are added to the array
public class DynamicInterface : UserInterface
{
    [Header("Reference")]
    public GameObject InventoryPerfab;

    [Header("Slot begins")]
    public int StartX;
    public int StartY;

    [Header("slot Settings")]
    public int XDistance;
    public int YDistance;
    public int MaxColumn;

    public override void CreateItemSlots()
    {
        
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
    private Vector3 GetPosition(int i)
    {
        return new Vector3(StartX + (XDistance * (i % MaxColumn)), StartY + (-YDistance * (i / MaxColumn)), 0f);

    }
}
