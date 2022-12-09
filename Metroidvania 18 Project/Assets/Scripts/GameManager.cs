using UnityEngine;
using UnityEngine.SceneManagement;

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

    public bool IsNewGame { get; set; } = true;

    private PlayerController _player;

    public void SetPlayerInput(bool toggle)
    {
        Player.SetInput(toggle);
    }

    public void RespawnPlayer()
    {
        PlayerSpawner.PlayerSpawned = false;

        Destroy(Player.gameObject);

        SceneManager.LoadScene("4,11 - 4,12");
    }
}
