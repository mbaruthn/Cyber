using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    public GameObject cardPrefab; // Kart prefab referansý
    public Transform cardContainer; // Kartlarýn yerleþtirileceði ana obje (container)

    private List<Card> cards = new List<Card>(); // Kartlarý depolayacaðýmýz liste
    private Card firstSelectedCard; // Ýlk seçilen kart
    private Card secondSelectedCard; // Ýkinci seçilen kart
    private ScoreManager scoreManager;

    public GridLayout3D gridLayout; // Grid düzenleyici referansý

    private void Start()
    {
        // ScoreManager'i bulup referans al
        scoreManager = FindObjectOfType<ScoreManager>();
    }

    // Kartlarý sahneye yerleþtirme
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
        cardIDs = ShuffleList(cardIDs); // Karýþtýrýlmýþ kart listesi

        for (int i = 0; i < totalCards; i++)
        {
            GameObject cardObj = Instantiate(cardPrefab, cardContainer);
            Card card = cardObj.GetComponent<Card>();
            card.SetCardID(cardIDs[i]);
            card.OnCardSelected = OnCardSelected; // Kartýn seçilme olayýný baðla
            cards.Add(card);
        }

        gridLayout.ArrangeCards(cards.ConvertAll(c => c.gameObject), rows, columns);
    }

    public void ClearCards()
    {
        // Kart listesindeki her kartý sahneden kaldýr ve bellekten temizle
        foreach (Card card in cards)
        {
            Destroy(card.gameObject);
        }

        // Kart listesi temizleniyor
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

    private void OnCardSelected(Card selectedCard)
    {
        if (firstSelectedCard == null)
        {
            firstSelectedCard = selectedCard;
            firstSelectedCard.FlipCard();
        }
        else if (secondSelectedCard == null && firstSelectedCard != selectedCard)
        {
            secondSelectedCard = selectedCard;
            secondSelectedCard.FlipCard();
            StartCoroutine(CheckMatch());
        }
    }

    private IEnumerator CheckMatch()
    {
        yield return new WaitForSeconds(1f);

        if (firstSelectedCard.CardID == secondSelectedCard.CardID)
        {
            firstSelectedCard.SetMatched();
            secondSelectedCard.SetMatched();
            scoreManager.IncreaseScore(10);
        }
        else
        {
            firstSelectedCard.FlipCard();
            secondSelectedCard.FlipCard();
            scoreManager.DecreaseScore(5);
        }

        firstSelectedCard = null;
        secondSelectedCard = null;

        // Tüm kartlar eþleþtiyse bir sonraki seviyeye geç
        if (cards.TrueForAll(card => card.IsMatched))
        {
            scoreManager.ShowFinalScore();
            FindObjectOfType<LevelManager>().NextLevel();
        }
    }
}
