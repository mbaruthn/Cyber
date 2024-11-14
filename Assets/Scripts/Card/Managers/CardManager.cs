using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    public GameObject cardPrefab; // Reference to the card prefab
    public Transform cardContainer; // Main container where cards will be placed

    private List<Card> cards = new List<Card>(); // List to store all cards
    private ScoreManager scoreManager;

    public GridLayout3D gridLayout; // Reference to the grid layout manager
    private LevelManager levelManager; // Reference to the LevelManager

    private void Start()
    {
        // Find and reference the ScoreManager and LevelManager
        scoreManager = FindObjectOfType<ScoreManager>();
        levelManager = FindObjectOfType<LevelManager>();
    }

    // Place cards on the scene
    public void InitializeCards(int rows, int columns)
    {
        int totalCards = rows * columns;

        ClearCards();

        List<int> cardIDs = new List<int>();
        for (int i = 0; i < totalCards / 2; i++)
        {
            cardIDs.Add(i);
            cardIDs.Add(i);
        }
        cardIDs = ShuffleList(cardIDs); // Shuffled list of card IDs

        for (int i = 0; i < totalCards; i++)
        {
            GameObject cardObj = Instantiate(cardPrefab, cardContainer);
            Card card = cardObj.GetComponent<Card>();
            card.SetCardID(cardIDs[i]);
            card.OnCardSelected = OnCardSelected; // Bind the card selection event
            cards.Add(card);
        }

        gridLayout.ArrangeCards(cards.ConvertAll(c => c.gameObject), rows, columns);
    }

    public void ClearCards()
    {
        // Remove each card from the scene and free up memory
        foreach (Card card in cards)
        {
            Destroy(card.gameObject);
        }

        // Clear the card list
        cards.Clear();
    }

    private List<int> ShuffleList(List<int> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int randomIndex = Random.Range(0, list.Count);
            int temp = list[i];
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
        return list;
    }

    // Manage selected cards
    private void OnCardSelected(Card selectedCard)
    {
        if (selectedCard.isFlipped || selectedCard.IsMatched)
            return; // Skip already flipped or matched cards

        selectedCard.FlipCard();

        // Check for other flipped cards with the same ID
        foreach (Card card in cards)
        {
            if (card != selectedCard && card.isFlipped && card.CardID == selectedCard.CardID)
            {
                // Match found
                selectedCard.SetMatched();
                card.SetMatched();
                scoreManager.IncreaseScore(10);

                // Check if all cards are matched
                if (cards.TrueForAll(c => c.IsMatched))
                {
                    scoreManager.ShowFinalScore(); // Show the final score
                    levelManager.NextLevel(); // Move to the next level
                }

                return; // Match processed
            }
        }
    }
}
