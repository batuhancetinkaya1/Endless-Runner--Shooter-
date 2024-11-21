using UnityEngine;

public class SmoothCameraFollow : MonoBehaviour
{
    [Header("Target Reference")]
    [SerializeField] private Transform playerTransform;
    [Header("Offset Settings")]
    [SerializeField] private Vector3 offset = new Vector3(0, 5, -10);
    [Header("Smoothing Parameters")]
    [SerializeField] private float smoothSpeed = 5f;
    [SerializeField] private bool lockY = true;
    [Header("Music Settings")]
    [SerializeField] private AudioClip backgroundMusic;

    private Vector3 desiredPosition;
    private Vector3 smoothedPosition;
    private bool isResetting;

    void Start()
    {
        if (playerTransform != null)
        {
            ResetCamera(playerTransform);
        }
        StartMusic();
    }

    void LateUpdate()
    {
        if (playerTransform == null || isResetting) return;

        // Calculate desired position
        desiredPosition = playerTransform.position + offset;

        // Lock Y position if required
        if (lockY)
        {
            desiredPosition.y = transform.position.y;
        }

        // Smoothly interpolate between current and desired positions
        smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);

        // Update camera position
        transform.position = smoothedPosition;
    }

    public void ResetCamera(Transform newTarget)
    {
        isResetting = true;
        playerTransform = newTarget;

        // Instantly move camera to correct position
        desiredPosition = playerTransform.position + offset;
        if (lockY)
        {
            desiredPosition.y = offset.y;
        }
        transform.position = desiredPosition;

        isResetting = false;
    }

    public void StartMusic()
    {
        if (backgroundMusic != null)
        {
            SoundManager.Instance.PlayMusic(backgroundMusic, 0.75f);
        }
    }
}