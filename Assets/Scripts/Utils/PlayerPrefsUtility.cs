using UnityEngine;

namespace Utils
{
    public static class PlayerPrefsUtility
    {
        public static void DeleteAll()
        {
            PlayerPrefs.DeleteAll();
        }
        public static bool GetBool(string key, bool defaultValue)
        {
            return PlayerPrefs.GetInt(key, defaultValue ? 1 : 0) == 1;
        }

        public static void SetBool(string key, bool value)
        {
            PlayerPrefs.SetInt(key, value ? 1 : 0);
            PlayerPrefs.Save();
        }
    }
}