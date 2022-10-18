using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
   Food,
   Helmet,
   Weapon,
   Tools,
   Boots,
   chest,
   Default
}

public enum Attributes
{
    // Basic States
    Agility,
    Intellect,
    Stamina,
    Strength,

    //Attack Status
    AttackDamge,
    ThrowingDmg,
    NumberOfThrows,
    AtkBonus,

    //Defend/ Health Status
    DenfenceBounus,
    HealthRegen,
    BonusHealth
}

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory System/Items/item")]
public class ItemData : ScriptableObject
{
    [Header("states")]
    public Sprite UIDisplay;
    public bool stackable;
    public ItemType type;
    [TextArea(15,20)]
    public string description;
    public Item itemData = new Item();

    public Item CreateItem()
    {
        Item newItem = new Item(this);
        return newItem;
    }
}

[System.Serializable]
public class Item
{
    public string Name;
    public int Id = -1;
    public ItemBuff[] buffs;

    public Item()
    {
        Name = "";
        Id = -1;
    }

    public Item(ItemData item)
    {
        Name = item.name;
        Id = item.itemData.Id;
        buffs = new ItemBuff[item.itemData.buffs.Length];

        for (int i = 0; i < buffs.Length; i++)
        {
            buffs[i] = new ItemBuff(item.itemData.buffs[i].MinBuffValue, item.itemData.buffs[i].MaxBuffValue)
            {
               attribute = item.itemData.buffs[i].attribute
            };
        }
    }
}

[System.Serializable]
public class ItemBuff : IModifiers
{
    public Attributes attribute;
    public int value;
    public int MaxBuffValue;
    public int MinBuffValue;

    public ItemBuff(int _MinBuffValue, int _MaxBuffBValue)
    {
        MinBuffValue = _MinBuffValue;
        MaxBuffValue = _MaxBuffBValue;
        GenerateValue();
    }

    public void Addvalue(ref int baseValue)
    {
        baseValue += value;
    }

    public void GenerateValue()
    {
        value = Random.Range(MinBuffValue, MaxBuffValue);
    }

}
