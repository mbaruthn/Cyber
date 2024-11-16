using UnityEngine;
using UnityEngine.UI;

public class ComboManager : MonoBehaviour
{
    public Text comboText; // UI Text to display combo messages
    private int comboMultiplier = 1; // Combo multiplier starts at 1
    private float comboTimer = 0f;
    public float comboDuration = 2f; // Duration for the combo in seconds
    public bool isComboActive = false; // Tracks if combo is active
    private int totalCombos = 0; // Tracks total combos since game start

    // Initialize the combo system
    private void Start()
    {
        LoadComboData(); // Load saved combo data
        ResetCombo();
    }

    // Trigger combo activation
    public void ActivateCombo()
    {
        if (!isComboActive)
        {
            isComboActive = true;
            comboMultiplier = 0; // Start combo with multiplier 1
            comboTimer = comboDuration;
        }
    }

    // Increase the combo multiplier
    public void IncreaseCombo()
    {
        if (isComboActive)
        {
            comboMultiplier++;
            comboTimer = comboDuration; // Reset combo timer
            ShowComboMessage();
            totalCombos++; // Increment total combo count

            // Play combo sound
            AudioManager.Instance.PlaySFX(AudioManager.Instance.comboClip);
        }
    }

    // Reset the combo
    public void ResetCombo()
    {
        comboMultiplier = 1;
        comboTimer = 0f;
        isComboActive = false;
        HideComboMessage();
    }

    // Get the current combo multiplier
    public int GetComboMultiplier()
    {
        return isComboActive ? comboMultiplier : 1; // Return 1 if combo is not active
    }

    // Get total combos (for saving)
    public int GetTotalCombos()
    {
        return totalCombos;
    }

    // Set total combos (for loading)
    public void SetTotalCombos(int combos)
    {
        totalCombos = combos;
    }

    // Show combo message
    private void ShowComboMessage()
    {
        if (comboText != null)
        {
            comboText.text = "Combo x" + comboMultiplier;
            comboText.gameObject.SetActive(true);
            CancelInvoke("HideComboMessage");
            Invoke("HideComboMessage", 2f); // Hide message after 2 seconds
        }
    }

    // Hide combo message
    private void HideComboMessage()
    {
        if (comboText != null)
        {
            comboText.gameObject.SetActive(false);
        }
    }

    // Update combo system every frame
    private void Update()
    {
        if (isComboActive)
        {
            comboTimer -= Time.deltaTime;

            // If combo timer runs out, reset combo
            if (comboTimer <= 0f)
            {
                ResetCombo();
            }
        }
    }

    // Save combo data
    public void SaveComboData()
    {
        SaveData saveData = SaveLoadManager.LoadGame() ?? new SaveData(); // Load existing data or create new
        saveData.totalCombos = totalCombos; // Update total combos
        SaveLoadManager.SaveGame(saveData);
        Debug.Log("Combo data saved.");
    }

    // Load combo data
    private void LoadComboData()
    {
        SaveData saveData = SaveLoadManager.LoadGame();

        if (saveData != null)
        {
            totalCombos = saveData.totalCombos;
            Debug.Log("Combo data loaded: Total Combos = " + totalCombos);
        }
        else
        {
            Debug.LogWarning("No combo data found. Starting fresh.");
        }
    }
}
