using Services;
namespace UI
{
    public class SkinSelectionView :  View<SkinSelectionView>
    {
        protected override void Awake()
        {
            base.Awake();

            // todo:
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
