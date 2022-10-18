using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item DataBase", menuName = "Inventory System/Items/Data")]
public class ItemSavingDatabase : ScriptableObject, ISerializationCallbackReceiver
{
    public ItemData[] ItemObjects;

    //Check is the id is current compare to database
    [ContextMenu("Update ID")]
    public void UpdateID()
    {
        for (int i = 0; i < ItemObjects.Length; i++)
        {
            if (ItemObjects[i].itemData.Id != i)
            {
                ItemObjects[i].itemData.Id = i;
                
            }
        }
    }
    public void OnAfterDeserialize()
    {
        UpdateID();
    }

    public void OnBeforeSerialize()
    {
        
    }


}
