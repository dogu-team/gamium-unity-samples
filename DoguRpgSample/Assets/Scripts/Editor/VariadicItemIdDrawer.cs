using System;
using Data;
using Data.Static;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(VariadicItemIdAttribute))]
public class VariadicItemIdDrawer : PropertyDrawer
{
    // Draw the property inside the given rect
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        int itemId = property.intValue;
        VariadicItemInfo itemInfo = VariadicItemInfoList.Instance.GetEntry(itemId);
        var newItemInfo = EditorGUI.ObjectField(position, label, itemInfo, typeof(VariadicItemInfo), false) as VariadicItemInfo;
        if (null != newItemInfo)
        {
            property.intValue = newItemInfo.value.id;
        }
    }
}