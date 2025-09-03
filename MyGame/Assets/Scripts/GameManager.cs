using UnityEngine;
using System.Collections.Generic; // Needed for using Lists

public class GameManager : MonoBehaviour
{
    // --- Singleton Pattern ---
    public static GameManager Instance;

    [Header("Game Objects")]
    public GameObject packagePrefab;
    public GameObject dropOffPrefab;
    public List<Transform> spawnPoints;

    private GameObject currentPackage;
    private GameObject currentDropOffZone;

    // --- NEW VARIABLE ---
    // This will remember the index of the last spawn point we used.
    private int lastSpawnIndex = -1; // Start at -1 to guarantee the first pick is always valid.

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        // Make sure we have at least 2 spawn points for this logic to work.
        if (spawnPoints.Count < 2)
        {
            Debug.LogError("Not enough spawn points! Please add at least 2 spawn points to the GameManager.");
            return;
        }
        SpawnNewPackage();
    }

    public void OnPackagePickedUp()
    {
        Debug.Log("GameManager knows package was picked up.");

        if (currentDropOffZone != null)
        {
            Destroy(currentDropOffZone);
        }

        SpawnDropOffZone();
    }

    public void OnPackageDelivered()
    {
        Debug.Log("GameManager knows package was delivered.");

        // TODO: Add time to timer

        SpawnNewPackage();
    }

    // --- UPDATED LOGIC ---
    private void SpawnNewPackage()
    {
        int spawnIndex;
        // Keep picking a random index until we get one that is different from the last one.
        do
        {
            spawnIndex = Random.Range(0, spawnPoints.Count);
        }
        while (spawnIndex == lastSpawnIndex);

        // We have a new, unique index. Use it and then save it for the next check.
        lastSpawnIndex = spawnIndex;
        Transform spawnPoint = spawnPoints[spawnIndex];

        currentPackage = Instantiate(packagePrefab, spawnPoint.position, spawnPoint.rotation);
    }

    // --- UPDATED LOGIC ---
    private void SpawnDropOffZone()
    {
        int spawnIndex;
        // Keep picking a random index until we get one that is different from the last one.
        do
        {
            spawnIndex = Random.Range(0, spawnPoints.Count);
        }
        while (spawnIndex == lastSpawnIndex);

        // We have a new, unique index. Use it and then save it for the next check.
        lastSpawnIndex = spawnIndex;
        Transform spawnPoint = spawnPoints[spawnIndex];

        // We make the drop-off zone active before instantiating it
        dropOffPrefab.SetActive(true);
        currentDropOffZone = Instantiate(dropOffPrefab, spawnPoint.position, Quaternion.identity);
    }
}

