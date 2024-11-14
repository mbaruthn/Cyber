using System.Collections.Generic;
using UnityEngine;

public class GridLayout3D : MonoBehaviour
{
    public float spacingY = 1.5f; // Kartlar arasýndaki dikey boþluk
    public float spacingX = 1.5f; // Kartlar arasýndaki yatay boþluk
    public float layoutPercentage = 0.8f; // Ekranýn % kaçýnýn kullanýlmasý gerektiði (örneðin %80 için 0.8)

    // Kartlarý bir ýzgaraya dizmek için metot
    public void ArrangeCards(List<GameObject> cards, int rows, int columns)
    {
        // Kameranýn ekran boyutlarýný alýyoruz
        Camera mainCamera = Camera.main;
        if (mainCamera == null)
        {
            Debug.LogError("Main Camera not found!");
            return;
        }

        // Ekranýn ortografik boyutlarýný hesapla
        float screenHeight = 2f * mainCamera.orthographicSize * layoutPercentage; // Y ekseni boyunca ekran yüksekliðinin %80'i
        float screenWidth = screenHeight * mainCamera.aspect; // X ekseni boyunca ekran geniþliði, en-boy oranýna baðlý

        // Dizinin toplam geniþliði ve yüksekliði hesaplanýr
        float gridWidth = (columns - 1) * spacingX;
        float gridHeight = (rows - 1) * spacingY;

        // Eðer grid yüksekliði ekranýn %80'ini aþýyorsa, columns deðerini artýrarak daha fazla sütun ekleyin
        while (gridHeight > screenHeight)
        {
            columns++;
            gridWidth = (columns - 1) * spacingX; // Geniþliði yeni columns deðerine göre güncelle
            gridHeight = Mathf.Ceil((float)cards.Count / columns) * spacingY; // Yüksekliði yeniden hesapla
        }

        // Baþlangýç pozisyonunu belirle
        Vector3 startPosition = new Vector3(
            -screenWidth / 2 + (screenWidth - gridWidth) / 2, // X ekseninde %80'lik alandan ortalanmýþ baþlangýç noktasý
            screenHeight / 2 - (screenHeight - gridHeight) / 2, // Y ekseninde %80'lik alandan ortalanmýþ baþlangýç noktasý
            0
        );

        // Kart dizini
        int index = 0;

        // Kartlarý satýr ve sütunlara göre konumlandýr
        for (int row = 0; row < Mathf.CeilToInt((float)cards.Count / columns); row++)
        {
            for (int col = 0; col < columns; col++)
            {
                if (index >= cards.Count)
                    return;

                // Kartýn pozisyonunu hesapla, X ve Y eksenine göre ortalanmýþ olacak þekilde
                Vector3 position = startPosition + new Vector3(
                    col * spacingX,
                    -row * spacingY,
                    0);

                // Kartý pozisyonla
                cards[index].transform.position = position;

                // Bir sonraki karta geç
                index++;
            }
        }
    }
}
