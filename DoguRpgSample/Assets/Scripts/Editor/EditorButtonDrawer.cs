using Data;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

[CustomPropertyDrawer(typeof(EditorButtonAttribute))]
public class EditorButtonDrawer : PropertyDrawer
{
    // Draw the property inside the given rect
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        // EditorGUI.LabelField(position, label);
        var attr = attribute as EditorButtonAttribute;

        if (GUI.Button(position, "Generate"))
        {
            attr.Invoke(property.serializedObject.targetObject);
        }
    }
}