using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

namespace ChartLibrary
{
    public class SplitLineGraph : VisualElement
    {
        public new class UxmlTraits : VisualElement.UxmlTraits
        {
            UxmlIntAttributeDescription m_RangeAttribute = new() { name = "range" };

            UxmlStringAttributeDescription m_TestValues = new() { name = "test-values" };

            public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
            {
                base.Init(ve, bag, cc);

                SplitLineGraph graph = (SplitLineGraph)ve;

                string values = "";
                if (m_TestValues.TryGetValueFromBag(bag, cc, ref values))
                {
                    graph.testValues = values;
                }

                int range = 0;
                if (m_RangeAttribute.TryGetValueFromBag(bag, cc, ref range))
                {
                    graph.range = range;
                }
            }
        }
        
        public new class UxmlFactory : UxmlFactory<SplitLineGraph, UxmlTraits>{}
        
        public static readonly string ussClassName = "split-line-graph";

        static CustomStyleProperty<Color> s_GraphColor = new("--graph-color");

        SplitLineGraphMesh m_Mesh;
        
        public SplitLineGraph()
        {
            AddToClassList(ussClassName);

            m_Mesh = new SplitLineGraphMesh();
            
            m_Mesh.values = new int[] { };
            
            generateVisualContent += GenerateVisualContent;
            
            RegisterCallback<CustomStyleResolvedEvent>(evt => CustomStylesResolved(evt));

        }

        static void CustomStylesResolved(CustomStyleResolvedEvent evt)
        {
            SplitLineGraph element = (SplitLineGraph)evt.currentTarget;
            element.UpdateCustomStyles();
        }

        void UpdateCustomStyles()
        {
            if (customStyle.TryGetValue(s_GraphColor, out var graphColor))
            {
                m_Mesh.color = graphColor;
            }

            if (m_Mesh.isDirty)
                MarkDirtyRepaint();
        }

        void GenerateVisualContent(MeshGenerationContext context)
        {
            if (contentRect.width < 2.0f || contentRect.height < 2.0f)
                return;

            m_Mesh.origin = contentRect.position;
            m_Mesh.width = contentRect.width;
            m_Mesh.height = contentRect.height;
            m_Mesh.UpdateMesh();
            
            if (m_Mesh.vertices == null || m_Mesh.vertices.Length == 0 || m_Mesh.indices == null || m_Mesh.indices.Length == 0)
                return;
            
            var trackMeshWriteData = context.Allocate(m_Mesh.vertices.Length, m_Mesh.indices.Length, resolvedStyle.backgroundImage.texture);
            
            m_Mesh.RemapUVs(trackMeshWriteData.uvRegion);
            
            trackMeshWriteData.SetAllVertices(m_Mesh.vertices);
            trackMeshWriteData.SetAllIndices(m_Mesh.indices);
        }

        public int range
        {
            get => m_Mesh.range;
            set
            {
                m_Mesh.range = value;
                
                if (m_Mesh.isDirty)
                    MarkDirtyRepaint();
            }
        }
        
        public int[] values
        {
            get => m_Mesh.values;
            set
            {
                if (value == null)
                    throw new ArgumentNullException(nameof(value));
                m_Mesh.values = value;
                if (m_Mesh.isDirty)
                    MarkDirtyRepaint();
            }
        }

        // This property is only for previewing in the UI Builder
        public string testValues
        {
            get => values != null ? string.Join(", ", values.Select(v => v.ToString())) : "";
            set
            {
                var list = new List<int>();
                foreach (var val in value.Split(","))
                {
                    if (int.TryParse(val.Trim(), out int numValue))
                    {
                        list.Add(numValue);    
                    }
                }

                if (list.Count == 0)
                    return;
                
                values = list.ToArray();
            }
        }

    }
}
