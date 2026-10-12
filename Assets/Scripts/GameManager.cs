
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("UI Panels")]
    public GameObject gameOverPanel;
    public GameObject victoryPanel;

    private bool gameEnded = false;

    private void Start()
    {
        // reset game time when scene starts/ restarts
        Time.timeScale = 1f;

        // hide game over screen at start
        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);

        // hide victory screen at start
        if (victoryPanel != null)
            victoryPanel.SetActive(false);
    }

    private void Update()
    {
        // allow player to restart by pressing R after game ends
        if (gameEnded && Input.GetKeyDown(KeyCode.R))
        {
            RestartGame();
        }
    }

    public void GameOver()
    {
        // prevent game from ending multiple times
        if (gameEnded)
            return;

        gameEnded = true;

        // display game over screen
        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);

        // pause all gameplay
        Time.timeScale = 0f;
    }

    public void Victory()
    {
        // prevent game from ending multiple times
        if (gameEnded)
            return;

        gameEnded = true;

        // display victory screen
        if (victoryPanel != null)
            victoryPanel.SetActive(true);

        // pause all gameplay
        Time.timeScale = 0f;
    }

    public void RestartGame()
    {
        // resume game time before reloading curr scene
        Time.timeScale = 1f;

        // reload active scene to reset game
        SceneManager.LoadScene(
            SceneManager.GetActiveScene().buildIndex
        );
    }
}
