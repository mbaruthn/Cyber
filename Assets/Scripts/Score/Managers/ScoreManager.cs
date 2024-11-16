using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public Text scoreText; // UI Text where the score will be displayed
    private int score = 0;

    private ComboManager comboManager; // Reference to ComboManager

    private void Start()
    {
        comboManager = FindObjectOfType<ComboManager>();
        LoadScoreData(); // Automatically load saved score data
        UpdateScoreUI();
    }

    private void OnApplicationQuit()
    {
        SaveScoreData(); // Automatically save score data when the application quits
    }

    private void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus)
        {
            SaveScoreData(); // Automatically save score data when the application is paused
        }
    }

    // Increase the score
    public void IncreaseScore(int baseAmount)
    {
        int multiplier = comboManager != null ? comboManager.GetComboMultiplier() : 1; // Get combo multiplier
        int finalScore = baseAmount * multiplier; // Apply combo multiplier
        score += finalScore;
        UpdateScoreUI();
    }

    // Decrease the score
    public void DecreaseScore(int amount)
    {
        score -= amount;
        if (score < 0) score = 0; // Ensure score does not go negative
        UpdateScoreUI();

        if (comboManager != null)
        {
            comboManager.ResetCombo(); // Reset combo on wrong match
        }
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
        Debug.Log("Game Finished! Final Score: " + score);
    }

    // Get the current score (for saving)
    public int GetScore()
    {
        return score;
    }

    // Set the score (for loading)
    public void SetScore(int newScore)
    {
        score = newScore;
        UpdateScoreUI();
    }

    // Save the score data
    public void SaveScoreData()
    {
        SaveData saveData = SaveLoadManager.LoadGame() ?? new SaveData(); // Load existing data or create new
        saveData.score = score; // Update score in save data
        SaveLoadManager.SaveGame(saveData);
        Debug.Log("Score data saved.");
    }

    // Load the score data
    private void LoadScoreData()
    {
        SaveData saveData = SaveLoadManager.LoadGame();

        if (saveData != null)
        {
            score = saveData.score;
            Debug.Log("Score data loaded: " + score);
        }
        else
        {
            Debug.LogWarning("No score data found. Starting with 0 score.");
        }
    }
}
