using UnityEngine;

public class PlayerShooter : MonoBehaviour
{
    [Header("Initial")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform gunTransform;
    [SerializeField] private float startFireRate = 0.25f;
    [SerializeField] private float minFireRate = 0.1f;
    [SerializeField] private AudioClip shootClip;
    private float lastFireTime;

    [Header("Boost")]
    [SerializeField] private ParticleSystem gunBoostEffectPrefab;
    [SerializeField] private GameObject gun;
    [SerializeField] private float currentFireRateBoost = 0f;
    [SerializeField] private float maxFireRateBoost = 0.15f;
    [SerializeField] private Bullet bulletScript;
    [SerializeField] private PlayerMove playerMoveScript;

    [Header("Light Effects")]
    [SerializeField] private Light pointLight;
    [SerializeField] private Transform lightGlowTransform;
    private float initialLightRange = 0.8f;
    private float maxLightRange = 4f;
    private Vector3 initialGlowScale = new Vector3(0.1f, 0.1f, 0.1f);
    private Vector3 maxGlowScale = Vector3.one;
    private const int MAX_BOOST_COUNT = 5;
    private const float BOOST_STEP = 0.03f; // Her boost için artýþ miktarý (0.15/5)

    private float currentFireRate;
    private ParticleSystem currentGunBoostEffect;

    private void Start()
    {
        SetupBoostEffect();
        if (pointLight != null) pointLight.range = initialLightRange;
        if (lightGlowTransform != null) lightGlowTransform.localScale = initialGlowScale;
    }

    public void TryShoot()
    {
        if (Time.time >= lastFireTime + startFireRate)
        {
            lastFireTime = Time.time;
            FireBullet();
            PlayShootSound();
        }
    }

    private void FireBullet()
    {
        bulletScript.SetSpeedBoost(playerMoveScript.speedBoost);
        GameObject bullet = Instantiate(bulletPrefab, gunTransform.position, gunTransform.rotation);
        Destroy(bullet, 3f);
    }

    private void UpdateBoostEffect()
    {
        if (currentGunBoostEffect == null) return;
        var main = currentGunBoostEffect.main;
        float boostRatio = currentFireRateBoost / maxFireRateBoost;
        main.startSize = 0.1f + (0.9f * boostRatio);

        UpdateLightEffects();

        if (!currentGunBoostEffect.isPlaying)
        {
            currentGunBoostEffect.Play();
        }
    }

    private void UpdateLightEffects()
    {
        // Boost sayýsýný hesapla (0-5 arasý)
        int currentBoostStep = Mathf.FloorToInt(currentFireRateBoost / BOOST_STEP);

        // Point Light range güncelleme
        if (pointLight != null)
        {
            float rangeDifference = maxLightRange - initialLightRange;
            float rangeIncreasePerBoost = rangeDifference / MAX_BOOST_COUNT;
            float newRange = initialLightRange + (rangeIncreasePerBoost * currentBoostStep);
            pointLight.range = Mathf.Clamp(newRange, initialLightRange, maxLightRange);
        }

        // LightGlow scale güncelleme
        if (lightGlowTransform != null)
        {
            float scaleIncreasePerBoost = (1f - initialGlowScale.x) / MAX_BOOST_COUNT;
            float newScale = initialGlowScale.x + (scaleIncreasePerBoost * currentBoostStep);
            newScale = Mathf.Clamp(newScale, initialGlowScale.x, 1f);
            lightGlowTransform.localScale = new Vector3(newScale, newScale, newScale);
        }
    }

    public void ApplyFireRateBoost(float boostAmount)
    {
        currentFireRateBoost = Mathf.Min(currentFireRateBoost + boostAmount, maxFireRateBoost);
        currentFireRate = Mathf.Max(startFireRate - currentFireRateBoost, minFireRate);
        UpdateBoostEffect();
    }

    private void PlayShootSound()
    {
        SoundManager.Instance.PlaySound(shootClip, transform);
    }

    private void SetupBoostEffect()
    {
        if (gunBoostEffectPrefab != null)
        {
            currentGunBoostEffect = Instantiate(gunBoostEffectPrefab, gun.transform);
            var main = currentGunBoostEffect.main;
            main.startSize = 0.1f;
        }
    }

    public void ShooterReset()
    {
        currentFireRateBoost = 0f;
        currentFireRate = startFireRate;

        // Light efektlerini sýfýrla
        if (pointLight != null) pointLight.range = initialLightRange;
        if (lightGlowTransform != null) lightGlowTransform.localScale = initialGlowScale;

        if (currentGunBoostEffect != null)
        {
            var main = currentGunBoostEffect.main;
            main.startSize = 0.1f;
        }
    }
}