using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneHandler : Singleton<SceneHandler>
{
    public void RestartGame()
    {     
        StartCoroutine(RestartGameProccess());
    }

    public IEnumerator RestartGameProccess()
    {
        yield return SceneManager.LoadSceneAsync(sceneBuildIndex: SceneManager.GetActiveScene().buildIndex);
        
        ContinueGame();
    }

    public IEnumerator LoadNextLevelProcess()
    {
        yield return SceneManager.LoadSceneAsync(sceneBuildIndex: SceneManager.GetActiveScene().buildIndex + 1);
        ContinueGame();
    }

    public void LoadNextScene()
    {
        StartCoroutine(LoadNextLevelProcess());
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void PauseGame()
    {
        Time.timeScale = 0f;
    }

    public void ContinueGame()
    {
        Time.timeScale = 1f;
       GameManager.Instance.ResetGameManager();
    }
}
