using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
   Food,
   Equipment,
   Weapon,
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

public class ItemData : ScriptableObject
{
    //public GameObject perfab;
    [Header("states")]
    public int Id;
    public Sprite UIDisplay;
    public ItemType type;
    [TextArea(15,20)]
    public string description;
    public ItemBuff[] buffs;

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
    public int Id;
    public ItemBuff[] buffs;

    public Item(ItemData item)
    {
        Name = item.name;
        Id = item.Id;
        buffs = new ItemBuff[item.buffs.Length];

        for (int i = 0; i < buffs.Length; i++)
        {
            buffs[i] = new ItemBuff(item.buffs[i].MinBuffValue, item.buffs[i].MaxBuffValue)
            {
               attribute = item.buffs[i].attribute
            };
        }
    }
}

[System.Serializable]
public class ItemBuff
{
    public Attributes attribute;
    public int value;
    public int MaxBuffValue;
    public int MinBuffValue;

    public ItemBuff(int _MinBuffValue, int _MaxBuffBValue)
    {
        MinBuffValue = _MinBuffValue;
        MaxBuffValue = _MaxBuffBValue;

    }

    public void GenerateValue()
    {
        value = UnityEngine.Random.Range(MinBuffValue, MaxBuffValue);
    }

}
