using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public Text scoreText; // Skorun görüneceði UI Text
    private int score = 0;

    // Skoru baþlat
    private void Start()
    {
        UpdateScoreUI();
    }

    // Skoru arttýr
    public void IncreaseScore(int amount)
    {
        score += amount;
        UpdateScoreUI();
    }

    // Skoru azalt
    public void DecreaseScore(int amount)
    {
        score -= amount;
        if (score < 0) score = 0; // Negatif skor olmasýn
        UpdateScoreUI();
    }

    // Skor UI'ýný güncelle
    private void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + score;
        }
    }

    // Oyunun tamamlanma durumu
    public void ShowFinalScore()
    {
        // Oyunun tamamlandýðýný gösteren bir UI veya animasyon tetikleyebiliriz
        Debug.Log("Game Finished! Final Score: " + score);
    }
}
