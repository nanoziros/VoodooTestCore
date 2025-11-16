using Gameplay.Players;
using Interfaces.Services;
using UnityEngine;
using Zenject;
namespace Gameplay.PowerUps
{
    public class PowerUp_RandomPaintBomb : PowerUp
    {
        public static AnimationCurve s_DefaultFillCurve = null;

        public float 			m_Radius = 6.0f;
        public float			m_FillDuration = 0.3f;
        public AnimationCurve	m_FillCurve;
        private ITerrainService	m_TerrainService;
        private IGameService    m_GameService;
        private float           m_RadiusMultiplier;

        [Inject]
        public void ChildConstruct(ITerrainService terrainService, IGameService gameService)
        {
            m_TerrainService = terrainService;
            m_GameService = gameService;
            m_RadiusMultiplier = 1f;
            s_DefaultFillCurve = m_FillCurve;
        }


        public override void OnPlayerTouched (Player _Player)
        {

            m_RadiusMultiplier = Mathf.Clamp(_Player.GetSize() / _Player.GetMinSize(), 1f, 2.5f);
            UnregisterMap();
            m_Model.enabled = false;
            m_ParticleSystem.Play(true);
            m_IdleParticleSystem.Stop(true, ParticleSystemStopBehavior.StopEmitting);
            m_Shadow.SetActive(false);

            Vector3 position = m_GameService.CalculateRandomSpawnPosition();
            m_TerrainService.FillCircle(_Player, position, m_Radius * m_RadiusMultiplier, m_FillDuration, SelfDestroy);
        }

        private void SelfDestroy()
        {
            Destroy(gameObject);
        }
    }
}
