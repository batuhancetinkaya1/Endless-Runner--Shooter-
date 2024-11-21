using UnityEngine;

public class Block : MonoBehaviour
{
    [SerializeField] private float health = 3f;
    [SerializeField] private GameObject explosionEffect;
    [SerializeField] private float damage = 1f;
    [SerializeField] private AudioClip hitClip;
    [SerializeField] private AudioClip explodeClip;
    [SerializeField] private PlayerPowerUpHandler powerUpHandler;

    public void Explode()
    {
        if (gameObject == null) return; // Safety check

        SoundManager.Instance?.PlaySound(explodeClip, transform);

        // Create explosion effect
        if (explosionEffect != null)
        {
            Transform blockages = GameObject.Find("Blockages")?.transform;
            if (blockages != null)
            {
                Instantiate(explosionEffect, transform.position, Quaternion.identity, blockages);
            }
            else
            {
                Instantiate(explosionEffect, transform.position, Quaternion.identity);
            }
        }

        // Increment score before destroying
        ScoreManager.Instance?.IncrementBlocksDestroyed();
        Destroy(gameObject);
    }

    public void TakeDamage(float damageAmount)
    {
        health -= damageAmount;
        if (health <= 0)
        {
            Explode();
        }
        else
        {
            PlayHitSound();
        }
    }

    private void PlayHitSound()
    {
        SoundManager.Instance?.PlaySound(hitClip, transform);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            TakeDamage(damage);
        }
        else if (collision.gameObject.CompareTag("Player"))
        {
            if (powerUpHandler != null && powerUpHandler.IsShieldActive())
            {
                // Just explode the block if shield is active, don't deactivate shield
                Explode();
            }
            // If no shield, the PlayerMove script will handle the death logic
        }
    }
}