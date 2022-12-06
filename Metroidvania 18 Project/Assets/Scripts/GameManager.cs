using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    /// <summary>
    /// Unique player instance.
    /// </summary>
    public PlayerController Player
    {
        get
        {
            if (_player == null)
                _player = FindObjectOfType<PlayerController>();

            return _player;
        }
    }

    private PlayerController _player;

    [SerializeField] private bool _isNewGame;

    protected override void Awake()
    {
        base.Awake();

        if (_isNewGame)
            NewGame();
        else
            LoadGame();
    }

    private void NewGame()
    {
        SaveSystem.DeleteGameData();
    }

    private void LoadGame()
    {
        SaveSystem.LoadGameData();
    }

    private void OnDisable()
    {
        SaveSystem.SaveGameData();
    }
}
