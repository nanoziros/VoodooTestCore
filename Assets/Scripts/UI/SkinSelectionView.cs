using System.Collections.Generic;
using Gameplay.Data;
using Services;
using UnityEngine;
using Utils;
namespace UI
{
    public class SkinSelectionView :  View<SkinSelectionView>
    {
        public SkinPreviewGrid m_SkinPreviewGrid;
        public Transform m_SkinOptionsParent;
        public SkinOptionButton m_SkinOptionButtonPrefab;
        
        protected override void Awake()
        {
            base.Awake();
            PopulateSkins();
        }

        private void PopulateSkins()
        {
            List<SkinData> skinsData = GameService.m_Skins;
            int totalVariants = skinsData.Count;
            
            // Setup preview skin models
            List<GameObject> uniqueModels = new List<GameObject>();
            foreach (var skinData in skinsData)
            {
                if (!uniqueModels.Contains(skinData.Brush.m_PreviewPrefab))
                {
                    uniqueModels.Add(skinData.Brush.m_PreviewPrefab);
                }
            }
            
            int totalModels = uniqueModels.Count;
            int cols = Mathf.CeilToInt(Mathf.Sqrt(totalModels));
            int rows = Mathf.CeilToInt((float)totalModels / cols);
            
            m_SkinPreviewGrid.Setup(
                totalModels, 
                cols, 
                rows,
                uniqueModels,
                out RenderTexture atlasRenderTexture,
                out RenderTexture atlasMaskRenderTexture);
            
            Material sharedUiMat = new Material(m_SkinOptionButtonPrefab.m_RawImageContent.material);
            sharedUiMat.name = "UI_RecolorMat_Runtime";
            sharedUiMat.SetTexture("_MainTex", atlasRenderTexture);
            sharedUiMat.SetTexture("_MaskTex", atlasMaskRenderTexture);
            
            Dictionary<GameObject, Rect> uniqueModelsUvRect = new Dictionary<GameObject, Rect>();
            for (int index = 0; index < uniqueModels.Count; index++)
            {
                GameObject uniqueModel = uniqueModels[index];

                int col = index % cols; 
                int row = index / cols; 

                float tileWidth  = 1f / cols;
                float tileHeight = 1f / rows;

                float uMin = col * tileWidth;
                float vMin = row * tileHeight;

                Rect uvRect =  new Rect(uMin, vMin, tileWidth, tileHeight);
                
                uniqueModelsUvRect.Add(uniqueModel, uvRect);
            }

            for (int index = 0; index < totalVariants; index++)
            {
                SkinData skinData = skinsData[index];
                
                SkinOptionButton skinOptionButton = Instantiate(m_SkinOptionButtonPrefab, m_SkinOptionsParent);
                skinOptionButton.transform.localPosition = Vector3.zero;
                skinOptionButton.transform.localRotation = Quaternion.identity;
                skinOptionButton.transform.localScale = Vector3.one;

                skinOptionButton.m_RawImageContent.material = sharedUiMat;
                skinOptionButton.m_RawImageContent.color = skinData.Color.m_Colors[0];
                skinOptionButton.m_RawImageContent.uvRect = uniqueModelsUvRect[skinData.Brush.m_PreviewPrefab];

                skinOptionButton.SetData(skinData);
            }
        }
        
        public void OnClickBackButton()
        {
            if (GameService.currentPhase == GamePhase.SKIN_SELECTION)
                GameService.ChangePhase(GamePhase.MAIN_MENU);
        }
        
        protected override void OnGamePhaseChanged(GamePhase _GamePhase)
        {
            base.OnGamePhaseChanged(_GamePhase);
            switch (_GamePhase)
            {
                case GamePhase.SKIN_SELECTION:
                    m_SkinPreviewGrid.gameObject.SetActive(true);
                    Transition(true);
                    break;
                default:
                    m_SkinPreviewGrid.gameObject.SetActive(false);
                    if (m_Visible)
                        Transition(false);
                    break;
            }
        }
    }
}
