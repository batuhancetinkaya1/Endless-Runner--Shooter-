using DG.Tweening;
using UnityEngine;

public class FireRateBoostScript : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float baseFireRateBoost = 0.05f;
    [SerializeField] private AudioClip powerUpSound;

    // References set via PowerUpManager instantiation
    private PlayerShooter playerShooter;
    private Transform targetTransform;

    private void Start()
    {
        SetupAnimations();
    }

    private void SetupAnimations()
    {
        // Single rotation tween
        transform.DORotate(new Vector3(0, 360, 0), 1f, RotateMode.FastBeyond360)
            .SetEase(Ease.Linear)
            .SetLoops(-1, LoopType.Incremental);

        // Single move tween
        transform.DOMoveY(transform.position.y + 0.5f, 0.5f)
            .SetEase(Ease.OutSine)
            .SetLoops(-1, LoopType.Yoyo);
    }

    public void Initialize(PlayerShooter shooter)
    {
        playerShooter = shooter;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerShooter.ApplyFireRateBoost(baseFireRateBoost);
            SoundManager.Instance.PlaySound(powerUpSound, transform);
            DOTween.Kill(transform);
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        DOTween.Kill(transform);
    }
}