using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public PlayerController Player { get; private set; }

    [SerializeField] private bool _isNewGame;

    protected override void Awake()
    {
        base.Awake();

        Player = FindObjectOfType<PlayerController>();

        if (_isNewGame)
            NewGame();
        else
            LoadGame();
    }

    private void NewGame()
    {
        DoorManager.ClearDoorData();
    }

    private void LoadGame()
    {
        DoorManager.LoadDoorData();
    }

    private void OnApplicationQuit()
    {
        DoorManager.SaveDoorData();
    }
}
