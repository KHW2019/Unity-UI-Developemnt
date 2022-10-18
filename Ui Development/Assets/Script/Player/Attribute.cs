using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Attribute
{
    [NonSerialized]
    public PlayerInventory parent;

    public Attributes type;
    public ModifiableInt value;

    public void SetParent(PlayerInventory _playerInventory)
    {
        parent = _playerInventory;
        value = new ModifiableInt(AttributeModified);
    }

    public void AttributeModified()
    {
        parent.AttributeModified(this);
    }

}
