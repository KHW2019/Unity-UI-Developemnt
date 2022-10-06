using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item DataBase", menuName = "Inventory System/Items/Data")]
public class ItemSavingDatabase : ScriptableObject, ISerializationCallbackReceiver
{
    public ItemData[] Items;
    //Two Dictionary to avoid system overloading when going through all item in the system.
    //public Dictionary<ItemData, int> GetId = new Dictionary<ItemData, int>();
    public Dictionary<int, ItemData > GetItem = new Dictionary<int, ItemData>();

    public void OnAfterDeserialize()
    {
        //GetId = new Dictionary<ItemData, int>();
        GetItem = new Dictionary<int, ItemData>();
        for (int i = 0; i < Items.Length; i++)
        {
            //GetId.Add(Items[i], i);
            Items[i].Id = i;
            GetItem.Add(i, Items[i] );
        }
    }

    public void OnBeforeSerialize()
    {
        GetItem = new Dictionary<int, ItemData>();
    }
}
