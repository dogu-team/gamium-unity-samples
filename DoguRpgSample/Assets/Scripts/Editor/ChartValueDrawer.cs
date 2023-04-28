using System;
using Data;
using UnityEditor;
using UnityEngine;
using Util;


[CustomPropertyDrawer(typeof(ChartValue))]
public class ChartValueDrawer : PropertyDrawer
{
    private float basePropHeight = 0;
    private const float axisLabelWidth = 60;
    private const float selectButtonWidth = 120;
    private const float ChartHeight = 100;
    

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);
        basePropHeight = base.GetPropertyHeight(property, label);

        var rightArea = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);
        var leftArea = new Rect(position.x, position.y + basePropHeight, position.width - rightArea.width, ChartHeight - basePropHeight);
        var chartArea = new Rect(rightArea.x, rightArea.y + basePropHeight, rightArea.width, ChartHeight);
        FillChart(property);

        DrawInterpolationSelectButton(leftArea, property);
        chartArea = DrawXMinMax(chartArea, property);
        chartArea = DrawYMinMax(chartArea, property);
        DrawChart(chartArea, property);
        DrawBars(chartArea, property);


        EditorGUI.EndProperty();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        float height = base.GetPropertyHeight(property, label);
        height += ChartHeight;
        return height;
    }
    
    private Rect DrawInterpolationSelectButton(Rect position, SerializedProperty property)
    {
        var maxRect = new Rect(position.x, position.y, selectButtonWidth, basePropHeight);
        var interpolationType = property.FindPropertyRelative("interpolationType");

        EditorGUI.PropertyField(maxRect, interpolationType, GUIContent.none);

        return new Rect(position.x, position.y + basePropHeight, position.width, position.height);
    }

    private Rect DrawXMinMax(Rect position, SerializedProperty property)
    {
        var yPos = position.y + position.height - basePropHeight;
        // xMin not supported
        // var minRect = new Rect(position.x + axisLabelWidth, yPos, axisLabelWidth, basePropHeight);
        // var min = property.FindPropertyRelative("xmin");
        // EditorGUI.PropertyField(minRect, min, GUIContent.none);
        var maxRect = new Rect(position.x + position.width - axisLabelWidth, yPos, axisLabelWidth, basePropHeight);
        var max = property.FindPropertyRelative("xmax");

        EditorGUI.PropertyField(maxRect, max, GUIContent.none);

        return new Rect(position.x, position.y, position.width, position.height - basePropHeight);
    }

    private Rect DrawYMinMax(Rect position, SerializedProperty property)
    {
        var minRect = new Rect(position.x, position.y + position.height - basePropHeight, axisLabelWidth,
            basePropHeight);
        var maxRect = new Rect(position.x, position.y, axisLabelWidth, basePropHeight);
        var min = property.FindPropertyRelative("ymin");
        var max = property.FindPropertyRelative("ymax");

        EditorGUI.PropertyField(minRect, min, GUIContent.none);
        EditorGUI.PropertyField(maxRect, max, GUIContent.none);

        return new Rect(position.x + axisLabelWidth, position.y, position.width - axisLabelWidth, position.height);
    }

    private void DrawChart(Rect rect, SerializedProperty property)
    {
        Handles.color = Color.white;
        Handles.DrawSolidRectangleWithOutline(rect, new Color(0, 0, 0, 0.1f), Color.black);
    }

    private void DrawBars(Rect position, SerializedProperty property)
    {
        var yAdditive = 10;
        var chartHeight = position.height - yAdditive;
        var points = property.FindPropertyRelative("points");
        var ymin = property.FindPropertyRelative("ymin").longValue;
        var ymax = property.FindPropertyRelative("ymax").longValue;
        var ydelta = ymax - ymin;
        if (ydelta < 0)
        {
            return;
        }

        var space = 1;
        var barWidth = (position.width - space * (points.arraySize - 1)) / points.arraySize;
        for (int i = 0; i < points.arraySize; i++)
        {
            var point = points.GetArrayElementAtIndex(i);
            var pointValueFromMin = point.longValue - ymin;
            var barHeight = (float)pointValueFromMin / (float) (ydelta + 1) * chartHeight + yAdditive;
            var pointRect = new Rect(position.x + i * (barWidth + space), position.y + position.height - barHeight,
                barWidth, barHeight);
            DrawBar(pointRect, point.longValue);
        }
    }

    private void DrawBar(Rect rect, long value)
    {
        Handles.color = Color.green;
        Handles.DrawSolidRectangleWithOutline(rect, new Color(0, 0.5f, 0, 0.1f), Color.black);
        var content = new GUIContent(value.ToString());
        content.tooltip = $" {value} ";
        var style = new GUIStyle(GUI.skin.label);
        style.alignment = TextAnchor.MiddleCenter;
        style.wordWrap = true;
        EditorGUI.LabelField(rect, content, style);
    }

    private void FillChart(SerializedProperty property)
    {
        var interpolationType = (ChartValue.ChartInterpolationType)property.FindPropertyRelative("interpolationType").intValue;
        var xmin = property.FindPropertyRelative("xmin").longValue;
        var xmax = property.FindPropertyRelative("xmax").longValue;
        var ymin = property.FindPropertyRelative("ymin").longValue;
        var ymax = property.FindPropertyRelative("ymax").longValue;
        var points = property.FindPropertyRelative("points");
        points.arraySize = (int)ChartValue.CalcuatePointsSize(xmin, xmax);
        for (int i = 0; i < points.arraySize; i++)
        {
            var point = points.GetArrayElementAtIndex(i);
            point.longValue = ChartValue.CaculateYValue(interpolationType, xmin, xmax, ymin, ymax, i);
        }
    }
}