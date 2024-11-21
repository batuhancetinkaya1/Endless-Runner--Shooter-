using UnityEngine;
using DG.Tweening;

public class Coin : MonoBehaviour
{
    [SerializeField] private AudioClip coinEffect;
    [SerializeField] private GameObject particleEffectPrefab;

    private void Start()
    {
        AnimateCoin();
        DOTween.SetTweensCapacity(2000, 100);
    }

    private void AnimateCoin()
    {
        // Rotation animation
        transform.DORotate(new Vector3(0, 360, 0), 1f, RotateMode.FastBeyond360)
            .SetEase(Ease.Linear)
            .SetLoops(-1, LoopType.Incremental);

        // Vertical bounce animation
        transform.DOMoveY(transform.position.y + 0.5f, 0.5f)
            .SetEase(Ease.OutSine)
            .SetLoops(-1, LoopType.Yoyo);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            CollectCoin();
        }
    }

    private void CollectCoin()
    {
        SoundManager.Instance.PlaySound(coinEffect, transform);
        ScoreManager.Instance.IncrementCoins(); // Changed from CollectCoin() to IncrementCoins()
        if (particleEffectPrefab != null)
        {
            Instantiate(particleEffectPrefab, transform.position, Quaternion.identity);
        }
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        // Stop all DOTween animations
        DOTween.Kill(transform);
    }
}