using Services;
using UnityEngine;
using Utils;
namespace UI
{
    public class SkinSelectionView :  View<SkinSelectionView>
    {
        public Camera m_PreviewCamera;
        public Transform m_SkinOptionsParent;
        public SkinOptionButton m_SkinOptionButtonPrefab;
        public LayerMask m_PreviewLayerMask;
        public int m_TotalVariants = 12;
        public Vector2Int m_SkinPreviewTileResolution = new Vector2Int(128, 128);
        private SkinPreviewGridBuilder m_SkinPreviewGridBuilder;
        
        protected override void Awake()
        {
            base.Awake();
            PopulateSkins();
        }

        private void PopulateSkins()
        {
            m_SkinPreviewGridBuilder = new SkinPreviewGridBuilder(
                m_PreviewCamera,
                m_SkinPreviewTileResolution,
                m_TotalVariants,
                m_PreviewLayerMask);
            m_SkinPreviewGridBuilder.Build();
            
            for (int index = 0; index < m_TotalVariants; index++)
            {
                SkinOptionButton skinOptionButton = Instantiate(m_SkinOptionButtonPrefab, m_SkinOptionsParent);
                skinOptionButton.transform.localPosition = Vector3.zero;
                skinOptionButton.transform.localRotation = Quaternion.identity;
                skinOptionButton.transform.localScale = Vector3.one;
                
                skinOptionButton.m_RawImageContent.texture = m_SkinPreviewGridBuilder.m_AtlasRenderTexture;
                skinOptionButton.m_RawImageContent.uvRect  = m_SkinPreviewGridBuilder.GetVariantUvRect(index);
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
                    Transition(true);
                    break;
                default:
                    if (m_Visible)
                        Transition(false);
                    break;
            }
        }
    }
}
