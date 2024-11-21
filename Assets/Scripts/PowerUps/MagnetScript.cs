using DG.Tweening;
using UnityEngine;

public class MagnetScript : MonoBehaviour
{
    private float duration;
    [SerializeField] private GameObject magnetBoostEffectObject;

    // Reference set via PowerUpManager instantiation
    private PlayerPowerUpHandler powerUpHandler;

    private void Start()
    {
        SetupAnimations();
    }

    private void SetupAnimations()
    {
        transform.DORotate(new Vector3(0, 360, 0), 1f, RotateMode.FastBeyond360)
            .SetEase(Ease.Linear)
        .SetLoops(-1, LoopType.Incremental);
        transform.DOMoveY(transform.position.y + 0.5f, 0.5f)
            .SetEase(Ease.OutSine)
            .SetLoops(-1, LoopType.Yoyo);
    }

    public void Initialize(PlayerPowerUpHandler handler, float magnetDuration)
    {
        powerUpHandler = handler;
        duration = magnetDuration;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            powerUpHandler.ActivateMagnet(duration);
            DOTween.Kill(transform);
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        DOTween.Kill(transform);
    }
}