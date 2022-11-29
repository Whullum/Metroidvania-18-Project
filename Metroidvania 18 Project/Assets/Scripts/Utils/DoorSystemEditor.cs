using UnityEditor;

public class DoorSystemEditor : EditorWindow
{
    [MenuItem("Utils/Clear Door Data")]
    public static void ShowClear()
    {
        DoorManager.ClearDoorData();
    }
}
