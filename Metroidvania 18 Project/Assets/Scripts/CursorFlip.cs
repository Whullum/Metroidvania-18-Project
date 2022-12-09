using UnityEngine;

public class CursorFlip : Singleton<CursorFlip>
{
    private CursorMode _cursorMode = CursorMode.Auto;

    [SerializeField] private Texture2D _reticalOpen;
    [SerializeField] private Texture2D _reticalClosed;

    protected override void Awake()
    {
        base.Awake();

        ChangueCursor(_reticalOpen);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
            ChangueCursor(_reticalClosed);
        if (Input.GetMouseButtonUp(0))
            ChangueCursor(_reticalOpen);
    }

    private void ChangueCursor(Texture2D texture)
    {
        Vector2 hotSpot = new Vector2(texture.width / 2, texture.height / 2);
        Cursor.SetCursor(texture, hotSpot, _cursorMode);
    }
}
