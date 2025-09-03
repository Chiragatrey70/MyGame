using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // This function will be called by the Play Button
    public void PlayGame()
    {
        // Make sure your main game scene is named "GameScene"
        SceneManager.LoadScene("GameScene");
    }

    // This function will be called by the Exit Button
    public void QuitGame()
    {
        Debug.Log("QUIT!"); // This shows up in the editor
        Application.Quit(); // This only works in a built game
    }
}
