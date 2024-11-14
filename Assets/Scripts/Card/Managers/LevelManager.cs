using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public CardManager cardManager; // Class to manage cards
    public int currentLevel = 1; // Starting level
    private int maxLevel = 10; // Maximum level count

    private void Start()
    {
        StartLevel(currentLevel); // Start the first level
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
        }
        else
        {
            Debug.Log("Congratulations! You've completed all levels.");
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
}
