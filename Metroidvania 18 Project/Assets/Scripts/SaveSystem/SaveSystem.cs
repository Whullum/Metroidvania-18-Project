using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class SaveSystem
{
    public static GameData GameData
    {
        get
        {
            return _gameData;
        }
        set
        {
            _gameData = value;
        }
    }

    public static string GameDataPath { get { return Application.persistentDataPath + Path.AltDirectorySeparatorChar + _dataFolderName + _saveName; } }

    private static GameData _gameData;
    private static string _fullPath;
    // The folder name that will store game data.
    private static string _dataFolderName = "data/";
    // Name of the save file.
    private static string _saveName = "gameData.dat";

    public static void SaveGameData()
    {
        Debug.Log("Save System: Saving game state...");

        CheckDataFolder();

        PlayerData player = GameManager.Instance.Player.GetPlayerData();

        GameData = new GameData(DoorManager.DoorData, ResourceManager.CollectedResources, ResourceManager.TotalResource, DrinkInventory.Instance.DrinkInventoryData, player);

        BinaryFormatter formatter = new BinaryFormatter();
        FileStream file = File.Create(_fullPath);

        formatter.Serialize(file, _gameData);
        file.Close();

        Debug.Log("Save System : Game state saved correctly.");
    }

    public static void LoadGameData()
    {
        CheckDataFolder();

        if (!File.Exists(_fullPath)) { Debug.LogError("Save System ERROR : Cannot load from file. File does not exists: " + _fullPath); return; }

        BinaryFormatter formatter = new BinaryFormatter();
        FileStream file = File.Open(_fullPath, FileMode.Open);
        GameData gameData = formatter.Deserialize(file) as GameData;

        file.Close();

        GameData = new GameData(gameData.DoorData, gameData.CollectedPickUps, gameData.ResourceWallet, gameData.DrinkInventory, gameData.PlayerData);

        DoorManager.LoadDoorData();
        ResourceManager.LoadResourcesData();
        DrinkInventory.Instance.LoadDrinkData();
    }

    /// <summary>
    /// Clears all DoorData related information, deleting the save file.
    /// </summary>
    public static void DeleteGameData()
    {
        _fullPath = Application.persistentDataPath + Path.AltDirectorySeparatorChar + _dataFolderName + _saveName;

        if (!File.Exists(_fullPath)) { Debug.LogWarning("Save System WARNING : Cannot delete file. File does not exists: " + _fullPath); return; }

        File.Delete(_fullPath);

        Debug.Log("Save System : Save file deleted successfully.");
    }


    /// <summary>
    /// Checks if the data folder exits, if not, it creates a new one.
    /// </summary>
    private static void CheckDataFolder()
    {
        if (!Directory.Exists(Application.persistentDataPath + Path.AltDirectorySeparatorChar + _dataFolderName))
        {
            Directory.CreateDirectory(Application.persistentDataPath + Path.AltDirectorySeparatorChar + _dataFolderName);
        }

        _fullPath = Application.persistentDataPath + Path.AltDirectorySeparatorChar + _dataFolderName + _saveName;
    }
}
