using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEditor;

[CreateAssetMenu(fileName = "New Inventory", menuName = "Inventory System/Inventory")]
public class InventoryObject : ScriptableObject, ISerializationCallbackReceiver
{
    public string savePath;
    private ItemSavingDatabase database;
    public List<InventorySlot> Container = new List<InventorySlot>();

    private void OnEnable()
    {
#if UNITY_EDITOR
        database = (ItemSavingDatabase)AssetDatabase.LoadAssetAtPath("Assets/Resources/DataBase.asset", typeof(ItemSavingDatabase));
#else
        database = Resources.Load<ItemSavingDatabase>("DataBase");
#endif
    }

    public void AddItem(ItemData _item, int _amount)
    {
        for (int i = 0; i < Container.Count; i++)
        {
            if(Container[i].item == _item)
            {
                Container[i].AddAmount(_amount);
                return;
            }
        }
        Container.Add(new InventorySlot(database.GetId[_item], _item, _amount));

    }

    public void SaveItem()
    {
        string SaveData = JsonUtility.ToJson(this, true);
        BinaryFormatter bf = new BinaryFormatter();
        FileStream SaveFile = File.Create(string.Concat(Application.persistentDataPath, savePath));
        bf.Serialize(SaveFile, SaveData);
        SaveFile.Close();
    }

    public void LoadItem()
    {
        if (File.Exists(string.Concat(Application.persistentDataPath, savePath)))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream loadFile = File.Open(string.Concat(Application.persistentDataPath, savePath), FileMode.Open);
            JsonUtility.FromJsonOverwrite(bf.Deserialize(loadFile).ToString(), this);
            loadFile.Close();
        }
    }

    public void OnAfterDeserialize()
    {
        for(int i = 0; i < Container.Count; i++)
            Container[i].item = database.GetItem[Container[i].ID];
    }

    public void OnBeforeSerialize()
    {
        
    }
}

[System.Serializable]
public class InventorySlot
{
    public int ID;
    public ItemData item;
    public int amount;
    public InventorySlot(int _id, ItemData _item, int _amount)
    {
        ID = _id;
        item = _item;
        amount = _amount;
    }

    public void AddAmount(int value)
    {
        amount += value;
    }
}