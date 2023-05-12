using TMPro;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private GameObject m_loseScreen;
    [SerializeField] private GameObject m_winScreen;
    [SerializeField] private TMP_Text m_GemAmountText;

    private const string GEM_TEXT = "/ 3";

    private int m_collectedGems = 0;
    private const int MAX_GEMS_PER_LEVEL = 3;

    private void OnEnable()
    {
        Enemy.OnGameLost += GameLost;
        Collector.OnGemCollected += GameWon;
    }

    private void OnDestroy()
    {
        Enemy.OnGameLost -= GameLost;
        Collector.OnGemCollected -= GameWon;
    }

    public void ResetGameManager()
    {
        m_loseScreen.SetActive(false);
        m_winScreen.SetActive(false);
        m_collectedGems = 0;
    }

    public void GameLost()
    {
        m_loseScreen.SetActive(true);
        SceneHandler.Instance.PauseGame();            
    }

    public void GameWon()
    {
        m_collectedGems++;
        m_GemAmountText.text = m_collectedGems.ToString() + GEM_TEXT;

        if (m_collectedGems == MAX_GEMS_PER_LEVEL)
        {
            m_winScreen.SetActive(true);
            SceneHandler.Instance.PauseGame();
        }
    }
}
