using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void QuitGame()
    {
        Debug.Log("QUIT!");
        Application.Quit();
    }
    
    public void Retry()
    {
        Scene currentScene = SceneManager.GetActiveScene();

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}