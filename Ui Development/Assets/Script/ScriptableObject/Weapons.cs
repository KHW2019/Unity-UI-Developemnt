using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Object", menuName = "Inventory System/Items Type/ Weapon")]
public class Weapons : ItemData
{
    //public int AttackDamge;
    //public int ThrowingDmg;
    //public int NumberOfThrows;
    public bool Throwable;
   
    public void Awake()
    {
        type = ItemType.Weapon;
    }
}
