using UnityEditor;
public class SaveSystemEditor
{
    [MenuItem("Utils/Delete Save File")]
    public static void ShowClear()
    {
        SaveSystem.DeleteGameData();
    }
}
