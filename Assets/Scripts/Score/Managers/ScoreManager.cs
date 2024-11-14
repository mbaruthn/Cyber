using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public Text scoreText; // UI Text where the score will be displayed
    private int score = 0;

    // Initialize the score
    private void Start()
    {
        UpdateScoreUI();
    }

    // Increase the score
    public void IncreaseScore(int amount)
    {
        score += amount;
        UpdateScoreUI();
    }

    // Decrease the score
    public void DecreaseScore(int amount)
    {
        score -= amount;
        if (score < 0) score = 0; // Ensure score does not go negative
        UpdateScoreUI();
    }

    // Update the score UI
    private void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + score;
        }
    }

    // Display the final score when the game is complete
    public void ShowFinalScore()
    {
        // Here we could trigger a UI or animation to show that the game is finished
        Debug.Log("Game Finished! Final Score: " + score);
    }
}
