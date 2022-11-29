[System.Serializable]
public class DoorData
{
    public string ID;
    public bool IsTraversable;

    public DoorData(string ID, bool isTraversable)
    {
        this.ID = ID;
        this.IsTraversable = isTraversable;
    }
}
