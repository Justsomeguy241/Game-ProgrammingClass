using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    public GameObject gameOverUI;
    public Text currentWaveText;
    public Text highScore1Text;
    public Text highScore2Text;
    public Text highScore3Text;

    private void Start()
    {
        gameOverUI.SetActive(false);
    }

    public void ShowGameOver()
    {
        // Despawn all enemies and player
        CleanupScene();

        // Show Game Over UI
        gameOverUI.SetActive(true);
        Time.timeScale = 0f;

        int waveReached = FindFirstObjectByType<WaveManager>().currentWave;
        currentWaveText.text = $"You reached Wave {waveReached}";

        SaveHighScore(waveReached); 

        highScore1Text.text = $"1st: Wave {PlayerPrefs.GetInt("HighScore1", 0)}";
        highScore2Text.text = $"2nd: Wave {PlayerPrefs.GetInt("HighScore2", 0)}";
        highScore3Text.text = $"3rd: Wave {PlayerPrefs.GetInt("HighScore3", 0)}";
    }

    private void CleanupScene()
    {
        // Stop enemy spawning
        WaveManager waveManager = FindFirstObjectByType<WaveManager>();
        if (waveManager != null)
        {
            waveManager.StopSpawning();
        }

        // Destroy all enemies
        foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            Destroy(enemy);
        }

        // Destroy player
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            Destroy(player);
        }
        foreach (GameObject effect in GameObject.FindGameObjectsWithTag("Effect"))
        {
            Destroy(effect);
        }
        foreach (GameObject Bullet in GameObject.FindGameObjectsWithTag("PlayerBullet"))
        {
            Destroy(Bullet);
        }
    }

    public void Retry()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void QuitToMenu()
    {
        Time.timeScale = 1f;
        Application.Quit();
    }

    void SaveHighScore(int waveReached)
    {
        int score1 = PlayerPrefs.GetInt("HighScore1", 0);
        int score2 = PlayerPrefs.GetInt("HighScore2", 0);
        int score3 = PlayerPrefs.GetInt("HighScore3", 0);

        if (waveReached > score1)
        {
            PlayerPrefs.SetInt("HighScore3", score2);
            PlayerPrefs.SetInt("HighScore2", score1);
            PlayerPrefs.SetInt("HighScore1", waveReached);
        }
        else if (waveReached > score2)
        {
            PlayerPrefs.SetInt("HighScore3", score2);
            PlayerPrefs.SetInt("HighScore2", waveReached);
        }
        else if (waveReached > score3)
        {
            PlayerPrefs.SetInt("HighScore3", waveReached);
        }

        PlayerPrefs.Save(); // Save changes to disk
    }

}
