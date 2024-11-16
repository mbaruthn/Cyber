public interface IComboManager
{
    void ActivateCombo();
    void IncreaseCombo();
    void ResetCombo();
    int GetComboMultiplier();
    int GetTotalCombos();
    void SetTotalCombos(int combos);
}