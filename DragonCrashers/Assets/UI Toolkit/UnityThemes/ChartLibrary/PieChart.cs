using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

namespace ChartLibrary
{
    public class PieChart : VisualElement
    {
        public new class UxmlTraits : VisualElement.UxmlTraits
        {
            UxmlStringAttributeDescription m_TestValues = new() { name = "test-values" };
            
            UxmlColorAttributeDescription m_TestColor = new() { name = "test-color" };

            public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
            {
                base.Init(ve, bag, cc);

                PieChart chart = (PieChart)ve;

                string values = "";
                if (m_TestValues.TryGetValueFromBag(bag, cc, ref values))
                {
                    chart.testValues = values;
                }
            }
        }
        
        public new class UxmlFactory : UxmlFactory<PieChart, UxmlTraits>{}
        
        public static readonly string ussClassName = "pie-chart";
        
        // Number of outer vertices to generate the circle of the chart
        const int k_NumSteps = 200;
        
        PieChartMesh m_Mesh;
        

        public PieChart()
        {
            AddToClassList(ussClassName);

            m_Mesh = new PieChartMesh(k_NumSteps, s_SliceColors.Length);
            m_Mesh.values = new int[] { };
            
            generateVisualContent += GenerateVisualContent;
            
            RegisterCallback<CustomStyleResolvedEvent>(StylesResolved);
        }

        // Support up to eight colors
        static CustomStyleProperty<Color>[] s_SliceColors = {
            new("--color-slice-1"),
            new("--color-slice-2"),
            new("--color-slice-3"),
            new("--color-slice-4"),
            new("--color-slice-5"),
            new("--color-slice-6"),
            new("--color-slice-7"),
            new("--color-slice-8"),
        };
        
        void StylesResolved(CustomStyleResolvedEvent evt)
        {
            for (int i = 0; i < s_SliceColors.Length; ++i)
            {
                if (customStyle.TryGetValue(s_SliceColors[i], out Color color))
                {
                    m_Mesh.SetColor(i, color);
                }
                else
                {
                    m_Mesh.SetColor(i, Color.cyan);
                }
            }
            
            if (m_Mesh.isDirty)
                MarkDirtyRepaint();
        }

        void GenerateVisualContent(MeshGenerationContext context)
        {
            float halfWidth = contentRect.width * 0.5f;
            float halfHeight = contentRect.height * 0.5f;

            if (halfWidth < 2.0f || halfHeight < 2.0f)
                return;

            m_Mesh.width = halfWidth;
            m_Mesh.height = halfHeight;
            m_Mesh.UpdateMesh();
            
            if (m_Mesh.vertices == null || m_Mesh.vertices.Length == 0 || m_Mesh.indices == null || m_Mesh.indices.Length == 0)
                return;
            
            var trackMeshWriteData = context.Allocate(m_Mesh.vertices.Length, m_Mesh.indices.Length);
            trackMeshWriteData.SetAllVertices(m_Mesh.vertices);
            trackMeshWriteData.SetAllIndices(m_Mesh.indices);
        }

        public IReadOnlyList<Color> colors => m_Mesh.colors;

        public int[] values
        {
            get => m_Mesh.values;
            set
            {
                if (value == null)
                    throw new ArgumentNullException(nameof(value));
                m_Mesh.values = value;
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
