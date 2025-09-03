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
    public CarController playerCarController;

    [Header("UI Elements")]
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI deliveriesCounterText; // NEW: Reference for the counter text
    public GameObject gameOverPanel;
    public GameObject pauseMenuPanel;

    [Header("Game Settings")]
    public float startingTime = 60f;
    public float timeBonus = 15f;
    private float currentTime;

    [Header("Game State")]
    public bool isGameOver = false;
    public bool isPaused = false;
    private int deliveriesCompleted = 0; // NEW: Variable to track deliveries
    private GameObject currentPackage;
    private GameObject currentDropOff;
    private int lastSpawnIndex = -1;

    [Header("Minimap")]
    public MinimapIcon objectiveIcon;
    public Transform playerTransform;
    public Camera minimapCamera;
    public RectTransform minimapRect;

    [Header("Audio")]
    public CarAudio playerCarAudio;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        currentTime = startingTime;
        gameOverPanel.SetActive(false);
        pauseMenuPanel.SetActive(false);
        isGameOver = false;

        deliveriesCompleted = 0; // NEW: Initialize the counter at the start
        UpdateDeliveriesUI();    // NEW: Update the UI text at the start

        if (playerCarController != null) playerCarController.enabled = true;

        if (objectiveIcon != null)
        {
            objectiveIcon.player = playerTransform;
            objectiveIcon.minimapCamera = minimapCamera;
            objectiveIcon.mapRect = minimapRect;
            objectiveIcon.gameObject.SetActive(false);
        }

        SpawnNewPackage();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isGameOver) return;
            if (isPaused) ResumeGame();
            else PauseGame();
        }

        if (isGameOver || isPaused) return;

        currentTime -= Time.deltaTime;
        if (currentTime <= 0)
        {
            currentTime = 0;
            GameOver();
        }
        UpdateTimerUI();
    }

    public void OnPackageDelivered()
    {
        if (isGameOver || isPaused) return;
        if (currentDropOff != null)
        {
            Destroy(currentDropOff);
            currentDropOff = null;
            currentTime += timeBonus;

            deliveriesCompleted++;      // NEW: Increment the counter on delivery
            UpdateDeliveriesUI();       // NEW: Update the UI text

            SpawnNewPackage();
        }
    }

    // NEW: Function to update the deliveries counter text
    void UpdateDeliveriesUI()
    {
        if (deliveriesCounterText != null)
        {
            deliveriesCounterText.text = "Deliveries: " + deliveriesCompleted;
        }
    }

    // --- Other existing functions (PauseGame, ResumeGame, etc.) ---
    public void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0f;
        pauseMenuPanel.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        if (playerCarAudio != null) playerCarAudio.PauseSound();
    }

    public void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1f;
        pauseMenuPanel.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        if (playerCarAudio != null) playerCarAudio.ResumeSound();
    }

    public void ReturnToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenuScene");
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
        if (currentDropOff != null) currentDropOff.SetActive(true);

        if (objectiveIcon != null && currentDropOff != null)
        {
            objectiveIcon.SetTarget(currentDropOff.transform);
            objectiveIcon.gameObject.SetActive(true);
        }
    }

    public void OnPackagePickedUp()
    {
        if (isGameOver || isPaused) return;
        if (currentPackage != null)
        {
            Destroy(currentPackage);
            currentPackage = null;
            SpawnDropOffZone();
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
        if (isGameOver) return;
        isGameOver = true;
        Time.timeScale = 0f;
        gameOverPanel.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        if (playerCarController != null) playerCarController.enabled = false;
        if (playerCarAudio != null) playerCarAudio.StopSound();
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}

