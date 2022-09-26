using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Equipement Object", menuName = "Inventory System/Items Type/ Equipment")]
public class Equipment : ItemData
{
    public float atkBonus;
    public float denfenceBounus;

    public void Awake()
    {
        type = ItemType.Equipment;
    }
}
