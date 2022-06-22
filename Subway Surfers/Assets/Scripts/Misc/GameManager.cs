using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private int currentSceneIndex;
    
    private void Start()
    {
        currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
    }

    public void LoadNextScene()
    {
        SceneManager.LoadScene(currentSceneIndex + 1);
        Resume();
    }

    public void LoadPrevScene()
    {
        SceneManager.LoadScene(currentSceneIndex - 1);
        Resume();
    }

    public void Restart()
    {
        SceneManager.LoadScene(currentSceneIndex);
        Resume();
    }

    public void Pause() => Time.timeScale = 0f;

    public void Resume() => Time.timeScale = 1f;

    public void Exit() => Application.Quit();
}
