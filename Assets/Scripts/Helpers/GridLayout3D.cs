using System.Collections.Generic;
using UnityEngine;

public class GridLayout3D : MonoBehaviour
{
    public RectTransform cardContainer; // Container for grid placement
    public int _rows = 5; // Number of rows in the grid
    public int _columns = 5; // Number of columns in the grid
    public float margin = 20f; // Fixed margin from all sides in pixels
    public Color gridColor = Color.green; // Color for the grid lines in debug

    public void ArrangeCards(List<GameObject> cards, int rows, int columns)
    {
        _rows = rows;
        _columns = columns;

        if (cardContainer == null)
        {
            Debug.LogError("CardContainer is not assigned!");
            return;
        }

        // Define padding between cells
        float cellPadding = 10f; // Adjust this value to set space between cards

        // Calculate available width and height for the grid within the container, considering the margins and padding
        float availableWidth = cardContainer.rect.width - (2 * margin) - ((columns - 1) * cellPadding);
        float availableHeight = cardContainer.rect.height - (2 * margin) - ((rows - 1) * cellPadding);

        // Calculate the cell size based on the available space and number of rows/columns
        float cellWidth = availableWidth / columns;
        float cellHeight = availableHeight / rows;
        float cellSize = Mathf.Min(cellWidth, cellHeight); // Keep cells square

        // Calculate the total grid width and height based on independent cell sizes and padding
        float gridWidth = cellWidth * columns + (columns - 1) * cellPadding;
        float gridHeight = cellHeight * rows + (rows - 1) * cellPadding;
       
        // Calculate the starting position to center the grid within the container
        Vector3 startPosition = new Vector3(
            (-gridWidth / 2 + cellWidth / 2), // Center horizontally based on grid width

            (gridHeight / 2 - cellHeight / 2) , // Center vertically based on grid height
            0
        );
        Debug.Log(gridHeight + " " + availableHeight);
       
        int index = 0;

        // Position cards in the grid
        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < columns; col++)
            {
                if (index >= cards.Count)
                    return;

                // Calculate the position of the card within the cell, centered from the middle, with padding
                Vector3 cellPosition = startPosition + new Vector3(

                    col * (cellWidth + cellPadding),

                    -row * (cellHeight + cellPadding),
                    0
                );

                // Place the card in the center of the cell
                cards[index].transform.localPosition = cellPosition;

                cards[index].transform.SetParent(cardContainer, false); // Ensure cards are children of cardContainer

                cards[index].transform.localScale = new Vector2(cellSize, cellSize);

                index++;
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (cardContainer == null ||_rows <= 0 || _columns <= 0)
            return;

        Gizmos.color = gridColor;

        // Calculate available width and height for the grid within the container, considering the margins
        float availableWidth = cardContainer.rect.width - (2 * margin);
        float availableHeight = cardContainer.rect.height - (2 * margin);

        // Calculate the cell size
        float cellWidth = availableWidth / _columns;
        float cellHeight = availableHeight / _rows;
        float cellSize = Mathf.Min(cellWidth, cellHeight); // Use the smaller dimension to keep it square

        // Calculate the starting position for drawing in world coordinates
        Vector3 startPosition = cardContainer.position + new Vector3(
            -availableWidth / 2 + cellSize / 2 + margin,
            availableHeight / 2 - cellSize / 2 - margin,
            0
        );

        // Draw the grid cells
        for (int row = 0; row < _rows; row++)
        {
            for (int col = 0; col < _columns; col++)
            {
                // Calculate the position of the cell in world coordinates
                Vector3 cellPosition = startPosition + new Vector3(
                    col * cellSize,
                    -row * cellSize,
                    0
                );

                // Draw a square (grid cell) in world coordinates
                Vector3 topLeft = cellPosition + new Vector3(-cellSize / 2, cellSize / 2, 0);
                Vector3 topRight = cellPosition + new Vector3(cellSize / 2, cellSize / 2, 0);
                Vector3 bottomLeft = cellPosition + new Vector3(-cellSize / 2, -cellSize / 2, 0);
                Vector3 bottomRight = cellPosition + new Vector3(cellSize / 2, -cellSize / 2, 0);

                Gizmos.DrawLine(topLeft, topRight);
                Gizmos.DrawLine(topRight, bottomRight);
                Gizmos.DrawLine(bottomRight, bottomLeft);
                Gizmos.DrawLine(bottomLeft, topLeft);
            }
        }
    }
}
