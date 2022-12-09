[System.Serializable]
public class PlayerData
{
    public int MaxHealth;
    public int CurrentHealth;
    public string[] UnlockedGunSettings;
    public bool DoubleJump;

    public PlayerData(int maxHealth, int currentHealth, string[] unlockedGunSettings, bool doubleJump)
    {
        this.MaxHealth = maxHealth;
        this.CurrentHealth = currentHealth;
        this.UnlockedGunSettings = unlockedGunSettings;
        this.DoubleJump = doubleJump;
    }
}
