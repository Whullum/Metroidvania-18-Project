using System.Collections.Generic;

public class DoorManager
{
    public static DoorData[] DoorData
    {
        get
        {
            DoorData[] data = new DoorData[_activeDoors.Count];
            int i = 0;

            foreach (KeyValuePair<string, bool> door in _activeDoors)
            {
                DoorData newDoorData = new DoorData(door.Key, door.Value);
                data[i] = newDoorData;
                i++;
            }
            return data;
        }
    }
    // Doors loaded in this session.
    public static Dictionary<string, bool> _activeDoors = new Dictionary<string, bool>();

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
    /// Saves from the save file all the DoorData that was used in previous sessions.
    /// </summary>
    public static void LoadDoorData()
    {
        _activeDoors.Clear();

        for (int i = 0; i < SaveSystem.GameData.DoorData.Length; i++)
            AddDoor(SaveSystem.GameData.DoorData[i].ID, SaveSystem.GameData.DoorData[i].IsTraversable);
    }
}
