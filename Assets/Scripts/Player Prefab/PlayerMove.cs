using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float baseSpeed = 5f;
    [SerializeField] public float speedBoost = 0f;
    [SerializeField] private Animator animator;
    [SerializeField] private PlayerDeathHandler playerDeathHandler;
    [SerializeField] private PlayerAnimControl playerAnimControl;
    [SerializeField] private AudioClip deathSoundEffect;
    [SerializeField] private GameObject deathEffect;
    [SerializeField] private PlayerPowerUpHandler powerUpHandler; // PlayerPowerUpHandler bile�eni

    private bool isDying = false;

    private void Awake()
    {
        // PlayerPowerUpHandler bile�enini do�rudan atamak i�in kontrol ediyoruz
        if (powerUpHandler == null)
        {
            Debug.LogWarning("PowerUpHandler bile�eni atanmam��, l�tfen Unity'de atay�n.");
        }
    }

    public void MoveForward()
    {
        transform.Translate(Vector3.forward * (baseSpeed + speedBoost) * Time.deltaTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Block"))
        {
            if (powerUpHandler != null && powerUpHandler.IsShieldActive())
            {
                // Block bile�enini al�p patlatmay� �a��r�yoruz
                Block block = collision.gameObject.GetComponent<Block>();
                if (block != null)
                {
                    block.Explode();
                    // Kalkan� devre d��� b�rakmadan ��k�yoruz
                    return;
                }
            }

            HandleDeath();
        }
        else if (collision.gameObject.CompareTag("Building") || collision.gameObject.CompareTag("NoSpawn"))
        {
            // Bina ile �arp��malar her durumda oyuncuyu �ld�r�r
            HandleDeath();
        }
    }

    private void HandleDeath()
    {
        if (!isDying)
        {
            isDying = true;
            SoundManager.Instance.PlaySound(deathSoundEffect, transform);
            Instantiate(deathEffect, transform.position + new Vector3(0, 1, 0), Quaternion.identity);
            animator.SetBool("IsDeath", true);
            playerAnimControl.GameOff();
            StartCoroutine(playerDeathHandler.HandleDeath());

            // �l�mden sonra kalkan� devre d��� b�rak�yoruz
            if (powerUpHandler != null)
            {
                powerUpHandler.ResetAllPowerUps();
            }
        }
    }

    public void ResetDeathState()
    {
        speedBoost = 0f;
        isDying = false;
    }

    public void IncreaseSpeedBoost()
    {
        speedBoost += 0.5f;
        speedBoost = Mathf.Clamp(speedBoost, 0f, 5f);
    }
}
