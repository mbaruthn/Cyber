using System;
using System.Collections.Generic;

[Serializable]
public class SaveData
{
    public int currentLevel; // Current level
    public List<int> cardLayout; // Card layout (IDs)
    public int score; // Current score
    public int totalCombos; // Total combos since game start
}
