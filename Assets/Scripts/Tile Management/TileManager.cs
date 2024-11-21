using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour
{
    [Header("References")]
    public GameObject[] tilePrefabs;
    public Transform playerTransform;
    public GameObject initialTile; // Reference to Tile_0 set in Inspector

    [Header("Settings")]
    [SerializeField] private int desiredTileCount = 10;
    [SerializeField] private float deletionDistance = 10f;
    [SerializeField] private float spawnDistance = 50f;

    private Queue<(GameObject tileObject, Tile tileScript)> activeTiles;
    private Transform lastTileEndPoint;
    private bool isInitialized = false;

    [SerializeField] private GameObject player;

    private void Start()
    {
        Initialize();
    }

    public void Initialize()
    {
        if (isInitialized)
        {
            return;
        }

        if (playerTransform == null)
        {
            playerTransform = player?.transform;
        }

        if (playerTransform == null || initialTile == null)
        {
            Debug.LogError("TileManager: Missing player or initial tile reference!");
            enabled = false;
            return;
        }

        if (activeTiles == null)
        {
            activeTiles = new Queue<(GameObject, Tile)>(desiredTileCount);
        }

        CleanupExistingTiles();
        SetupInitialTileEndpoint();
        SpawnInitialTiles();
        isInitialized = true;
    }

    private void CleanupExistingTiles()
    {
        if (activeTiles != null)
        {
            while (activeTiles.Count > 0)
            {
                var (tileObject, _) = activeTiles.Dequeue();
                if (tileObject != null)
                {
                    Destroy(tileObject);
                }
            }
        }
    }

    private void SetupInitialTileEndpoint()
    {
        Tile initialTileScript = initialTile.GetComponent<Tile>();
        if (initialTileScript != null && initialTileScript.endPoint != null)
        {
            lastTileEndPoint = initialTileScript.endPoint;
        }
        else
        {
            Debug.LogError("TileManager: Initial tile missing Tile component or endpoint!");
            enabled = false;
        }
    }

    private void SpawnInitialTiles()
    {
        while (activeTiles.Count < desiredTileCount)
        {
            SpawnTile();
        }
    }

    private void Update()
    {
        if (!isInitialized)
        {
            return;
        }

        ManageTiles();
    }

    private void ManageTiles()
    {
        DeletePassedTiles();

        if (ShouldSpawnNewTile())
        {
            SpawnTile();
        }
    }

    private void DeletePassedTiles()
    {
        while (activeTiles.Count > 0)
        {
            var (tileObject, tileScript) = activeTiles.Peek();

            if (tileScript == null || tileScript.endPoint == null)
            {
                activeTiles.Dequeue();
                Destroy(tileObject);
                continue;
            }

            if (playerTransform.position.z > tileScript.endPoint.position.z + deletionDistance)
            {
                activeTiles.Dequeue();
                Destroy(tileObject);
            }
            else
            {
                break;
            }
        }
    }

    private bool ShouldSpawnNewTile()
    {
        if (lastTileEndPoint == null)
        {
            return false;
        }

        float distanceToEnd = lastTileEndPoint.position.z - playerTransform.position.z;
        return distanceToEnd < spawnDistance && activeTiles.Count < desiredTileCount;
    }

    private void SpawnTile()
    {
        if (lastTileEndPoint == null)
        {
            return;
        }

        int prefabIndex = Random.Range(0, tilePrefabs.Length);
        GameObject newTileObject = Instantiate(tilePrefabs[prefabIndex], lastTileEndPoint.position, Quaternion.identity);
        Tile newTileScript = newTileObject.GetComponent<Tile>();

        if (newTileScript == null || newTileScript.endPoint == null || newTileScript.startPoint == null)
        {
            Destroy(newTileObject);
            return;
        }

        Vector3 offset = newTileScript.startPoint.position - newTileObject.transform.position;
        newTileObject.transform.position = lastTileEndPoint.position - offset;

        activeTiles.Enqueue((newTileObject, newTileScript));
        lastTileEndPoint = newTileScript.endPoint;
    }

    public void ResetTiles()
    {
        isInitialized = false;
        CleanupExistingTiles();
        Initialize();
    }
}
