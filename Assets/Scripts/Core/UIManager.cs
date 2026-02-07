using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    [Header("UI Panels")]
    public GameObject menuPanel;
    public GameObject gameplayPanel;
    public GameObject winPanel;
    public GameObject failPanel;

    public void UpdateUI(GameManager.GameState state)
    {
        // Disable all
        menuPanel?.SetActive(false);
        gameplayPanel?.SetActive(false);
        winPanel?.SetActive(false);
        failPanel?.SetActive(false);

        // Enable based on state
        switch (state)
        {
            case GameManager.GameState.Menu:
                menuPanel?.SetActive(true);
                break;
            case GameManager.GameState.Playing:
                gameplayPanel?.SetActive(true);
                break;
            case GameManager.GameState.Win:
                winPanel?.SetActive(true);
                break;
            case GameManager.GameState.Fail:
                failPanel?.SetActive(true);
                break;
        }
    }
}
