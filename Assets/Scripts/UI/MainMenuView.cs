using System.Collections.Generic;
using Interfaces.Services;
using Services;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI
{
    public class MainMenuView : View<MainMenuView>
    {
        private const string m_BestScorePrefix = "BEST SCORE ";

        public Text m_BestScoreText;
        public Image m_BestScoreBar;
        public GameObject m_BestScoreObject;
        public InputField m_InputField;
        public List<Image> m_ColoredImages;
        public List<Text> m_ColoredTexts;

        public GameObject m_currentBrushSelectionParent;
        public GameObject m_BrushesPrefab;
        public int m_IdSkin = 0;
        public GameObject m_PointsPerRank;
        public RankingView m_RankingView;
        public Button m_SkinSelectionScreenButton;
        
        [Header("Ranks")]
        public string[] m_Ratings;

        private IStatsService m_StatsService;
        private IFeatureFlagService m_FeatureFlagService;
        private bool m_IsSkinSelectionScreenEnabled;

        [Inject]
        public void Construct(IStatsService statsService, IFeatureFlagService featureFlagService)
        {
            m_StatsService = statsService;
            m_FeatureFlagService = featureFlagService;
        }

        protected override void Awake()
        {
            base.Awake();

            m_IsSkinSelectionScreenEnabled = m_FeatureFlagService.SkinSelectionScreenEnabled;
            m_IdSkin = m_StatsService.FavoriteSkin;
        }

        public void OnSkinSelectionScreenButton()
        {
            //TODO: open skin selection screen
        }

        public void OnPlayButton()
        {
            if (GameService.currentPhase == GamePhase.MAIN_MENU)
                GameService.ChangePhase(GamePhase.LOADING);
        }

        protected override void OnGamePhaseChanged(GamePhase _GamePhase)
        {
            base.OnGamePhaseChanged(_GamePhase);

            switch (_GamePhase)
            {
                case GamePhase.MAIN_MENU:
                    DisplayBrushSelectionMode(true);
                    Transition(true);
                    break;

                case GamePhase.LOADING:
                    DisplayBrushSelectionMode(false);

                    if (m_Visible)
                        Transition(false);
                    break;
                
                case GamePhase.DEBUG:
                    DisplayBrushSelectionMode(false);

                    Transition(false);
                    break;
            }
        }

        void DisplayBrushSelectionMode(bool isSelectionEnabled)
        {
            if (isSelectionEnabled)
            {
                if (m_IsSkinSelectionScreenEnabled)
                {
                    ManageOnMenuBrushSelectionDisplay(false);
                    m_SkinSelectionScreenButton.gameObject.SetActive(true);
                }
                else
                {
                    ManageOnMenuBrushSelectionDisplay(true);
                    m_SkinSelectionScreenButton.gameObject.SetActive(false);
                }
            }
            else
            {
                ManageOnMenuBrushSelectionDisplay(false);
                m_SkinSelectionScreenButton.gameObject.SetActive(false);
            }
        }
        
        void ManageOnMenuBrushSelectionDisplay(bool state)
        {
            m_currentBrushSelectionParent.SetActive(state);
            m_BrushesPrefab.SetActive(state);
        }

        public void SetTitleColor(Color _Color)
        {
            int favoriteSkin = Mathf.Min(m_StatsService.FavoriteSkin, GameService.m_Skins.Count - 1);
            m_BrushesPrefab.GetComponent<BrushMainMenu>().Set(GameService.m_Skins[favoriteSkin]);
            string playerName = m_StatsService.GetNickname();

            if (playerName != null)
                m_InputField.text = playerName;

            for (int i = 0; i < m_ColoredImages.Count; ++i)
                m_ColoredImages[i].color = _Color;

            for (int i = 0; i < m_ColoredTexts.Count; i++)
                m_ColoredTexts[i].color = _Color;
            
            m_RankingView.gameObject.SetActive(true);
            m_RankingView.RefreshNormal();
        }

        public void OnSetPlayerName(string _Name)
        {
            m_StatsService.SetNickname(_Name);
        }

        public string GetRanking(int _Rank)
        {
            return m_Ratings[_Rank];
        }

        public int GetRankingCount()
        {
            return m_Ratings.Length;
        }

        public void LeftButtonBrush()
        {
            ChangeBrush(m_IdSkin - 1);
        }

        public void RightButtonBrush()
        {
            ChangeBrush(m_IdSkin + 1);
        }

        public void ChangeBrush(int _NewBrush)
        {
            _NewBrush = Mathf.Clamp(_NewBrush, 0, GameService.m_Skins.Count);
            m_IdSkin = _NewBrush;
            if (m_IdSkin >= GameService.m_Skins.Count)
                m_IdSkin = 0;
            GameService.m_PlayerSkinID = m_IdSkin;
            int favoriteSkin = Mathf.Min(m_StatsService.FavoriteSkin, GameService.m_Skins.Count - 1);
            m_BrushesPrefab.GetComponent<BrushMainMenu>().Set(GameService.m_Skins[favoriteSkin]);
            m_StatsService.FavoriteSkin = m_IdSkin;
            GameService.SetColor(GameService.ComputeCurrentPlayerColor(true, 0));
        }
    }
}
