using System.Collections.Generic;
using System.Linq;

public class ResourceManager
{
    public static int TotalResource { get { return _totalResource; } }
    public static ResourceData[] CollectedResources
    {
        get
        {
            return _allResources.ToArray();
        }
    }

    private static int _totalResource;
    private static List<ResourceData> _allResources = new List<ResourceData>();

    public static void CollectResource(string guid, bool collected, int amount)
    {
        UpdateResource(guid, collected);

        _totalResource += amount;
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

    public static void AddResource(string guid, bool collected)
    {
        ResourceData newResource = new ResourceData(guid, collected);

        _allResources.Add(newResource);
    }

    private static void UpdateResource(string guid, bool collected)
    {
        foreach (ResourceData resource in _allResources)
        {
            if (resource.GUID == guid)
            {
                resource.Collected = collected;
                return;
            }
        }
    }

    public static ResourceData GetResource(string guid)
    {
        foreach (ResourceData resource in _allResources)
        {
            if (resource.GUID == guid)
                return resource;
        }
        return null;
    }

    public static void LoadResourcesData()
    {
        _allResources.Clear();
        _allResources = SaveSystem.GameData.CollectedResources.ToList();
        _totalResource = SaveSystem.GameData.ResourceWallet;
    }
}
