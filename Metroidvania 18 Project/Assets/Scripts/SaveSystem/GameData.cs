[System.Serializable]
public class GameData
{
    public DoorData[] DoorData;
    public PickUpData[] CollectedPickUps;
    public int ResourceWallet;
    public DrinkData[] DrinkInventory;
    public PlayerData PlayerData;

    public GameData(DoorData[] doorData, PickUpData[] collectedPickUps, int resourceWallet, DrinkData[] drinkInventory, PlayerData playerData)
    {
        this.DoorData = doorData;
        this.CollectedPickUps = collectedPickUps;
        this.ResourceWallet = resourceWallet;
        this.DrinkInventory = drinkInventory;
        this.PlayerData = playerData;
    }
}
