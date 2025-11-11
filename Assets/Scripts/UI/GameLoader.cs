using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI
{
    public class LoadingManager : MonoBehaviour
    {
        public Image loadingFill;
        public string sceneToLoad = "Game";
        public float duration = 2f;

        private void Start()
        {
            StartCoroutine(ProgressAndLoad());
        }

        private IEnumerator ProgressAndLoad()
        {
            float elapsed = 0f;

            // Progression du fillAmount sur 2 secondes
            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                loadingFill.fillAmount = Mathf.Clamp01(elapsed / duration);
                yield return null;
            }

            // Lancer le chargement de la scène en mode additive
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneToLoad, LoadSceneMode.Single);
        }
    }
}