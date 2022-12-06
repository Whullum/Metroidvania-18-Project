[System.Serializable]
public class DrinkData
{
    public string DrinkType;
    public int Amount;

    public DrinkData(string type, int amount)
    {
        this.DrinkType = type;
        this.Amount = amount;
    }
}
