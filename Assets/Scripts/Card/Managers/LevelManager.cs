using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public CardManager cardManager; // Kartlarý yöneten sýnýf
    public int currentLevel = 1; // Baþlangýç seviyesi
    private int maxLevel = 10; // Maksimum seviye sayýsý

    private void Start()
    {
        StartLevel(currentLevel); // Ýlk seviyeyi baþlat
    }

    // Seviyeyi baþlatma
    private void StartLevel(int level)
    {
        Vector2 gridSize = GetGridSize(level);
        int rows = (int)gridSize.x;
        int columns = (int)gridSize.y;

        cardManager.InitializeCards(rows, columns); // Kartlarý yerleþtir
        Debug.Log($"Level {level} started with grid {rows}x{columns}");
    }

    // Bir sonraki seviyeye geçiþ yap
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

        // Belirli seviyelere göre satýr ve sütun sayýsýný düzenli artýr
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

        // Kart sayýsýnýn çift olmasý gerektiðinden, satýr ve sütun sayýsýný kontrol et
        if ((rows * columns) % 2 != 0)
        {
            columns += 1; // Kart sayýsýný çift yapmak için bir sütun ekliyoruz
        }

        return new Vector2(rows, columns);
    }
}
