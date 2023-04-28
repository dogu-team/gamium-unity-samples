using Unity.Collections;
using UnityEngine;
using UnityEngine.UIElements;

namespace ChartLibrary
{
    public class RadialCounter : VisualElement
    {
        public new class UxmlTraits : VisualElement.UxmlTraits
        {
            UxmlFloatAttributeDescription m_ProgressAttribute = new UxmlFloatAttributeDescription()
            {
                name = "progress"
            };

            public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
            {
                base.Init(ve, bag, cc);

                float progress = 0.0f;
                if (m_ProgressAttribute.TryGetValueFromBag(bag, cc, ref progress))
                {
                    ((RadialCounter)ve).progress = progress;
                }
            }
        }
        
        public new class UxmlFactory : UxmlFactory<RadialCounter, UxmlTraits>{}

        public static readonly string ussClassName = "radial-counter";

        static CustomStyleProperty<Color> s_TrackColor = new("--track-color");
        static CustomStyleProperty<Color> s_ProgressColor = new("--progress-color");
        
        EllipseMesh m_TrackMesh;
        EllipseMesh m_ProgressMesh;
        
        // Number of outer vertices to generate the circle
        const int k_NumSteps = 200;

        float m_Progress;
        
        /// <summary>
        /// A value between 0 and 100
        /// </summary>
        public float progress
        {
            get => m_Progress;
            set
            {
                m_Progress = value;
                MarkDirtyRepaint();
            }
        }

        public RadialCounter()
        {
            m_ProgressMesh = new EllipseMesh(k_NumSteps);
            m_TrackMesh = new EllipseMesh(k_NumSteps);

            AddToClassList(ussClassName);
            RegisterCallback<CustomStyleResolvedEvent>(evt => CustomStylesResolved(evt));

            generateVisualContent += context => GenerateVisualContent(context);

            progress = 0.0f;
        }
        
        static void CustomStylesResolved(CustomStyleResolvedEvent evt)
        {
            RadialCounter element = (RadialCounter)evt.currentTarget;
            element.UpdateCustomStyles();
        }

        void UpdateCustomStyles()
        {
            if (customStyle.TryGetValue(s_ProgressColor, out var progressColor))
            {
                m_ProgressMesh.color = progressColor;
            }

            if (customStyle.TryGetValue(s_TrackColor, out var trackColor))
            {
                m_TrackMesh.color = trackColor;
            }
            
            if (m_ProgressMesh.isDirty || m_TrackMesh.isDirty)
                MarkDirtyRepaint();
        }

        static void GenerateVisualContent(MeshGenerationContext context)
        {
            RadialCounter element = (RadialCounter)context.visualElement;
            element.DrawMeshes(context);
        }
        
        void DrawMeshes(MeshGenerationContext context)
        {
            float halfWidth = contentRect.width * 0.5f;
            float halfHeight = contentRect.height * 0.5f;

            if (halfWidth < 2.0f || halfHeight < 2.0f)
                return;

            m_ProgressMesh.width = halfWidth;
            m_ProgressMesh.height = halfHeight;
            m_ProgressMesh.borderSize = 10;
            m_ProgressMesh.UpdateMesh();
            
            m_TrackMesh.width = halfWidth;
            m_TrackMesh.height = halfHeight;
            m_TrackMesh.borderSize = 10;
            m_TrackMesh.UpdateMesh();
            
            // Draw track mesh first
            var trackMeshWriteData = context.Allocate(m_TrackMesh.vertices.Length, m_TrackMesh.indices.Length);
            trackMeshWriteData.SetAllVertices(m_TrackMesh.vertices);
            trackMeshWriteData.SetAllIndices(m_TrackMesh.indices);

            // Keep progress between 0 and 100
            float clampedProgress = Mathf.Clamp(m_Progress, 0.0f, 100.0f);
            
            // Determine how many triangle are used to depending on progress, to achieve a partially filled circle
            int sliceSize = Mathf.FloorToInt((k_NumSteps * clampedProgress) / 100.0f);
            
            if (sliceSize == 0)
                return;
            
            // Every step is 6 indices in the corresponding array
            sliceSize *= 6;
            
            var progressMeshWriteData = context.Allocate(m_ProgressMesh.vertices.Length, sliceSize);
            progressMeshWriteData.SetAllVertices(m_ProgressMesh.vertices);
            
            var tempIndicesArray = new NativeArray<ushort>(m_ProgressMesh.indices, Allocator.Temp);
            progressMeshWriteData.SetAllIndices(tempIndicesArray.Slice(0, sliceSize));
            tempIndicesArray.Dispose();
        }
        
    }
}
