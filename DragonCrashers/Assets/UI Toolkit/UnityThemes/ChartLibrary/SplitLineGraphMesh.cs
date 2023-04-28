using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UIElements;

namespace ChartLibrary
{
    public class SplitLineGraphMesh : MeshBase
    {
        Color m_TopColor;
        Color m_Color;
        int m_Range;
        int[] m_Values;
        float m_LineSize;
        
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
            float stepSize = width / (m_Values.Length - 1);

            for (int i = 1; i < m_Values.Length; ++i)
            {
                int previousValue = m_Values[i - 1];
                int currentValue = m_Values[i];

                int lowest = Mathf.Min(previousValue, currentValue);

                float minX = (i - 1) * stepSize;
                float maxX = minX + stepSize;
                float splitY = ((range - lowest) / (float)range) * height;

                if (lowest > 0)
                {
                    Vertex vert = new Vertex();
                    vert.tint = m_Color;
                    vert.position = new Vector3(minX, splitY, Vertex.nearZ);
                    vertList.Add(vert);
                    vert.position = new Vector3(maxX, splitY, Vertex.nearZ);
                    vertList.Add(vert);
                    vert.position = new Vector3(minX, height, Vertex.nearZ);
                    vertList.Add(vert);
                    vert.position = new Vector3(maxX, height, Vertex.nearZ);
                    vertList.Add(vert);
                    
                    indexList.Add((ushort)(vertList.Count - 4));
                    indexList.Add((ushort)(vertList.Count - 3));
                    indexList.Add((ushort)(vertList.Count - 2));
                    
                    indexList.Add((ushort)(vertList.Count - 2));
                    indexList.Add((ushort)(vertList.Count - 3));
                    indexList.Add((ushort)(vertList.Count - 1));
                }

                {
                    Vertex vert = new Vertex();
                    vert.tint = m_Color;
                    vert.position = new Vector3(minX, ((range - previousValue) / (float)range) * height);
                    vertList.Add(vert);

                    vert.position = new Vector3(maxX, ((range - currentValue) / (float)range) * height);
                    vertList.Add(vert);
                    
                    vert.position = new Vector3(previousValue > currentValue ? minX : maxX, splitY);
                    vertList.Add(vert);
                    
                    indexList.Add((ushort)(vertList.Count - 3));
                    indexList.Add((ushort)(vertList.Count - 2));
                    indexList.Add((ushort)(vertList.Count - 1));
                }
                

            }
            
            // Write UVs and adjust origin all in one go
            for(int i = 0; i < vertList.Count; ++i)
            {
                Vertex vert = vertList[i];
                vert.uv = new Vector2(0, 1.0f -(vert.position.y / height));
                vert.position += (Vector3)origin;
                vertList[i] = vert;
            }
        }

        public Color color
        {
            get => m_Color;
            set => CompareAndWrite(ref m_Color, value);
        }

        public int range
        {
            get => m_Range;
            set => CompareAndWrite(ref m_Range, value);
        }

        public int[] values
        {
            get => m_Values;
            set
            {
                m_Values = value;
                isDirty = true;
            }
        }

    }
}
