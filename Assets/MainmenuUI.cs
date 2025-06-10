using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
    // Called by Start Button
    public void StartGame()
    {
        // Replace "GameScene" with your actual scene name
        SceneManager.LoadScene("SampleScene");
    }

    // Called by Quit Button
    public void QuitGame()
    {
        Debug.Log("Quit Game"); // Works in Editor
        Application.Quit();     // Works in build
    }
}
