using UnityEngine;
using UnityEngine.UIElements;

namespace ChartLibrary
{
    public class MeshBase
    {
        public bool isDirty { get; protected set; } = true;
        
        public Vertex[] vertices { get; protected set; }
        public ushort[] indices { get; protected set; }

        float m_Width;
        float m_Height;
        Vector2 m_Origin;
        Rect m_UVRegion;

        public float width
        {
            get => m_Width;
            set => CompareAndWrite(ref m_Width, value);
        }

        public float height
        {
            get => m_Height;
            set => CompareAndWrite(ref m_Height, value);
        }

        public Vector2 origin
        {
            get => m_Origin;
            set => CompareAndWrite(ref m_Origin, value);
        }

        public void RemapUVs(Rect uvRegion)
        {
            for(int i = 0; i < vertices.Length; ++i)
            {
                Vertex vert = vertices[i];
                vert.uv *= uvRegion.size;
                vert.uv += uvRegion.position;
                vertices[i] = vert;
            }
        }
        
        protected void CompareAndWrite(ref float field, float newValue)
        {
            if (Mathf.Abs(field - newValue) > float.Epsilon)
            {
                isDirty = true;
                field = newValue;
            }
        }
        
        protected void CompareAndWrite(ref Color field, Color newValue)
        {
            if (field != newValue)
            {
                isDirty = true;
                field = newValue;
            }
        }
        
        protected void CompareAndWrite(ref int field, int newValue)
        {
            if (field != newValue)
            {
                isDirty = true;
                field = newValue;
            }
        }
        
        protected void CompareAndWrite(ref Vector2 field, Vector2 newValue)
        {
            if (field != newValue)
            {
                isDirty = true;
                field = newValue;
            }
        }
    }
}
