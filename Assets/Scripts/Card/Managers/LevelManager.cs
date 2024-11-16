using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // To manage UI elements

public class LevelManager : MonoBehaviour
{
    public CardManager cardManager; // Class to manage cards
    public int currentLevel = 1; // Starting level
    private int maxLevel = 6; // Maximum level count

    public Text endGameText; // UI Text to display end game message
    public float restartDelay = 3f; // Delay before restarting the game

    private void Start()
    {

        GameObject.Find("restartGame").GetComponent<Button>().onClick.AddListener(RestartGame);

        // Attempt to load game data
        LoadGame();

        // Start the current level (from loaded data or default)
        StartLevel(currentLevel);
    }

    // Start a specific level
    private void StartLevel(int level)
    {
        Vector2 gridSize = GetGridSize(level);
        int rows = (int)gridSize.x;
        int columns = (int)gridSize.y;

        cardManager.InitializeCards(rows, columns); // Place cards on the grid
        Debug.Log($"Level {level} started with grid {rows}x{columns}");
    }

    // Move to the next level
    public void NextLevel()
    {
        if (currentLevel < maxLevel)
        {
            currentLevel++;
            StartLevel(currentLevel);
            
            // Play level complete sound
            AudioManager.Instance.PlaySFX(AudioManager.Instance.levelCompleteClip);

            // Save progress
            SaveGame();
        }
        else
        {
            EndGame();
        }
    }

    private Vector2 GetGridSize(int level)
    {
        int rows, columns;

        // Increment the number of rows and columns based on the level
        switch (level)
        {
            case 1:
                rows = 2;
                columns = 2;
                break;
            case 2:
                rows = 2;
                columns = 3;
                break;
            case 3:
                rows = 3;
                columns = 4;
                break;
            case 4:
                rows = 4;
                columns = 4;
                break;
            case 5:
                rows = 4;
                columns = 5;
                break;
            case 6:
                rows = 5;
                columns = 6;
                break;
            case 7:
                rows = 6;
                columns = 6;
                break;
            case 8:
                rows = 6;
                columns = 7;
                break;
            case 9:
                rows = 7;
                columns = 8;
                break;
            case 10:
                rows = 8;
                columns = 8;
                break;
            default:
                rows = 2;
                columns = 2;
                break;
        }

        // Ensure the number of cards is even, adjusting rows and columns if necessary
        if ((rows * columns) % 2 != 0)
        {
            columns += 1; // Add one column to make the card count even
        }

        return new Vector2(rows, columns);
    }

    private void EndGame()
    {
        Debug.Log("Game Completed! Restarting...");

        // Show end game message
        if (endGameText != null)
        {
            endGameText.text = "Congratulations! Restarting...";
            endGameText.transform.parent.gameObject.SetActive(true);
        }

        // Reset score and save progress
        FindObjectOfType<ScoreManager>().SetScore(0);
        SaveLoadManager.SaveGame(new SaveData()); // Reset save data

        // Restart the game after a delay
        Invoke(nameof(RestartGame), restartDelay);
    }

    private void RestartGame()
    { 
        // Play restart sound
        AudioManager.Instance.PlaySFX(AudioManager.Instance.gameRestartClip);

        if (endGameText != null)
        {
            endGameText.transform.parent.gameObject.SetActive(false); // Hide the message
        }
        FindObjectOfType<ScoreManager>().ResetScore(); // Reset score
        FindObjectOfType<ComboManager>().ResetCombo(); // Reset combos
        currentLevel = 1;
        StartLevel(currentLevel);
        SaveGame();
    }

    public void SaveGame()
    {
        SaveData saveData = new SaveData
        {
            currentLevel = currentLevel, // Save the current level
            cardLayout = cardManager.GetCurrentCardLayout(), // Save card layout
            score = FindObjectOfType<ScoreManager>().GetScore(), // Save current score
            totalCombos = FindObjectOfType<ComboManager>().GetTotalCombos() // Save total combos
        };

        SaveLoadManager.SaveGame(saveData);
        Debug.Log("Game Saved!");
    }

    public void LoadGame()
    {
        if (endGameText != null)
        {
            endGameText.transform.parent.gameObject.SetActive(false); // Hide the message
        }

        SaveData loadedData = SaveLoadManager.LoadGame();

        if (loadedData != null)
        {
            currentLevel = loadedData.currentLevel; // Load the current level
            cardManager.SetCardLayout(loadedData.cardLayout); // Load card layout
            FindObjectOfType<ScoreManager>().SetScore(loadedData.score); // Load score
            FindObjectOfType<ComboManager>().SetTotalCombos(loadedData.totalCombos); // Load combos

            Debug.Log("Game Loaded!");
        }
        else
        {
            Debug.LogWarning("No save data found. Starting a new game.");
        }
    }
}
