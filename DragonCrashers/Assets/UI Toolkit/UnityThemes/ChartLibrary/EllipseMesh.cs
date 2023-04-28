using UnityEngine;
using UnityEngine.UIElements;

namespace ChartLibrary
{
    public class EllipseMesh : MeshBase
    {
        readonly int m_NumSteps;
        Color m_Color;
        float m_BorderSize;
        
        public EllipseMesh(int numSteps)
        {
            m_NumSteps = numSteps;
        }

        public void UpdateMesh()
        {
            if (!isDirty)
                return;
            
            int numVertices = m_NumSteps * 2;
            int numIndices = numVertices * 6;
            
            if (vertices == null || vertices.Length != numVertices)
                vertices = new Vertex[numVertices];
            
            if (indices == null || indices.Length != numIndices)
                indices = new ushort[numIndices];

            float stepSize = 360.0f / (float)m_NumSteps;
            float angle = -180.0f;
            
            for (int i = 0; i < m_NumSteps; ++i)
            {
                angle -= stepSize;
                float radians = Mathf.Deg2Rad * angle;

                float outerX = Mathf.Sin(radians) * width;
                float outerY = Mathf.Cos(radians) * height;
                Vertex outerVertex = new Vertex();
                outerVertex.position = new Vector3(width + outerX, height + outerY, Vertex.nearZ);
                outerVertex.tint = color;
                vertices[i * 2] = outerVertex;
                
                float innerX = Mathf.Sin(radians) * (width - borderSize);
                float innerY = Mathf.Cos(radians) * (height - borderSize);
                Vertex innerVertex = new Vertex();
                innerVertex.position = new Vector3(width + innerX, height + innerY, Vertex.nearZ);
                innerVertex.tint = color;
                vertices[i * 2 + 1] = innerVertex;
                
                indices[i * 6] = (ushort)((i == 0) ? vertices.Length-2 : (i-1) * 2); // previous outer vertex
                indices[i * 6 + 1] = (ushort)(i * 2); // current outer vertex
                indices[i * 6 + 2] = (ushort)(i * 2 + 1); // current inner vertex
                
                indices[i * 6 + 3] = (ushort)((i == 0) ? vertices.Length-2 : (i-1) * 2); // previous outer vertex
                indices[i * 6 + 4] = (ushort)(i * 2 + 1); // current inner vertex
                indices[i * 6 + 5] = (ushort)((i == 0) ? vertices.Length-1 : (i-1) * 2 + 1); // previous inner vertex
            }

            isDirty = false;
        }

        public Color color
        {
            get => m_Color;
            set => CompareAndWrite(ref m_Color, value);
        }

        public float borderSize
        {
            get => m_BorderSize;
            set => CompareAndWrite(ref m_BorderSize, value);
        }

    }
}
