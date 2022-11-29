using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
public class DoorManager
{
    public static Dictionary<string, bool> _activeDoors = new Dictionary<string, bool>();

    private static string _dataFolderName = "/data/";
    private static string _saveName = "doorData.dat";

    public static void AddDoor(string id, bool isTraversable)
    {
        if (!_activeDoors.ContainsKey(id))
            _activeDoors.Add(id, isTraversable);
        else
            _activeDoors[id] = isTraversable;
    }

    public static DoorData GetDoor(string id)
    {
        if (_activeDoors.ContainsKey(id))
        {
            DoorData door = new DoorData(id, _activeDoors[id]);

            return door;
        }

        return null;
    }

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

        for(int j = 0; j < traveledDoors.Length; j++)
        {
            Debug.Log("ID: " + traveledDoors[j].ID + ". Traversable: " + traveledDoors[j].IsTraversable);
        }

        string fullPath = Application.persistentDataPath + Path.AltDirectorySeparatorChar + _dataFolderName + _saveName;

        BinaryFormatter formatter = new BinaryFormatter();
        FileStream file = File.Create(fullPath);

        formatter.Serialize(file, traveledDoors);
        file.Close();
    }

    public static void UpdateDoor(string id, bool isTraversable)
    {
        if(_activeDoors.ContainsKey(id))
        {
            _activeDoors[id] = isTraversable;
        }
    }

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

            for (int i = 0; i < loadedDoorData.Length; i++)
                AddDoor(loadedDoorData[i].ID, loadedDoorData[i].IsTraversable);
        }
        catch
        {
            Debug.LogErrorFormat("Failed to load file at {0}", fullPath);

            file.Close();
        }
    }

    private static void CheckDataFolder()
    {
        if (!Directory.Exists(Application.persistentDataPath + Path.AltDirectorySeparatorChar + _dataFolderName))
        {
            Directory.CreateDirectory(Application.persistentDataPath + Path.AltDirectorySeparatorChar + _dataFolderName);
        }
    }

    public static void ClearDoorData()
    {
        _activeDoors.Clear();

        string fullPath = Application.persistentDataPath + Path.AltDirectorySeparatorChar + _dataFolderName + _saveName;

        File.Delete(fullPath);
    }
}
