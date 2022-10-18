using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class StaticInterface : UserInterface
{
    public GameObject[] slots;

    public override void CreateItemSlots()
    {
        //New dictionary for item displayed 
        slotsOnInterface = new Dictionary<GameObject, InventorySlot>();

        //loop through the database
        for (int i = 0; i < inventory.GetItemFromSlot.Length; i++)
        {
            var obj = slots[i];

            AddSlotEvent(obj, EventTriggerType.PointerEnter, delegate { PointerEnterSlot(obj); });
            AddSlotEvent(obj, EventTriggerType.PointerExit, delegate { PointerExitSlot(obj); });
            AddSlotEvent(obj, EventTriggerType.BeginDrag, delegate { PointerStartDrag(obj); });
            AddSlotEvent(obj, EventTriggerType.Drag, delegate { PointerDragging(obj); });
            AddSlotEvent(obj, EventTriggerType.EndDrag, delegate { PointerExitDrag(obj); });

            inventory.GetItemFromSlot[i].slotDisplay = obj; 

            slotsOnInterface.Add(obj, inventory.GetItemFromSlot[i]);
        }
    }
}
