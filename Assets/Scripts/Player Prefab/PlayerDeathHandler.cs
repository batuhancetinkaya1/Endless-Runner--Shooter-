using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeathHandler : MonoBehaviour
{
    [SerializeField] private Animator animator;

    public IEnumerator HandleDeath()
    {
        // Wait for death animation
        yield return new WaitForSecondsRealtime(2f);
        GameManager.Instance.GameOver();
    }
}
