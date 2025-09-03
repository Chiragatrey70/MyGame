using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Game Objects")]
    public GameObject packagePrefab;
    public GameObject dropOffPrefab;
    public List<Transform> spawnPoints;

    [Header("UI Elements")]
    public TextMeshProUGUI timerText;
    public GameObject gameOverPanel; // For later use

    [Header("Game Settings")]
    public float startingTime = 60f;
    public float timeBonus = 15f;
    private float currentTime;

    [Header("Game State")]
    public bool isGameOver = false;
    private GameObject currentPackage;
    private GameObject currentDropOff;
    private int lastSpawnIndex = -1;

    [Header("Minimap")]
    public MinimapIcon objectiveIcon;
    public Transform playerTransform;
    public Camera minimapCamera;
    public RectTransform minimapRect;


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
        currentTime = startingTime;
        if (gameOverPanel != null) gameOverPanel.SetActive(false);
        isGameOver = false;

        // Set up the minimap icon's references
        if (objectiveIcon != null)
        {
            objectiveIcon.player = playerTransform;
            objectiveIcon.minimapCamera = minimapCamera;
            objectiveIcon.mapRect = minimapRect;
            objectiveIcon.gameObject.SetActive(false); // Start with it hidden
        }

        SpawnNewPackage();
    }

    void Update()
    {
        if (isGameOver) return;

        currentTime -= Time.deltaTime;
        if (currentTime <= 0)
        {
            currentTime = 0;
            GameOver();
        }
        UpdateTimerUI();
    }

    void SpawnNewPackage()
    {
        if (objectiveIcon != null) objectiveIcon.gameObject.SetActive(false);

        int spawnIndex;
        do
        {
            spawnIndex = Random.Range(0, spawnPoints.Count);
        } while (spawnPoints.Count > 1 && spawnIndex == lastSpawnIndex);
        lastSpawnIndex = spawnIndex;

        currentPackage = Instantiate(packagePrefab, spawnPoints[spawnIndex].position, spawnPoints[spawnIndex].rotation);

        // Tell icon to track the new package
        if (objectiveIcon != null)
        {
            objectiveIcon.SetTarget(currentPackage.transform);
            objectiveIcon.gameObject.SetActive(true);
        }
    }

    void SpawnDropOffZone()
    {
        int spawnIndex;
        do
        {
            spawnIndex = Random.Range(0, spawnPoints.Count);
        } while (spawnPoints.Count > 1 && spawnIndex == lastSpawnIndex);
        lastSpawnIndex = spawnIndex;

        currentDropOff = Instantiate(dropOffPrefab, spawnPoints[spawnIndex].position, spawnPoints[spawnIndex].rotation);

        // This line was missing in some versions, ensure it's here
        if (currentDropOff != null) currentDropOff.SetActive(true);

        // Tell icon to track the new drop-off zone
        if (objectiveIcon != null && currentDropOff != null)
        {
            objectiveIcon.SetTarget(currentDropOff.transform);
            objectiveIcon.gameObject.SetActive(true);
        }
    }

    public void OnPackagePickedUp()
    {
        if (currentPackage != null)
        {
            Destroy(currentPackage);
            currentPackage = null;
            SpawnDropOffZone();
        }
    }

    public void OnPackageDelivered()
    {
        if (currentDropOff != null)
        {
            Destroy(currentDropOff);
            currentDropOff = null;
            currentTime += timeBonus;
            SpawnNewPackage();
        }
    }

    void UpdateTimerUI()
    {
        if (timerText != null)
        {
            timerText.text = "Time: " + Mathf.RoundToInt(currentTime);
        }
    }

    void GameOver()
    {
        isGameOver = true;
        Debug.Log("GAME OVER!");
        // We will add the UI panel logic here later
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}

