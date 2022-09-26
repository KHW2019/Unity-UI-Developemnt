using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
   Food,
   Equipment,
   Default
}

public class ItemData : ScriptableObject
{
    public GameObject perfab;
    public ItemType type;
    [TextArea(15,20)]
    public string description;
}
