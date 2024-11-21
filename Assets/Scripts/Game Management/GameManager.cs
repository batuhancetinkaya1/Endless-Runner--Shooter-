using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    #region Enums and State

    public enum GameState
    {
        Menu,
        Game,
        HighScore,
        Pause,
        Death,
        Controls,
        Credits,
        Countdown
    }

    public GameState CurrentState { get; private set; } = GameState.Menu;
    public bool IsCountingDown { get; private set; }
    private bool isTransitioning = false;
    [SerializeField] private AudioClip countdownSound;
    [SerializeField] private AudioClip countdownFinishSound;

    #endregion

    #region Serialized Fields

    [Header("Player Settings")]
    [SerializeField] private GameObject player;
    [SerializeField] private PlayerAnimControl playerAnimControl;
    [SerializeField] private PlayerMove playerMove;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private PlayerDistanceTracker distanceTracker;
    [SerializeField] private PlayerChangeLine playerChangeLine;
    [SerializeField] private PlayerShooter playerShooter;
    [SerializeField] private PlayerPowerUpHandler powerUpHandler;

    [Header("Camera Settings")]
    [SerializeField] private SmoothCameraFollow cameraFollow;
    [SerializeField] private Vector3 playerStartPosition = new Vector3(-1.5f, -1.85f, -6);
    [SerializeField] private Vector3 cameraOffset = new Vector3(0, 5, -10);

    [Header("Level Settings")]
    [SerializeField] private TileManager tileManager;
    [SerializeField] private PowerUpManager powerUpManager;

    #endregion

    #region Unity Lifecycle

    private void Start()
    {
        SwitchToMenu();
    }

    #endregion

    #region State Management Methods

    public void SwitchToMenu()
    {
        if (isTransitioning) return;

        CurrentState = GameState.Menu;
        Time.timeScale = 0;
        UIManager.Instance?.ShowPanel("Menu");
        ResetGameElements();
    }

    public void StartGame()
    {
        StartCountdown();
    }

    public void ShowHighScores()
    {
        CurrentState = GameState.HighScore;
        UIManager.Instance?.ShowPanel("HighScore");
    }

    public void ShowCredits()
    {
        CurrentState = GameState.Credits;
        UIManager.Instance?.ShowPanel("Credits");
    }

    public void ShowControls()
    {
        CurrentState = GameState.Controls;
        UIManager.Instance?.ShowPanel("Controls");
    }

    public void PauseGame()
    {
        CurrentState = GameState.Pause;
        Time.timeScale = 0;
        UIManager.Instance?.ShowPanel("Pause");
    }

    public void ResumeGame()
    {
        CurrentState = GameState.Game;
        Time.timeScale = 1;
        UIManager.Instance?.ShowPanel("Game");
    }

    public void GameOver()
    {
        if (isTransitioning) return;
        isTransitioning = true;

        CurrentState = GameState.Death;
        Time.timeScale = 0;

        playerAnimControl?.DeathTrue();
        playerAnimControl?.GameOff();

        ScoreManager.Instance?.UpdateDeathPanelUI();
        UIManager.Instance?.ShowPanel("Death");
        ScoreManager.Instance?.SaveHighScore();

        StartCoroutine(ClearTransitionFlag());
    }

    public void RestartGame()
    {
        if (isTransitioning) return;
        isTransitioning = true;

        CurrentState = GameState.Countdown;
        ResetGameElements(true);
        UIManager.Instance.ResetCountdownText();
        Time.timeScale = 1;

        StartCoroutine(DelayedCountdownStart());
    }

    public void ReturnToMenu()
    {
        if (isTransitioning) return;
        isTransitioning = true;

        Time.timeScale = 0;
        ResetGameElements(true);

        CurrentState = GameState.Menu;
        UIManager.Instance?.ShowPanel("Menu");
        UIManager.Instance?.ResetCountdownText();

        StartCoroutine(ClearTransitionFlag());
    }

    #endregion

    #region Reset System

    private void ResetGameElements(bool resetScore = false)
    {
        ResetPlayer();
        ResetLevelElements();

        if (resetScore)
        {
            ScoreManager.Instance?.ResetStats();
        }
    }

    private void ResetPlayer()
    {
        if (player == null) return;

        player.transform.position = playerStartPosition;
        player.transform.rotation = Quaternion.identity;

        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        playerAnimControl.Reset();
        playerAnimControl.DeathFalse();
        playerAnimControl.GameOff();

        playerMove.ResetDeathState();
        distanceTracker.ResetDistance();
        playerChangeLine.Reset();
        playerShooter.ShooterReset();
        powerUpHandler.ResetAllPowerUps();

        cameraFollow.ResetCamera(player.transform);
    }

    private void ResetLevelElements()
    {
        BulletManager.ClearAllBullets();
        tileManager.ResetTiles();
        powerUpManager.ResetBoosts();
    }

    #endregion

    #region Countdown System

    private IEnumerator CountdownSequence()
    {
        if (isTransitioning) yield break;

        IsCountingDown = true;
        UIManager.Instance?.ShowPanel("Countdown");
        Time.timeScale = 1;

        yield return new WaitForSecondsRealtime(0.5f);

        for (int i = 3; i > 0; i--)
        {
            UIManager.Instance?.UpdateCountdownText(i.ToString());
            SoundManager.Instance.PlaySound(countdownSound, transform, 2f);
            yield return new WaitForSecondsRealtime(1f);
        }

        UIManager.Instance?.UpdateCountdownText("GO!");
        SoundManager.Instance.PlaySound(countdownFinishSound, transform, 2f);
        yield return new WaitForSecondsRealtime(0.5f);

        CurrentState = GameState.Game;
        UIManager.Instance?.ShowPanel("Game");

        playerAnimControl.StartRunning();
        playerAnimControl.GameOn();

        IsCountingDown = false;
    }

    public void StartCountdown()
    {
        StartCoroutine(CountdownSequence());
    }

    private IEnumerator ClearTransitionFlag()
    {
        yield return new WaitForSecondsRealtime(0.5f);
        isTransitioning = false;
    }

    private IEnumerator DelayedCountdownStart()
    {
        yield return new WaitForSecondsRealtime(0.5f);
        isTransitioning = false;
        StartCountdown();
    }

    #endregion
}