namespace Interfaces.Services
{
    public interface IFeatureFlagService
    {
        bool BoosterGameModeEnabled { get; set; }
        bool SkinSelectionScreenEnabled { get; set; }
    }
}