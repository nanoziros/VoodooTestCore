using Services;
namespace Utils
{
    public static class GameModeExtensions
    {
        private const string BoosterSuffix = "_Booster";

        public static string GetSuffix(this GameMode mode)
        {
            switch (mode)
            {
                case GameMode.BOOSTER:
                    return BoosterSuffix;
                default:
                    return string.Empty;
            }
        }
    }
}
