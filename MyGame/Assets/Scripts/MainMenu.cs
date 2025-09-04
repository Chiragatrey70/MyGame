using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [Header("UI Panels")]
    public GameObject mainMenuPanel;
    public GameObject objectivePanel;

    void Start()
    {
        // On start, ensure the main menu is visible and the objective panel is not.
        mainMenuPanel.SetActive(true);
        objectivePanel.SetActive(false);
    }

    public void ShowObjectiveScreen()
    {
        // This function is called by the "Play" button.
        mainMenuPanel.SetActive(false);
        objectivePanel.SetActive(true);
    }

    public void StartGame()
    {
        // This function is called by the "Start" button on the objective screen.
        SceneManager.LoadScene("GameScene");
    }

    public void QuitGame()
    {
        // This function is called by the "Exit" button.
        Debug.Log("Quitting Game...");
        Application.Quit();
    }
}

