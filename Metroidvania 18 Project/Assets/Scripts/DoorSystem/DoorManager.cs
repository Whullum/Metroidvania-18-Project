using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
public class DoorManager
{
    // Doors loaded in this session.
    public static Dictionary<string, bool> _activeDoors = new Dictionary<string, bool>();

    // The folder name that will store game data.
    private static string _dataFolderName = "data/";
    // Name of the save file.
    private static string _saveName = "doorData.dat";

    /// <summary>
    /// Adds a door to the manager list.
    /// </summary>
    /// <param name="id">Unique ID of this door.</param>
    /// <param name="isTraversable">Sets if the door is Traversable or not.</param>
    public static void AddDoor(string id, bool isTraversable)
    {
        _activeDoors.Add(id, isTraversable);
    }

    /// <summary>
    /// Searchs for a door and if there's one it returns it.
    /// </summary>
    /// <param name="id">The ID of the door.</param>
    /// <returns>DoorData containing usefull information.</returns>
    public static DoorData GetDoor(string id)
    {
        if (_activeDoors.ContainsKey(id))
        {
            DoorData door = new DoorData(id, _activeDoors[id]);

            return door;
        }

        return null;
    }

    public static void UpdateDoor(string id, bool isTraversable)
    {
        if (_activeDoors.ContainsKey(id))
        {
            _activeDoors[id] = isTraversable;
        }
    }

    /// <summary>
    /// Saves the current dictionary status to disk.
    /// </summary>
    public static void SaveDoorData()
    {
        CheckDataFolder();

        DoorData[] traveledDoors = new DoorData[_activeDoors.Count];
        int i = 0;

        foreach (KeyValuePair<string, bool> door in _activeDoors)
        {
            DoorData newDoorData = new DoorData(door.Key, door.Value);
            traveledDoors[i] = newDoorData;
            i++;
        }

        string fullPath = Application.persistentDataPath + Path.AltDirectorySeparatorChar + _dataFolderName + _saveName;

        BinaryFormatter formatter = new BinaryFormatter();
        FileStream file = File.Create(fullPath);

        formatter.Serialize(file, traveledDoors);
        file.Close();
    }

    /// <summary>
    /// Saves from the save file all the DoorData that was used in previous sessions.
    /// </summary>
    public static void LoadDoorData()
    {
        CheckDataFolder();

        string fullPath = Application.persistentDataPath + Path.AltDirectorySeparatorChar + _dataFolderName + _saveName;

        if (!File.Exists(fullPath)) { Debug.LogError("Door Manager ERROR : Cannot load from file. File does not exists: " + fullPath); }

        BinaryFormatter formatter = new BinaryFormatter();
        FileStream file = File.Open(fullPath, FileMode.Open);

        try
        {
            DoorData[] loadedDoorData = formatter.Deserialize(file) as DoorData[];
            file.Close();

            _activeDoors.Clear();

            for (int i = 0; i < loadedDoorData.Length; i++)
                AddDoor(loadedDoorData[i].ID, loadedDoorData[i].IsTraversable);
        }
        catch
        {
            Debug.LogErrorFormat("Failed to load file at {0}", fullPath);

            file.Close();
        }
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
    }

    /// <summary>
    /// Clears all DoorData related information, deleting the save file.
    /// </summary>
    public static void ClearDoorData()
    {
        _activeDoors.Clear();

        string fullPath = Application.persistentDataPath + Path.AltDirectorySeparatorChar + _dataFolderName + _saveName;

        File.Delete(fullPath);
    }
}
