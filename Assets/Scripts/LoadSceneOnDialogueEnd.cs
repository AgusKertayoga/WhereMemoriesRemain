using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts
{
    public class LoadSceneOnDialogueEnd : MonoBehaviour
    {
        [Tooltip("Name of the scene to load when dialogue ends")]
        public string sceneName;

        [Tooltip("Optional delay (seconds) before loading the scene")]
        public float delaySeconds = 0f;

        // Hook this method to DialogueManager.endDialogueEvent in the Inspector
        public void OnDialogueEnd()
        {
            if (string.IsNullOrEmpty(sceneName))
            {
                Debug.LogWarning("LoadSceneOnDialogueEnd: sceneName is empty.");
                return;
            }

            if (delaySeconds <= 0f)
                SceneManager.LoadScene(sceneName);
            else
                StartCoroutine(LoadAfterDelay());
        }

        private IEnumerator LoadAfterDelay()
        {
            yield return new WaitForSeconds(delaySeconds);
            SceneManager.LoadScene(sceneName);
        }
    }
}
