using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    public GameObject cardPrefab; // Kart prefab referansý
    public Transform cardContainer; // Kartlarýn yerleþtirileceði ana obje (container)

    private List<Card> cards = new List<Card>(); // Kartlarý depolayacaðýmýz liste
    private ScoreManager scoreManager;

    public GridLayout3D gridLayout; // Grid düzenleyici referansý
    private LevelManager levelManager; // LevelManager referansý

    private void Start()
    {
        // ScoreManager ve LevelManager'i bulup referans al
        scoreManager = FindObjectOfType<ScoreManager>();
        levelManager = FindObjectOfType<LevelManager>();
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

    // Seçilen kartlarý yönetme
    private void OnCardSelected(Card selectedCard)
    {
        if (selectedCard.isFlipped || selectedCard.IsMatched)
            return; // Zaten açýk veya eþleþmiþ kartlarý iþlememize gerek yok

        selectedCard.FlipCard();

        // Ayný ID'ye sahip açýk diðer kartlarý kontrol et
        foreach (Card card in cards)
        {
            if (card != selectedCard && card.isFlipped && card.CardID == selectedCard.CardID)
            {
                // Eþleþme saðlandý
                selectedCard.SetMatched();
                card.SetMatched();
                scoreManager.IncreaseScore(10);

                // Tüm kartlar eþleþti mi kontrol et
                if (cards.TrueForAll(c => c.IsMatched))
                {
                    scoreManager.ShowFinalScore(); // Son skoru göster
                    levelManager.NextLevel(); // Sonraki seviyeye geç
                }

                return; // Eþleþmeyi bulduk ve iþledik
            }
        }
    }
}
