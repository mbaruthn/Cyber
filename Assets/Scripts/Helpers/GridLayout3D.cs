using System.Collections.Generic;
using UnityEngine;

public class GridLayout3D : MonoBehaviour
{
    public float spacingY = 1.5f; // Kartlar arasýndaki dikey boþluk
    public float spacingX = 1.5f; // Kartlar arasýndaki yatay boþluk

    // Kartlarý bir ýzgaraya dizmek için metot
    public void ArrangeCards(List<GameObject> cards, int rows, int columns)
    {
        // Dizinin toplam geniþliði ve yüksekliði hesaplanýr
        float gridWidth = (columns - 1) * spacingX;
        float gridHeight = (rows - 1) * spacingY;

        // Kart dizini
        int index = 0;

        // Kartlarý satýr ve sütunlara göre konumlandýr
        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < columns; col++)
            {
                if (index >= cards.Count)
                    return;

                // Kartýn pozisyonunu hesapla, ortalanmýþ konumda olacak þekilde
                Vector3 position = new Vector3(
                    col * spacingX - gridWidth / 2,  // X ekseni boyunca ortalanmýþ konum
                    -(row * spacingY - gridHeight / 2),  // Y ekseni boyunca ortalanmýþ konum
                    0);  // Z ekseni sabit

                // Kartý pozisyonla
                cards[index].transform.position = position;

                // Bir sonraki karta geç
                index++;
            }
        }
    }
}
