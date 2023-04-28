using System;
using Data;
using Data.Static;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(ItemIdAttribute))]
public class ItemIdDrawer : PropertyDrawer
{
    // Draw the property inside the given rect
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        int itemId = property.intValue;
        ItemInfo itemInfo = ItemInfoList.Instance.GetEntry(itemId);
        var newItemInfo = EditorGUI.ObjectField(position, label, itemInfo, typeof(ItemInfo), false) as ItemInfo;
        if (null != newItemInfo)
        {
            property.intValue = newItemInfo.value.id;
        }
    }
}