using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : Singleton<UIManager>
{
    [SerializeField] private GameObject menuPanel;
    [SerializeField] private GameObject gamePanel;
    [SerializeField] private GameObject highScorePanel;
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject deathPanel;
    [SerializeField] private GameObject creditsPanel;
    [SerializeField] private GameObject controlsPanel;
    [SerializeField] private GameObject countdownPanel;
    [SerializeField] private TMP_Text countdownText;

    public void ShowPanel(string panelName)
    {
        menuPanel.SetActive(panelName == "Menu");
        gamePanel.SetActive(panelName == "Game");
        highScorePanel.SetActive(panelName == "HighScore");
        creditsPanel.SetActive(panelName == "Credits");
        controlsPanel.SetActive(panelName == "Controls");
        pausePanel.SetActive(panelName == "Pause");
        deathPanel.SetActive(panelName == "Death");
        countdownPanel.SetActive(panelName == "Countdown");
    }

    public void ResetCountdownText()
    {
        if (countdownText != null)
        {
            countdownText.text = "3";
        }
    }

    public void UpdateCountdownText(string text)
    {
        if (countdownText != null)
        {
            countdownText.text = text;
        }
    }

    public void OnCreditsButtonPressed()
    {
        GameManager.Instance.ShowCredits();
    }

    public void OnControlsButtonPressed()
    {
        GameManager.Instance.ShowControls();
    }

    public void OnPlayButtonPressed()
    {
        GameManager.Instance.StartGame();
    }

    public void OnHighScoreButtonPressed()
    {
        GameManager.Instance.ShowHighScores();
    }

    public void OnPauseButtonPressed()
    {
        GameManager.Instance.PauseGame();
    }

    public void OnReturnToGameButtonPressed()
    {
        GameManager.Instance.ResumeGame();
    }

    public void OnRestartButtonPressed()
    {
        GameManager.Instance.RestartGame(); // You might want to reset the scene
    }

    public void OnGoToMainMenuButtonPressed()
    {
        GameManager.Instance.ReturnToMenu();
    }
}

