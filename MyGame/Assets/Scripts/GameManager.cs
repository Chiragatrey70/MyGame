using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections.Generic;
using System.Linq;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("UI Elements")]
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI deliveriesCounterText;
    public GameObject inGameHUD;
    public GameObject touchControlsUI; // New field for touch controls
    public GameObject gameOverPanel;
    public GameObject pauseMenuPanel;

    [Header("Game Settings")]
    public float startingTime = 60f;
    public float timeBonus = 15f;
    private float currentTime;
    private int deliveriesCompleted = 0;

    [Header("Game State")]
    private bool isPaused = false;
    private bool isGameOver = false;

    [Header("Gameplay Objects")]
    public GameObject packagePrefab;
    public GameObject dropOffPrefab;
    public List<Transform> spawnPoints;
    private GameObject currentPackage;
    private GameObject currentDropOffZone;
    private int lastSpawnIndex = -1;

    [Header("Player References")]
    public CarController playerCarController;

    [Header("Audio")]
    public CarAudio playerCarAudio;

    [Header("Minimap")]
    public MinimapIcon objectiveIcon;
    public Transform playerTransform;
    public Camera minimapCamera;
    public RectTransform minimapRect;
    private Transform currentObjectiveTransform;

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
        Time.timeScale = 1f;
        currentTime = startingTime;
        isGameOver = false;
        isPaused = false;

        inGameHUD.SetActive(true);
        touchControlsUI.SetActive(true); // Show touch controls at start
        gameOverPanel.SetActive(false);
        pauseMenuPanel.SetActive(false);

        UpdateDeliveriesUI();
        SpawnNewPackage();
    }

    void Update()
    {
        if (isGameOver || isPaused) return;

        currentTime -= Time.deltaTime;
        UpdateTimerUI();

        if (currentTime <= 0)
        {
            currentTime = 0;
            GameOver();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
                ResumeGame();
            else
                PauseGame();
        }

        UpdateObjectiveIcon();
    }

    void SpawnNewPackage()
    {
        if (currentPackage != null) Destroy(currentPackage);

        int spawnIndex;
        do
        {
            spawnIndex = Random.Range(0, spawnPoints.Count);
        } while (spawnPoints.Count > 1 && spawnIndex == lastSpawnIndex);

        lastSpawnIndex = spawnIndex;
        Transform spawnPoint = spawnPoints[spawnIndex];
        currentPackage = Instantiate(packagePrefab, spawnPoint.position, spawnPoint.rotation);
        SetObjectiveIconTarget(currentPackage.transform);
    }

    void SpawnDropOffZone()
    {
        if (currentDropOffZone != null) Destroy(currentDropOffZone);

        int spawnIndex;
        do
        {
            spawnIndex = Random.Range(0, spawnPoints.Count);
        } while (spawnPoints.Count > 1 && spawnIndex == lastSpawnIndex);

        lastSpawnIndex = spawnIndex;
        Transform spawnPoint = spawnPoints[spawnIndex];
        currentDropOffZone = Instantiate(dropOffPrefab, spawnPoint.position, spawnPoint.rotation);
        SetObjectiveIconTarget(currentDropOffZone.transform);
    }

    public void OnPackagePickedUp()
    {
        if (isGameOver || isPaused) return;
        if (currentPackage == null) return;

        Destroy(currentPackage);
        currentPackage = null;
        SpawnDropOffZone();
    }

    public void OnPackageDelivered()
    {
        if (isGameOver || isPaused) return;
        if (currentDropOffZone == null) return;

        Destroy(currentDropOffZone);
        currentDropOffZone = null;
        currentTime += timeBonus;
        deliveriesCompleted++;
        UpdateDeliveriesUI();
        SpawnNewPackage();
    }

    void UpdateTimerUI()
    {
        if (timerText != null)
        {
            timerText.text = "Time: " + Mathf.RoundToInt(currentTime);
        }
    }

    void UpdateDeliveriesUI()
    {
        if (deliveriesCounterText != null)
        {
            deliveriesCounterText.text = "Deliveries: " + deliveriesCompleted;
        }
    }

    public void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0f;
        pauseMenuPanel.SetActive(true);
        inGameHUD.SetActive(false);
        touchControlsUI.SetActive(false); // Hide touch controls
        playerCarController.enabled = false;
        if (playerCarAudio != null) playerCarAudio.PauseSound();
    }

    public void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1f;
        pauseMenuPanel.SetActive(false);
        inGameHUD.SetActive(true);
        touchControlsUI.SetActive(true); // Show touch controls
        playerCarController.enabled = true;
        if (playerCarAudio != null) playerCarAudio.ResumeSound();
    }

    void GameOver()
    {
        isGameOver = true;
        Time.timeScale = 0f;
        gameOverPanel.SetActive(true);
        inGameHUD.SetActive(false);
        touchControlsUI.SetActive(false); // Hide touch controls
        playerCarController.enabled = false;
        if (playerCarAudio != null) playerCarAudio.StopSound();
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ReturnToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenuScene");
    }

    void SetObjectiveIconTarget(Transform target)
    {
        currentObjectiveTransform = target;
        if (objectiveIcon != null)
        {
            objectiveIcon.gameObject.SetActive(true);
        }
    }

    void UpdateObjectiveIcon()
    {
        if (objectiveIcon == null || !objectiveIcon.gameObject.activeInHierarchy) return;

        Vector3 screenPos = minimapCamera.WorldToScreenPoint(currentObjectiveTransform.position);

        RectTransform minimapImageRect = minimapRect;
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(minimapImageRect, screenPos, null, out localPoint);

        objectiveIcon.GetComponent<RectTransform>().localPosition = localPoint;
    }
}

