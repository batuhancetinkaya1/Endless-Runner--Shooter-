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
    [SerializeField] private PlayerPowerUpHandler powerUpHandler; // PlayerPowerUpHandler bileþeni

    private bool isDying = false;

    private void Awake()
    {
        // PlayerPowerUpHandler bileþenini doðrudan atamak için kontrol ediyoruz
        if (powerUpHandler == null)
        {
            Debug.LogWarning("PowerUpHandler bileþeni atanmamýþ, lütfen Unity'de atayýn.");
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
                // Block bileþenini alýp patlatmayý çaðýrýyoruz
                Block block = collision.gameObject.GetComponent<Block>();
                if (block != null)
                {
                    block.Explode();
                    // Kalkaný devre dýþý býrakmadan çýkýyoruz
                    return;
                }
            }

            HandleDeath();
        }
        else if (collision.gameObject.CompareTag("Building") || collision.gameObject.CompareTag("NoSpawn"))
        {
            // Bina ile çarpýþmalar her durumda oyuncuyu öldürür
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

            // Ölümden sonra kalkaný devre dýþý býrakýyoruz
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
