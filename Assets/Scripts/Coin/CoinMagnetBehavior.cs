using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinMagnetBehavior : MonoBehaviour
{
    private Transform playerTransform;
    private PowerUpManager powerUpManager;
    private float baseSpeed = 10f; // Reduced from 15f
    private float accelerationRate = 5f; // New acceleration parameter
    private float currentSpeed;
    private bool isBeingAttracted = false;

    private void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        powerUpManager = FindObjectOfType<PowerUpManager>();
        currentSpeed = baseSpeed;
    }

    private void Update()
    {
        if (!isBeingAttracted && playerTransform != null)
        {
            PlayerPowerUpHandler powerUpHandler = playerTransform.GetComponent<PlayerPowerUpHandler>();
            if (powerUpHandler != null && powerUpHandler.IsMagnetActive())
            {
                float distance = Vector3.Distance(transform.position, playerTransform.position);
                if (distance <= powerUpManager.MagnetRange)
                {
                    isBeingAttracted = true;
                }
            }
        }

        if (isBeingAttracted && playerTransform != null)
        {
            // Gradually increase speed as coin gets closer to player
            float distance = Vector3.Distance(transform.position, playerTransform.position);
            currentSpeed = baseSpeed + (accelerationRate * (1 - distance / powerUpManager.MagnetRange));

            Vector3 direction = (playerTransform.position - transform.position).normalized;
            transform.position += direction * currentSpeed * Time.deltaTime;
        }
    }
}