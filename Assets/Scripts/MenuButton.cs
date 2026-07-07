using UnityEngine;

public class MenuButton : MonoBehaviour
{
   public void OnClickStart()
    {
        // Load the scene with build index 1
        UnityEngine.SceneManagement.SceneManager.LoadScene("Bali");
    }
    public void OnClickGallery()
    {
        // Load the scene with build index 2
        UnityEngine.SceneManagement.SceneManager.LoadScene("Gallery");
    }
    public void OnQuit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        // Quit the application
        Application.Quit();
    }
}
