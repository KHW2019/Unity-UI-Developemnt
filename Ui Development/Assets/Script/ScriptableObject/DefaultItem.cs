using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Object", menuName = "Inventory System/Items Type/ Default")]
public class DefaultItem : ItemData
{
    public void Awake()
    {
        type = ItemType.Default;
    }
}
