using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardManager : MonoBehaviour, ICardManager
{
    public GameObject cardPrefab; // Reference to the card prefab
    public Transform cardContainer; // Main container where cards will be placed

    private List<Card> cards = new List<Card>(); // List to store all cards
    private ScoreManager scoreManager;
    private ComboManager comboManager; // Reference to the ComboManager

    public GridLayout3D gridLayout; // Reference to the grid layout manager
    private LevelManager levelManager; // Reference to the LevelManager

    private List<Card> activeSelection = new List<Card>(); // Current pair being checked
    private HashSet<Card> matchedCards = new HashSet<Card>(); // Cards that are locked in matched state

    private Coroutine showCardsCoroutine; // Reference to the coroutine to manage its execution

    private void Start()
    {
        // Find and reference the ScoreManager, ComboManager, and LevelManager
        scoreManager = FindObjectOfType<ScoreManager>();
        comboManager = FindObjectOfType<ComboManager>();
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

        // Show all cards briefly before hiding them
        if (showCardsCoroutine != null)
        {
            StopCoroutine(showCardsCoroutine); // Stop any ongoing coroutine
        }
        showCardsCoroutine = StartCoroutine(ShowAllCardsTemporarily());
    }

    public IEnumerator ShowAllCardsTemporarily()
    {
        foreach (var card in cards)
        {
            card.FlipCard(); // Show all cards
        }

        // Play card reveal sound
        AudioManager.Instance.PlaySFX(AudioManager.Instance.cardRevealClip);

        yield return new WaitForSeconds(2f); // Wait for 2 seconds

        foreach (var card in cards)
        {
            if (!card.IsMatched)
            {
                card.FlipCard(); // Hide all unmatched cards
            }
        }

        showCardsCoroutine = null; // Reset the coroutine reference
    }

    public void ClearCards()
    {
        // Stop any ongoing coroutine to avoid issues
        if (showCardsCoroutine != null)
        {
            StopCoroutine(showCardsCoroutine);
            showCardsCoroutine = null;
        }

        // Remove each card from the scene and free up memory
        foreach (Card card in cards)
        {
            Destroy(card.gameObject);
        }

        // Clear the card list
        cards.Clear();
        activeSelection.Clear();
        matchedCards.Clear();
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
        if (selectedCard.IsMatched || matchedCards.Contains(selectedCard))
            return; // Skip matched cards

        if (activeSelection.Contains(selectedCard))
            return; // Skip already selected cards

        selectedCard.FlipCard(); // Flip the selected card
        activeSelection.Add(selectedCard); // Add to the current selection list

        // If two cards are selected, check for a match
        if (activeSelection.Count == 2)
        {
            StartCoroutine(CheckSelectedCards(new List<Card>(activeSelection))); // Pass a copy of the current selection
            activeSelection.Clear(); // Clear the active selection for new cards
        }
    }

    private IEnumerator CheckSelectedCards(List<Card> selection)
    {
        Card firstCard = selection[0];
        Card secondCard = selection[1];

        yield return new WaitForSeconds(0.3f); // Wait briefly to show both cards

        if (firstCard.CardID == secondCard.CardID)
        {
            // Match found
            firstCard.SetMatched();
            secondCard.SetMatched();

            // Play match success sound
            AudioManager.Instance.PlaySFX(AudioManager.Instance.matchSuccessClip);

            matchedCards.Add(firstCard); // Lock the matched cards
            matchedCards.Add(secondCard);

            if (!comboManager.isComboActive)
            {
                comboManager.ActivateCombo();
            }

            // Update score with combo multiplier
            if (comboManager != null && comboManager.isComboActive)
            {
                comboManager.IncreaseCombo(); // Increase combo multiplier
                int comboMultiplier = comboManager.GetComboMultiplier();
                scoreManager.IncreaseScore(10 * comboMultiplier); // Apply combo multiplier
            }
            else
            {
                scoreManager.IncreaseScore(10); // Default score increment
            }

            // Check if all cards are matched
            if (matchedCards.Count == cards.Count) // All cards are locked (matched)
            {
                scoreManager.ShowFinalScore(); // Show the final score
                levelManager.NextLevel(); // Move to the next level
            }
        }
        else
        {
            // No match found, flip the cards back
            yield return new WaitForSeconds(0.3f); // Allow for animation/interaction delay
            firstCard.FlipCard();
            secondCard.FlipCard();

            // Play missmatch sound
            AudioManager.Instance.PlaySFX(AudioManager.Instance.missmatchClip);

            if (comboManager != null)
            {
                comboManager.ResetCombo(); // Reset combo on wrong match
            }
        }
    }

    // Get the current card layout (for saving)
    public List<int> GetCurrentCardLayout()
    {
        List<int> layout = new List<int>();
        foreach (Card card in cards)
        {
            layout.Add(card.CardID);
        }
        return layout;
    }

    // Set the card layout (for loading)
    public void SetCardLayout(List<int> layout)
    {
        ClearCards(); // Clear existing cards

        foreach (int cardID in layout)
        {
            GameObject cardObj = Instantiate(cardPrefab, cardContainer);
            Card card = cardObj.GetComponent<Card>();
            card.SetCardID(cardID);
            card.OnCardSelected = OnCardSelected; // Bind the card selection event
            cards.Add(card);
        }

        int rows = Mathf.CeilToInt(Mathf.Sqrt(layout.Count));
        int columns = Mathf.CeilToInt((float)layout.Count / rows);

        gridLayout.ArrangeCards(cards.ConvertAll(c => c.gameObject), rows, columns);
    }
}
