using System.Collections.Generic;
using System.Linq;
using Gameplay.Data;
using Interfaces.Services;
using Services;
using UnityEngine;
using UnityEngine.UI;
using Utils;
using Zenject;

namespace UI
{
    public class SkinSelectionView : View<SkinSelectionView>
    {
        public SkinPreviewGrid m_SkinPreviewGrid;
        public Transform m_SkinOptionsParent;
        public SkinOptionButton m_SkinOptionButtonPrefab;
        public RawImage m_currentBrush;

        private Dictionary<GameObject, Rect> m_UniqueModelsUvRect;
        private IStatsService m_StatsService;
        private List<SkinData> m_Skins;

        [Inject]
        public void Construct(IStatsService statsService)
        {
            m_StatsService = statsService;
        }

        protected override void Awake()
        {
            base.Awake();
            m_Skins = GameService.m_Skins;
            PopulateSkins();
        }

        private void SelectSkin(int skinId)
        {
            if (m_Skins == null || m_Skins.Count == 0)
                return;

            skinId = Mathf.Clamp(skinId, 0, m_Skins.Count - 1);

            m_StatsService.FavoriteSkin = skinId;

            var skinData = m_Skins[skinId];
            ApplySkinVisuals(m_currentBrush, skinData);
        }

        private void PopulateSkins()
        {
            int totalVariants = m_Skins.Count;

            List<GameObject> uniqueModels = m_Skins
                .Select(s => s.Brush.m_PreviewPrefab)
                .Distinct()
                .ToList();

            int totalModels = uniqueModels.Count;
            int cols = Mathf.CeilToInt(Mathf.Sqrt(totalModels));
            int rows = Mathf.CeilToInt((float)totalModels / cols);

            var sharedUiMat = GenerateSharedMaterial(totalModels, cols, rows, uniqueModels);

            m_currentBrush.material = sharedUiMat;

            PopulateUniqueModelUvRectDictionary(uniqueModels, cols, rows);
            PopulateScrollableSkinButtons(totalVariants, m_Skins, sharedUiMat);

            int favoriteSkin = Mathf.Min(m_StatsService.FavoriteSkin, m_Skins.Count - 1);
            SelectSkin(favoriteSkin);
        }

        private void PopulateUniqueModelUvRectDictionary(List<GameObject> uniqueModels, int cols, int rows)
        {
            m_UniqueModelsUvRect = new Dictionary<GameObject, Rect>(uniqueModels.Count);

            float tileWidth = 1f / cols;
            float tileHeight = 1f / rows;

            for (int index = 0; index < uniqueModels.Count; index++)
            {
                GameObject uniqueModel = uniqueModels[index];

                int col = index % cols;
                int row = index / cols;

                float uMin = col * tileWidth;
                float vMin = row * tileHeight;

                var uvRect = new Rect(uMin, vMin, tileWidth, tileHeight);
                m_UniqueModelsUvRect.Add(uniqueModel, uvRect);
            }
        }

        private void PopulateScrollableSkinButtons(int totalVariants, List<SkinData> skinsData, Material sharedUiMat)
        {
            for (int index = 0; index < totalVariants; index++)
            {
                var skinData = skinsData[index];

                var skinOptionButton = Instantiate(m_SkinOptionButtonPrefab, m_SkinOptionsParent);
                var buttonTransform = skinOptionButton.transform;
                buttonTransform.localPosition = Vector3.zero;
                buttonTransform.localRotation = Quaternion.identity;
                buttonTransform.localScale = Vector3.one;

                skinOptionButton.m_RawImageContent.material = sharedUiMat;
                ApplySkinVisuals(skinOptionButton.m_RawImageContent, skinData);

                int currentIndex = index;
                skinOptionButton.m_Button.onClick.AddListener(() => SelectSkin(currentIndex));
            }
        }

        private void ApplySkinVisuals(RawImage image, SkinData skinData)
        {
            image.color = skinData.Color.m_Colors[0];
            image.uvRect = m_UniqueModelsUvRect[skinData.Brush.m_PreviewPrefab];
        }

        private Material GenerateSharedMaterial(int totalModels, int cols, int rows, List<GameObject> uniqueModels)
        {
            m_SkinPreviewGrid.Setup(
                totalModels,
                cols,
                rows,
                uniqueModels,
                out RenderTexture atlasRenderTexture,
                out RenderTexture atlasMaskRenderTexture);

            var sharedUiMat = new Material(m_SkinOptionButtonPrefab.m_RawImageContent.material)
            {
                name = "UI_RecolorMat_Runtime"
            };

            sharedUiMat.SetTexture("_MainTex", atlasRenderTexture);
            sharedUiMat.SetTexture("_MaskTex", atlasMaskRenderTexture);

            return sharedUiMat;
        }

        public void OnClickBackButton()
        {
            if (GameService.currentPhase == GamePhase.SKIN_SELECTION)
            {
                GameService.ChangePhase(GamePhase.MAIN_MENU);
            }
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

