using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Food Object", menuName = "Inventory System/Items Type/ Food")]
public class FoodObject : ItemData
{
    //public int restoreHealthValue;

    public void Awake()
    {
        type = ItemType.Food;
    }
}
