using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerPowerUpHandler : MonoBehaviour
{
    [Header("Power-Up Effects")]
    [SerializeField] private GameObject shieldParticleEffectPrefab;
    [SerializeField] private GameObject magnetBoostObject;

    [Header("Audio Clips")]
    [SerializeField] private AudioClip powerUpSound;
    [SerializeField] private AudioClip powerDownSound;

    private AudioSource audioSource;
    private GameObject shieldParticleInstance;
    private Coroutine magnetCoroutine;
    private Coroutine shieldCoroutine;
    private bool isMagnetActive;
    private bool isShieldActive;

    #region Shield Power-Up
    public void ActivateShield(float duration)
    {
        if (isShieldActive)
        {
            PlaySound(powerUpSound);
            ResetCoroutine(ref shieldCoroutine, ShieldDurationCoroutine(duration));
            return; // Avoid recreating the shield effect if it's already active.
        }

        // Instantiate the shield effect
        shieldParticleInstance = Instantiate(shieldParticleEffectPrefab, transform.position, Quaternion.identity, transform);
        PlaySound(powerUpSound);

        shieldCoroutine = StartCoroutine(ShieldDurationCoroutine(duration));
        isShieldActive = true;
    }

    private IEnumerator ShieldDurationCoroutine(float duration)
    {
        yield return new WaitForSeconds(duration);
        DeactivateShield();
    }

    public void DeactivateShield()
    {
        if (!isShieldActive) return;

        PlaySound(powerDownSound);

        if (shieldParticleInstance != null)
        {
            Destroy(shieldParticleInstance);
        }

        isShieldActive = false;
    }
    #endregion

    #region Magnet Power-Up
    public void ActivateMagnet(float duration)
    {
        ResetCoroutine(ref magnetCoroutine, MagnetRoutine(duration));
    }

    private IEnumerator MagnetRoutine(float duration)
    {
        isMagnetActive = true;
        magnetBoostObject?.SetActive(true);

        PlaySound(powerUpSound);
        yield return new WaitForSeconds(duration);

        DeactivateMagnet();
    }

    private void DeactivateMagnet()
    {
        isMagnetActive = false;
        magnetBoostObject?.SetActive(false);
        PlaySound(powerDownSound);
    }
    #endregion

    private void ResetCoroutine(ref Coroutine coroutine, IEnumerator routine)
    {
        if (coroutine != null) StopCoroutine(coroutine);
        coroutine = StartCoroutine(routine);
    }

    public void ResetAllPowerUps()
    {
        StopAllCoroutines();

        if (isShieldActive) DeactivateShield();
        if (isMagnetActive) DeactivateMagnet();
    }

    private void PlaySound(AudioClip clip)
    {
        if (clip != null)
        {
            SoundManager.Instance.PlaySound(clip, transform);
        }
    }

    public bool IsMagnetActive() => isMagnetActive;
    public bool IsShieldActive() => isShieldActive;
}
