[System.Serializable]
public class GameData
{
    public DoorData[] DoorData;
    public ResourceData[] CollectedResources;
    public int ResourceWallet;
    public DrinkData[] DrinkInventory;

    public GameData(DoorData[] doorData, ResourceData[] collectedResources, int resourceWallet, DrinkData[] drinkInventory)
    {
        this.DoorData = doorData;
        this.CollectedResources = collectedResources;
        this.ResourceWallet = resourceWallet;
        this.DrinkInventory = drinkInventory;
    }
}
