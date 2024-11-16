using System.Collections.Generic;

public interface ICardManager
{
    void InitializeCards(int rows, int columns); // Initializes the cards for a given grid size
    void ClearCards(); // Clears all cards from the scene
    List<int> GetCurrentCardLayout(); // Returns the current card layout
    void SetCardLayout(List<int> layout); // Sets the card layout for the scene
}
