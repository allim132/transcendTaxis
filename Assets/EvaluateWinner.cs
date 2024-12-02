using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class EvaluateWinner : MonoBehaviour
{
    // While in game
    public TMP_Text timerText;

    // Game Over
    public float gameDuration = 10f; // 7 minutes in seconds
    public TMP_Text finalScoreText;

    public ScoreManager scoreManager;

    private float currentTime;
    private int finalScore;

    public GameObject gameOverPanel;

    void Start()
    {
        ResetTimer();
        gameOverPanel.SetActive(false);
    }

    void Update()
    {
        if (currentTime > 0)
        {
            currentTime -= Time.deltaTime;
            UpdateTimerDisplay();

            if (currentTime <= 0)
            {
                EndGame();
            }
        }
    }

    void UpdateTimerDisplay()
    {
        int minutes = Mathf.FloorToInt(currentTime / 60);
        int seconds = Mathf.FloorToInt(currentTime % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    void EndGame()
    {
        // Get final score
        finalScore = scoreManager.getScore();

        // Update score
        finalScoreText.SetText("Final Score: " + finalScore);

        // Show game over panel
        gameOverPanel.SetActive(true);

        
        
    }

    public void ResetGame()
    {
        // Go to main menu
        SceneManager.LoadScene("mainMenuScreen");
    }

    void ResetTimer()
    {
        currentTime = gameDuration;
    }
}