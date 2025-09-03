using UnityEngine;
using System.Collections.Generic;
using TMPro; // --- NEW: Required for using TextMeshPro UI elements

public class GameManager : MonoBehaviour
{
    // --- Singleton Pattern ---
    public static GameManager Instance;

    [Header("Game Objects")]
    public GameObject packagePrefab;
    public GameObject dropOffPrefab;
    public List<Transform> spawnPoints;

    // --- NEW: UI and Timer Variables ---
    [Header("UI & Timer Settings")]
    public TextMeshProUGUI timerText; // Reference to the timer UI text
    public float startingTime = 60f; // Time in seconds
    public float timeBonusPerDelivery = 15f; // Time added for a successful delivery

    private float currentTime;
    private bool isGameOver = false;

    private GameObject currentPackage;
    private GameObject currentDropOffZone;
    private int lastSpawnIndex = -1;

    void Awake()
    {
        // ... existing Awake code ...
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
        // ... existing Start code ...
        if (spawnPoints.Count < 2)
        {
            Debug.LogError("Not enough spawn points! Please add at least 2 spawn points to the GameManager.");
            return;
        }

        // --- NEW: Initialize the timer ---
        currentTime = startingTime;
        isGameOver = false;

        SpawnNewPackage();
    }

    // --- NEW: Update function to handle the countdown ---
    void Update()
    {
        // If the game is over, do nothing.
        if (isGameOver) return;

        // Decrease the current time
        currentTime -= Time.deltaTime;

        // Check if time has run out
        if (currentTime <= 0)
        {
            currentTime = 0;
            isGameOver = true;
            Debug.Log("GAME OVER!");
            // TODO: Show a Game Over screen
        }

        // Update the UI text
        UpdateTimerUI();
    }

    public void OnPackagePickedUp()
    {
        // ... existing OnPackagePickedUp code ...
        Debug.Log("GameManager knows package was picked up.");
        if (currentDropOffZone != null) { Destroy(currentDropOffZone); }
        SpawnDropOffZone();
    }

    public void OnPackageDelivered()
    {
        Debug.Log("GameManager knows package was delivered.");

        // --- UPDATED: Add time bonus on delivery ---
        currentTime += timeBonusPerDelivery;

        SpawnNewPackage();
    }

    // --- NEW: Function to format and update the UI ---
    private void UpdateTimerUI()
    {
        if (timerText != null)
        {
            // Format the time into minutes and seconds for display
            int minutes = Mathf.FloorToInt(currentTime / 60);
            int seconds = Mathf.FloorToInt(currentTime % 60);
            timerText.text = string.Format("Time: {0:00}:{1:00}", minutes, seconds);
        }
    }

    // --- Spawn logic remains the same ---
    private void SpawnNewPackage()
    {
        // ... existing SpawnNewPackage code ...
        int spawnIndex;
        do { spawnIndex = Random.Range(0, spawnPoints.Count); } while (spawnIndex == lastSpawnIndex);
        lastSpawnIndex = spawnIndex;
        Transform spawnPoint = spawnPoints[spawnIndex];
        currentPackage = Instantiate(packagePrefab, spawnPoint.position, spawnPoint.rotation);
    }

    private void SpawnDropOffZone()
    {
        // ... existing SpawnDropOffZone code ...
        int spawnIndex;
        do { spawnIndex = Random.Range(0, spawnPoints.Count); } while (spawnIndex == lastSpawnIndex);
        lastSpawnIndex = spawnIndex;
        Transform spawnPoint = spawnPoints[spawnIndex];
        dropOffPrefab.SetActive(true);
        currentDropOffZone = Instantiate(dropOffPrefab, spawnPoint.position, Quaternion.identity);
    }
}

