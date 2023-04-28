using UnityEditor;
using UnityEngine;
using Utilities.Inspector;

namespace Utilities.Editor
{
    /// <summary>
    /// Makes a field read only in Editor. This function SHOULD BE in a script INSIDE Unity Editor folder.
    /// </summary>
    [CustomPropertyDrawer(typeof(ReadOnlyAttribute))]
    public class ReadOnlyDrawer : PropertyDrawer
    {
        // =============================================================================================================
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUI.GetPropertyHeight(property, label, true);
        }
        // =============================================================================================================
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            GUI.enabled = false;
            EditorGUI.PropertyField(position, property, label, true);
            GUI.enabled = true;
        }
        // =============================================================================================================
    }
}