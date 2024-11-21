using UnityEngine;

public class InputManager : MonoBehaviour
{
    [SerializeField] private PlayerInputControl playerInputControl;
    [SerializeField] private PlayerAnimControl playerAnimControl;
    [SerializeField] private PlayerMove playerMove;

    private float lastPauseTime = 0f;
    private const float PAUSE_COOLDOWN = 0.5f;

    private void Update()
    {
        if (GameManager.Instance.IsCountingDown)
        {
            // Don't process any input during countdown
            return;
        }

        if (GameManager.Instance.CurrentState == GameManager.GameState.Game && playerAnimControl.isGameOn)
        {
            playerMove.MoveForward();
            GameInputPlayer();
            PauseButtonHandler();
        }
        // Remove the B key check since it's handled by countdown now
        else if (GameManager.Instance.CurrentState == GameManager.GameState.Pause)
        {
            PauseButtonHandler();
        }
    }

    private void GameInputPlayer()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            playerInputControl.KeyA();
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            playerInputControl.KeyD();
        }
        if (Input.GetKey(KeyCode.Mouse0)) // Assuming space is the shoot button
        {
            playerInputControl.KeyShootDown();
        }
        else if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            playerInputControl.KeyShootUp();
        }
    }

    private void PauseButtonHandler()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && Time.realtimeSinceStartup - lastPauseTime > PAUSE_COOLDOWN)
        {
            lastPauseTime = Time.realtimeSinceStartup;

            if (GameManager.Instance.CurrentState == GameManager.GameState.Game)
            {
                GameManager.Instance.PauseGame();
            }
            else if (GameManager.Instance.CurrentState == GameManager.GameState.Pause)
            {
                GameManager.Instance.ResumeGame();
            }
        }
    }
}


