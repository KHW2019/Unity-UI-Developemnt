using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class CollectItem : MonoBehaviour, ISerializationCallbackReceiver
{
    public ItemData item;

    public void OnAfterDeserialize()
    {
        
    }

    public void OnBeforeSerialize()
    {
        //GetComponentInChildren<SpriteRenderer>().sprite = item.UIDisplay;
        //EditorUtility.SetDirty(GetComponentInChildren<SpriteRenderer>());
    }
}
