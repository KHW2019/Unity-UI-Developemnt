using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void ModifiedEvent();

[Serializable]
public class ModifiableInt
{
    [SerializeField]
    private int baseValue;
    public int BaseValue { get { return baseValue; } set { baseValue = value; UpdateModifiedValue();  } }

    [SerializeField]
    private int modifiedValue;
    public int ModifiedValue { get { return modifiedValue; } private set { modifiedValue = value; } }

    //list of Modifier
    public List<IModifiers> modifiers = new List<IModifiers>();

    //delagate setup
    public event ModifiedEvent ValueModified; 

    //constructor for creating the modified value 
    //Set modified event to null so the event will only pass when it mets the requirment
    public ModifiableInt(ModifiedEvent method = null)
    {
        modifiedValue = BaseValue;
        if (method != null)
            ValueModified += method;
    }

    //function to registering the mod event
    public void RegsisterModEvent(ModifiedEvent method)
    {
        ValueModified += method;
    }

    //function to unregistering the mod event
    public void UnregsisterModEvent(ModifiedEvent method)
    {
        ValueModified -= method;
    }

    //function to update the modified value
    public void UpdateModifiedValue()
    {
        var valueToAdd = 0;
        for (int i  = 0; i < modifiers.Count; i++)
        {
            modifiers[i].Addvalue(ref valueToAdd);
        }

        ModifiedValue = baseValue + valueToAdd;
        if (ValueModified != null)
        {
            ValueModified.Invoke();
        }
    }

    //Method to add modifier
    public void AddModifer(IModifiers _modifiers)
    {
        modifiers.Add(_modifiers);
        UpdateModifiedValue();
    }

    //Method to Remove modifier
    public void ReMoveModifer(IModifiers _modifiers)
    {
        modifiers.Remove(_modifiers);
        UpdateModifiedValue();
    }
}
