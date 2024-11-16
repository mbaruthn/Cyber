public interface IScoreManager
{
    void IncreaseScore(int amount);
    void DecreaseScore(int amount);
    int GetScore();
    void SetScore(int score);
    void ShowFinalScore();
    void ResetScore();
}