using UnityEngine;

public class PlayerDistanceTracker : MonoBehaviour
{
    private float startZPosition;
    [SerializeField] private float distanceTravelled;
    [SerializeField] private float checkDistance = 250f; // Changed from 300f to 250f
    [SerializeField] private PlayerAnimControl playerAnimControl;
    [SerializeField] private PlayerMove playerMove;
    [SerializeField] private PowerUpManager powerUpManager; // Add this line

    private void Start()
    {
        ResetDistance();
    }

    private void Update()
    {
        if (GameManager.Instance.CurrentState == GameManager.GameState.Game &&
            playerAnimControl.isGameOn)
        {
            TrackDistance();
        }
    }

    private void TrackDistance()
    {
        float currentDistance = transform.position.z - startZPosition;
        if (currentDistance > distanceTravelled)
        {
            float increment = currentDistance - distanceTravelled;
            distanceTravelled = currentDistance;
            ScoreManager.Instance.UpdateDistanceScore(distanceTravelled);
        }
        if (distanceTravelled > checkDistance)
        {
            checkDistance += 250f; // Changed from 300f to 250f
            playerMove.IncreaseSpeedBoost();
        }
    }

    public float GetCurrentDistance()
    {
        return distanceTravelled;
    }

    public float GetCheckDistance()
    {
        return checkDistance;
    }

    public void ResetDistance()
    {
        startZPosition = transform.position.z;
        checkDistance = 250f; // Changed from 300f to 250f
        distanceTravelled = 0f;
        // Ensure score manager is updated with reset distance
        if (ScoreManager.Instance != null)
        {
            ScoreManager.Instance.UpdateDistanceScore(0f);
        }

        // Reset boosts in PowerUpManager
        if (powerUpManager != null)
        {
            powerUpManager.ResetBoosts();
        }
    }
}