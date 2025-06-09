using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private string levelChooserSceneName = "LevelChooser";
    
    public void PlayGame()
    {
        SceneManager.LoadScene(levelChooserSceneName);
    }
    
    public void QuitGame()
    {
        Debug.Log("Quitting game...");
        Application.Quit();
    }
}