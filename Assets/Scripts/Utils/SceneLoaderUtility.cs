using UnityEngine.SceneManagement;
namespace Utils
{
    public static class SceneLoaderUtility
    {
        public static void LoadMainMenu()
        {
            SceneManager.LoadScene("Game");
        }
    }
}
