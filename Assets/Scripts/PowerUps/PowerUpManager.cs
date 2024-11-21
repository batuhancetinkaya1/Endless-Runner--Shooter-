using System.Collections.Generic;
using UnityEngine;

public class PowerUpManager : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject fireRateBoostPrefab;
    [SerializeField] private GameObject shieldBoostPrefab;
    [SerializeField] private GameObject magnetBoostPrefab;
    [SerializeField] private float magnetDuration = 10f;
    [SerializeField] private float shieldDuration = 10f;
    [SerializeField] private float magnetRange = 10f;
    [SerializeField] private float minimumBoostDistance = 30f;
    [SerializeField] private float minimumDistanceFromObstacles = 5f;
    [SerializeField] private AudioClip fireRateBoostSpawnedSound;
    [SerializeField] private PlayerDistanceTracker playerDistanceTracker;
    [SerializeField] private PlayerPowerUpHandler powerUpHandler;

    private float[] lines = { -4.5f, -1.5f, 1.5f };
    private float nextFireRateSpawn;
    private float nextRandomBoostSpawn = 50f;
    private GameObject lastCreatedBoost;
    private float lastBoostLocationZ;

    private int coinLayerMask;
    private int preventerLayerMask;
    private int blockLayerMask;
    private int buildingLayerMask;

    public float MagnetRange => magnetRange;
    public float MagnetDuration => magnetDuration;
    public float ShieldDuration => shieldDuration;

    private Dictionary<BoostType, float> lastBoostSpawnPositions = new Dictionary<BoostType, float>();

    private void Awake()
    {
        // Initialize layer masks in Awake to avoid repeated GetMask calls
        coinLayerMask = LayerMask.GetMask("Coin");
        preventerLayerMask = LayerMask.GetMask("NoSpawn");
        blockLayerMask = LayerMask.GetMask("Block");
        buildingLayerMask = LayerMask.GetMask("Building");
    }

    private void Start()
    {
        foreach (BoostType boostType in System.Enum.GetValues(typeof(BoostType)))
        {
            lastBoostSpawnPositions[boostType] = 0f;
        }

        UpdateNextFireRateSpawn();
    }

    private void Update()
    {
        if (GameManager.Instance.CurrentState != GameManager.GameState.Game) return;

        float currentDistance = playerDistanceTracker.GetCurrentDistance();

        if (currentDistance >= nextFireRateSpawn)
        {
            SpawnBoost(BoostType.FireRate);
            UpdateNextFireRateSpawn();
        }

        if (currentDistance >= nextRandomBoostSpawn)
        {
            SpawnRandomBoost();
            nextRandomBoostSpawn = Mathf.Floor(currentDistance / 50f) * 50f + 50f;
        }

        DestroyOldBoosts();
    }

    private void UpdateNextFireRateSpawn()
    {
        float currentDistance = playerDistanceTracker.GetCurrentDistance();
        float checkDistance = playerDistanceTracker.GetCheckDistance();

        nextFireRateSpawn = Mathf.Floor(currentDistance / checkDistance) * checkDistance + checkDistance;
    }

    private bool IsPositionClear(Vector3 position)
    {
        if (Physics.OverlapSphere(position, minimumDistanceFromObstacles, coinLayerMask).Length > 0) return false;
        if (Physics.OverlapSphere(position, minimumDistanceFromObstacles, preventerLayerMask).Length > 0) return false;
        if (Physics.OverlapSphere(position, minimumDistanceFromObstacles, blockLayerMask).Length > 0) return false;
        if (Physics.OverlapSphere(position, minimumDistanceFromObstacles, buildingLayerMask).Length > 0) return false;

        return true;
    }

    private Vector3 GetValidSpawnPosition(BoostType boostType)
    {
        int maxAttempts = 10;
        int attempts = 0;
        float playerZ = player.transform.position.z;

        while (attempts < maxAttempts)
        {
            foreach (float line in lines)
            {
                for (float zOffset = 30f; zOffset <= 60f; zOffset += 5f)
                {
                    Vector3 potentialPosition = new Vector3(line, -1f, playerZ + zOffset);

                    if (IsPositionClear(potentialPosition) && IsSpawnPositionValid(potentialPosition, boostType))
                    {
                        return potentialPosition;
                    }
                }
            }
            attempts++;
        }

        return new Vector3(lines[Random.Range(0, 3)], -1f, playerZ + 60f);
    }

    private bool IsSpawnPositionValid(Vector3 newPosition, BoostType boostType)
    {
        float distanceFromPlayer = Mathf.Abs(newPosition.z - player.transform.position.z);
        if (distanceFromPlayer < minimumBoostDistance) return false;

        if (lastBoostSpawnPositions.ContainsKey(boostType))
        {
            float distanceFromLastSpawn = Mathf.Abs(newPosition.z - lastBoostSpawnPositions[boostType]);
            if (distanceFromLastSpawn < minimumBoostDistance * 2) return false;
        }

        GameObject[] existingBoosts = GameObject.FindGameObjectsWithTag("PowerUp");
        foreach (GameObject boost in existingBoosts)
        {
            float distanceFromBoost = Vector3.Distance(newPosition, boost.transform.position);
            if (distanceFromBoost < minimumBoostDistance) return false;
        }

        return true;
    }

    public void ResetBoosts()
    {
        foreach (var boost in GameObject.FindGameObjectsWithTag("PowerUp"))
        {
            Destroy(boost);
        }

        float currentDistance = playerDistanceTracker.GetCurrentDistance();

        UpdateNextFireRateSpawn();
        nextRandomBoostSpawn = Mathf.Ceil(currentDistance / 50f) * 50f;

        foreach (BoostType boostType in System.Enum.GetValues(typeof(BoostType)))
        {
            lastBoostSpawnPositions[boostType] = currentDistance;
        }

        if (powerUpHandler != null)
        {
            powerUpHandler.ResetAllPowerUps();
        }
    }

    private void SpawnBoost(BoostType boostType)
    {
        Vector3 spawnPosition = GetValidSpawnPosition(boostType);
        lastBoostSpawnPositions[boostType] = spawnPosition.z;

        GameObject prefabToSpawn = null;
        switch (boostType)
        {
            case BoostType.FireRate:
                prefabToSpawn = fireRateBoostPrefab;
                SoundManager.Instance.PlaySound(fireRateBoostSpawnedSound, transform);
                break;
            case BoostType.Shield:
                prefabToSpawn = shieldBoostPrefab;
                break;
            case BoostType.Magnet:
                prefabToSpawn = magnetBoostPrefab;
                break;
        }

        if (prefabToSpawn != null)
        {
            GameObject boost = Instantiate(prefabToSpawn, spawnPosition, Quaternion.identity);

            // Initialize boost with required references
            switch (boostType)
            {
                case BoostType.FireRate:
                    boost.GetComponent<FireRateBoostScript>().Initialize(powerUpHandler.GetComponent<PlayerShooter>());
                    break;
                case BoostType.Shield:
                    boost.GetComponent<ShieldScript>().Initialize(powerUpHandler);
                    break;
                case BoostType.Magnet:
                    boost.GetComponent<MagnetScript>().Initialize(powerUpHandler, magnetDuration);
                    break;
            }

            lastCreatedBoost = boost;
            lastBoostLocationZ = spawnPosition.z;
        }
    }

    private void SpawnRandomBoost()
    {
        BoostType randomBoost = Random.value < 0.5f ? BoostType.Shield : BoostType.Magnet;
        SpawnBoost(randomBoost);
    }

    private void DestroyOldBoosts()
    {
        foreach (var boost in GameObject.FindGameObjectsWithTag("PowerUp"))
        {
            if (boost.transform.position.z < player.transform.position.z - 10f)
            {
                Destroy(boost);
            }
        }
    }

    private enum BoostType
    {
        FireRate,
        Shield,
        Magnet
    }
}
