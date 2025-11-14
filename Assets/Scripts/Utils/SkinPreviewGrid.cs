using System.Collections.Generic;
using UnityEngine;

namespace Utils
{
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

        private const float CameraDistance = -2f;

        public void Setup(int totalModels, int cols, int rows, List<GameObject> previewPrefabs, 
            out RenderTexture atlasRenderTexture, out RenderTexture atlasMaskRenderTexture)
        {
            float worldWidth = cols * m_PreviewCellWorldSize;
            float worldHeight = rows * m_PreviewCellWorldSize;
            float orthoSize = worldHeight * 0.5f;
            Vector3 cameraPosition = new Vector3(worldWidth * 0.5f, worldHeight * 0.5f, CameraDistance);

            int texWidth = cols * m_PreviewPixelResolution;
            int texHeight = rows * m_PreviewPixelResolution;
            float aspectRatio = (float)texWidth / texHeight;

            atlasRenderTexture = CreateRenderTexture(texWidth, texHeight);
            atlasMaskRenderTexture = CreateRenderTexture(texWidth, texHeight);

            ConfigureCamera(m_PreviewCamera, m_PreviewLayerMask, atlasRenderTexture, aspectRatio, orthoSize, cameraPosition);
            ConfigureCamera(m_SkinMaskCamera, m_PreviewSkinMaskLayerMask, atlasMaskRenderTexture, aspectRatio, orthoSize, cameraPosition);

            m_SkinMaskCamera.SetReplacementShader(m_MaskShader, "");

            PopulateBrushGrid(totalModels, cols, previewPrefabs);
        }

        private RenderTexture CreateRenderTexture(int width, int height)
        {
            var renderTexture = new RenderTexture(width, height, m_SkinPreviewRTDepth, RenderTextureFormat.ARGB32)
            {
                filterMode = FilterMode.Point,
                wrapMode = TextureWrapMode.Clamp
            };
            renderTexture.Create();
            return renderTexture;
        }

        private void ConfigureCamera(Camera targetCamera, LayerMask cullingMask, RenderTexture targetTexture, float aspect, float orthoSize, Vector3 position)
        {
            targetCamera.orthographic = true;
            targetCamera.transform.position = position;
            targetCamera.cullingMask = cullingMask;
            targetCamera.targetTexture = targetTexture;
            targetCamera.aspect = aspect;
            targetCamera.orthographicSize = orthoSize;
        }

        private void PopulateBrushGrid(int totalModels, int cols, List<GameObject> previewPrefabs)
        {
            for (int i = 0; i < totalModels; i++)
            {
                int col = i % cols;
                int row = i / cols;
                float x = (col + 0.5f) * m_PreviewCellWorldSize;
                float y = (row + 0.5f) * m_PreviewCellWorldSize;

                Vector3 spawnPosition = new Vector3(x, y, 0f) + m_BrushWorldOffset;
                GameObject prefab = previewPrefabs[i];
                GameObject brushInstance = Instantiate(prefab, spawnPosition, Quaternion.identity, m_BrushParent);
                brushInstance.transform.localScale = Vector3.one * m_BrushWorldScale;
            }
        }
    }
}
