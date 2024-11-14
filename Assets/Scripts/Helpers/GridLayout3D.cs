using System.Collections.Generic;
using UnityEngine;

public class GridLayout3D : MonoBehaviour
{
    public float spacingY = 1.5f; // Vertical spacing between cards
    public float spacingX = 1.5f; // Horizontal spacing between cards
    public float layoutPercentage = 0.8f; // Percentage of the screen to use (e.g., 0.8 for 80%)

    // Method to arrange cards in a grid
    public void ArrangeCards(List<GameObject> cards, int rows, int columns)
    {
        // Get the screen dimensions from the camera
        Camera mainCamera = Camera.main;
        if (mainCamera == null)
        {
            Debug.LogError("Main Camera not found!");
            return;
        }

        // Calculate the screen's orthographic dimensions
        float screenHeight = 2f * mainCamera.orthographicSize * layoutPercentage; // 80% of the screen height along the Y-axis
        float screenWidth = screenHeight * mainCamera.aspect; // Width of the screen along the X-axis based on the aspect ratio

        // Calculate the total grid width and height
        float gridWidth = (columns - 1) * spacingX;
        float gridHeight = (rows - 1) * spacingY;

        // If grid height exceeds 80% of screen height, add more columns to expand horizontally
        while (gridHeight > screenHeight)
        {
            columns++;
            gridWidth = (columns - 1) * spacingX; // Update grid width with new column count
            gridHeight = Mathf.Ceil((float)cards.Count / columns) * spacingY; // Recalculate grid height
        }

        // Determine the starting position of the grid
        Vector3 startPosition = new Vector3(
            -screenWidth / 2 + (screenWidth - gridWidth) / 2, // Centered start position along X-axis within 80% area
            screenHeight / 2 - (screenHeight - gridHeight) / 2, // Centered start position along Y-axis within 80% area
            0
        );

        // Card index
        int index = 0;

        // Position cards in rows and columns
        for (int row = 0; row < Mathf.CeilToInt((float)cards.Count / columns); row++)
        {
            for (int col = 0; col < columns; col++)
            {
                if (index >= cards.Count)
                    return;

                // Calculate the position of the card, centered on the X and Y axes
                Vector3 position = startPosition + new Vector3(
                    col * spacingX,
                    -row * spacingY,
                    0);

                // Set the card's position
                cards[index].transform.position = position;

                // Move to the next card
                index++;
            }
        }
    }
}
