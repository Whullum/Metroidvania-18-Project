[System.Serializable]
public class PickUpData
{
    public string GUID;
    public bool Collected;

    public PickUpData(string guid, bool collected)
    {
        this.GUID = guid;
        this.Collected = collected;
    }
}
