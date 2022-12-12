using System.Collections.Generic;
using System.Linq;
using UnityEditor;
//using UnityEditor.PackageManager;

public class ResourceManager
{
    public static int TotalResource { get { return _totalResource; } }
    public static PickUpData[] CollectedResources
    {
        get
        {
            return _allResources.ToArray();
        }
    }

    private static int _totalResource;
    private static List<PickUpData> _allResources = new List<PickUpData>();

    public static void CollectPickUp(string guid, bool collected)
    {
        UpdatePickUp(guid, collected);
    }

    public static bool SpendResource(int amount)
    {
        if (_totalResource - amount < 0)
        {
            // Not enough resource.

            return false;
        }

        _totalResource -= amount;

        return true;
    }

    public static void AddPickUp(string guid, bool collected)
    {
        PickUpData newPickUp = new PickUpData(guid, collected);

        _allResources.Add(newPickUp);
    }

    public static void CollecResource(int amount) => _totalResource += amount;

    private static void UpdatePickUp(string guid, bool collected)
    {
        foreach (PickUpData resource in _allResources)
        {
            if (resource.GUID == guid)
            {
                resource.Collected = collected;
                return;
            }
        }
    }

    public static PickUpData GetPickUp(string guid)
    {
        foreach (PickUpData pickUp in _allResources)
        {
            if (pickUp.GUID == guid)
                return pickUp;
        }
        return null;
    }

    public static void LoadResourcesData()
    {
        _allResources.Clear();
        _allResources = SaveSystem.GameData.CollectedPickUps.ToList();
        _totalResource = SaveSystem.GameData.ResourceWallet;
    }
}
