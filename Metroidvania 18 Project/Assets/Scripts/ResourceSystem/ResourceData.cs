[System.Serializable]
public class ResourceData
{
    public string GUID;
    public bool Collected;

    public ResourceData(string guid, bool collected)
    {
        this.GUID = guid;
        this.Collected = collected;
    }
}
