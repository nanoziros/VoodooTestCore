using System.Collections.Generic;
using System.Linq;
using Gameplay.Data;
using Gameplay.Players;
using UnityEngine;
namespace Utils
{
    public class SkinPreviewGridBuilder
    {
        public RenderTexture m_AtlasRenderTexture;
        private Camera m_PreviewCamera;
        private Vector2Int m_TilePixelSize;
        private LayerMask m_PreviewLayerMask;
        private Vector2 m_WorldSpanPerTile;
        private Transform m_PreviewRootParent;
        private int m_AtlasColumns;
        private int m_AtlasRows;
        private int m_TotalVariants;

        const int c_RenderTextureDepth = 16;
        
        public SkinPreviewGridBuilder(Camera previewCamera,Vector2Int tilePixelSize, int totalVariants, LayerMask previewLayerMask)
        {
            m_PreviewCamera = previewCamera;
            m_TilePixelSize = tilePixelSize;
            m_PreviewLayerMask = previewLayerMask;
            m_TotalVariants = totalVariants;
        }
        
        public void Build()
        {
            // We try to roughly have a squared grid atlas
            m_AtlasColumns = Mathf.CeilToInt(Mathf.Sqrt(m_TotalVariants));
            m_AtlasRows = Mathf.CeilToInt((float)m_TotalVariants / m_AtlasColumns);
            
            // We determine the RT dimensions based on our individual tile pixel resolution
            int pixelWidth = m_AtlasColumns * m_TilePixelSize.x;
            int pixelHeight = m_AtlasRows * m_TilePixelSize.y ;
            m_AtlasRenderTexture = new RenderTexture(pixelWidth, pixelHeight, c_RenderTextureDepth, RenderTextureFormat.ARGB32)
            {
                name = $"BrushPreviewAtlas_{pixelWidth}x{pixelHeight}",
                useMipMap = false,
                autoGenerateMips = false,
                filterMode = FilterMode.Bilinear,
                anisoLevel = 1
            };
            m_AtlasRenderTexture.Create();
            
            m_PreviewCamera.cullingMask = m_PreviewLayerMask;
            m_PreviewCamera.targetTexture = m_AtlasRenderTexture;
            m_PreviewCamera.orthographic = true;
        }
        
        public Rect GetVariantUvRect(int variantFlatIndex)
        {
            int col = variantFlatIndex % m_AtlasColumns;
            int row = variantFlatIndex / m_AtlasColumns;
            float u = (float)col / m_AtlasColumns;
            float vFromTop = 1f - ((float)(row + 1) / m_AtlasRows);
            return new Rect(u, vFromTop, 1f / m_AtlasColumns, 1f / m_AtlasRows);
        }
    }
}
