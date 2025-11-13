using System.Collections.Generic;
using UnityEngine;
namespace Utils
{
    
    [System.Serializable]
    public struct CameraWorldBounds
    {
        public Vector3 bottomLeft;
        public Vector3 bottomRight;
        public Vector3 topLeft;
        public Vector3 topRight;

        public CameraWorldBounds(Vector3 bl, Vector3 br, Vector3 tl, Vector3 tr)
        {
            bottomLeft = bl;
            bottomRight = br;
            topLeft = tl;
            topRight = tr;
        }
    }
    
    public class SkinPreviewGrid : MonoBehaviour
    {
        public Shader m_MaskShader;
        
        public LayerMask m_PreviewLayerMask;
        public LayerMask m_PreviewSkinMaskLayerMask;
        
        public Camera m_PreviewCamera;
        public Camera m_SkinMaskCamera;
        public Transform m_BrushParent;

        public int m_PreviewCellWorldSize = 10;
        public int m_PreviewPixelResolution = 128;
        public int m_SkinPreviewRTDepth = 24;
        public Vector3 m_BrushWorldOffset = Vector3.zero;
        public float m_BrushWorldScale = 1f;
        
        public void Setup(
            int totalModels,
            int cols, 
            int rows,
            List<GameObject> previewPrefabs, 
            out RenderTexture atlasRenderTexture, 
            out RenderTexture atlasMaskRenderTexture)
        {
            float worldWidth  = cols * m_PreviewCellWorldSize;
            float worldHeight = rows * m_PreviewCellWorldSize;
            
            // Setup Preview Cameras
            float camDistance = -2f;
            float orthoSize = worldHeight * 0.5f;
            Vector3 cameraPosition = new Vector3(worldWidth * 0.5f, worldHeight * 0.5f, camDistance);
            m_PreviewCamera.orthographic = true;
            m_PreviewCamera.transform.position = cameraPosition;
            
            m_SkinMaskCamera.orthographic = true;
            m_SkinMaskCamera.transform.position = cameraPosition;
            
            // Setup preview render textures
            int texWidth  = cols * m_PreviewPixelResolution;
            int texHeight = rows * m_PreviewPixelResolution;
            
            atlasRenderTexture = new RenderTexture(texWidth, texHeight, m_SkinPreviewRTDepth, RenderTextureFormat.ARGB32)
            {
                filterMode = FilterMode.Point,
                wrapMode = TextureWrapMode.Clamp
            };
            atlasRenderTexture.Create();
            
            atlasMaskRenderTexture = new RenderTexture(texWidth, texHeight, m_SkinPreviewRTDepth, RenderTextureFormat.ARGB32)
            {
                filterMode = FilterMode.Point,
                wrapMode = TextureWrapMode.Clamp
            };
            atlasMaskRenderTexture.Create();
            
            // Assign render textures and culling masks
            float aspectRatio = (float)texWidth / texHeight;

            m_PreviewCamera.cullingMask = m_PreviewLayerMask;
            m_PreviewCamera.targetTexture = atlasRenderTexture;
            m_PreviewCamera.orthographic = true;
            m_PreviewCamera.aspect = aspectRatio;
            m_PreviewCamera.orthographicSize = orthoSize;

            m_SkinMaskCamera.cullingMask = m_PreviewSkinMaskLayerMask;
            m_SkinMaskCamera.targetTexture = atlasMaskRenderTexture;
            m_SkinMaskCamera.orthographic = true;
            m_SkinMaskCamera.aspect = aspectRatio;
            m_SkinMaskCamera.orthographicSize = orthoSize;
            m_SkinMaskCamera.SetReplacementShader(m_MaskShader, "");
            
            // Populate the world grid with the prefab instances
            for (int i = 0; i < totalModels; i++)
            {
                int col = i % cols;
                int row = i / cols;

                float x = (col + 0.5f) * m_PreviewCellWorldSize;
                float y = (row + 0.5f) * m_PreviewCellWorldSize;

                Vector3 spawnPosition = new Vector3(x, y, 0f) + m_BrushWorldOffset;
                GameObject brushInstance = Instantiate(previewPrefabs[i], spawnPosition, Quaternion.identity, m_BrushParent);
                brushInstance.transform.localScale = Vector3.one * m_BrushWorldScale;
            }
        }
    }
}
