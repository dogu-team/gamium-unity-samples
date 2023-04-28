using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UIElements;

namespace ChartLibrary
{
    public class PieChartMesh : MeshBase
    {
        readonly int m_NumSteps;
        Color[] m_Colors;
        int[] m_Values;
        int m_Total;
        public PieChartMesh(int numSteps, int colorPaletteSize)
        {
            m_NumSteps = numSteps;
            m_Colors = new Color[colorPaletteSize];
        }

        public void UpdateMesh()
        {
            if (!isDirty)
                return;

            var vertList = ListPool<Vertex>.Get();
            var indexList = ListPool<ushort>.Get();

            try
            {
                GenerateMesh(vertList, indexList);
                vertices = vertList.ToArray();
                indices = indexList.ToArray();
                isDirty = false;
            }
            finally
            {
                ListPool<Vertex>.Release(vertList);
                ListPool<ushort>.Release(indexList);
            }
            
        }

        void GenerateMesh(List<Vertex> vertList, List<ushort> indexList)
        {
            float stepSize = 360.0f / (float)m_NumSteps;
            float currentAngle = -180.0f;
            int remainingSteps = m_NumSteps;

            int total = 0;
            foreach (var value in m_Values)
            {
                total += value;
            }
            
            // Each value represents a slice of the pie
            for (int j = 0; j < m_Values.Length; j++)
            {
                // Pick slice color
                Color sliceColor = m_Colors[j % m_Colors.Length];
                
                // Calculate how many of the outer vertices of the circle this slice uses
                double ratio = m_Values[j] * 100.0 / (double)total;
                int steps;
                
                // Due to rounding errors, we need this to ensure the last slice aligns perfectly
                if (j == m_Values.Length - 1)
                {
                    steps = remainingSteps;
                }
                else
                {
                    steps = (int)Math.Floor((ratio * (double)m_NumSteps) / 100.0) ;
                    remainingSteps -= steps;
                }
                
                // Each slice needs to its center vertex because it's a different color
                var centerVertex = new Vertex();
                centerVertex.position = new Vector3(width, height, Vertex.nearZ);
                centerVertex.tint = sliceColor;
                vertList.Add(centerVertex);
                
                // Note we draw steps + 1 since we can't reuse vertices between slices (since color is different)
                for (int i = 0; i <= steps; ++i)
                {
                    float radians = Mathf.Deg2Rad * currentAngle;

                    float x = Mathf.Sin(radians) * width;
                    float y = Mathf.Cos(radians) * height;
                
                    Vertex vertex = new Vertex();
                    vertex.position = new Vector3(width + x, height + y, Vertex.nearZ);
                    vertex.tint = sliceColor;
                    vertList.Add(vertex);
                    
                    // The last step does not require a triangle (we just needed to generate an extra vertex)
                    if (i < steps)
                    {
                        indexList.Add((ushort)(vertList.Count - 1)); // This vert
                        indexList.Add((ushort)(vertList.Count)); // Next vert
                        indexList.Add((ushort)(vertList.Count - 2 - i)); // Center vert
                        
                        currentAngle -= stepSize;
                    }
                }
            }
        }
        
        public void SetColor(int i, Color c)
        {
            if (m_Colors[i] != c)
            {
                isDirty = true;
                m_Colors[i] = c;
            }
        }

        public int[] values
        {
            get => m_Values;
            set
            {
                isDirty = true;
                m_Values = value;
            }
        }

        public IReadOnlyList<Color> colors => m_Colors;
    }
}
