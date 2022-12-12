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

    private void Start() => PlayerUI.Instance.DisablePlayerUI();

    public void SetPlayerInput(bool toggle)
    {
        Player.SetInput(toggle);
        Player.SetInvincibility(!toggle);
    }

    public void RespawnPlayer()
    {
        Time.timeScale = 1;

        PlayerSpawner.PlayerSpawned = false;

        Destroy(Player.gameObject);

        SceneManager.LoadScene("4,11 - 4,12");
    }

    /// <summary>
    /// Used when entering Main Menu screen. Clears the player.
    /// </summary>
    public void ExitGame()
    {
        PlayerSpawner.PlayerSpawned = false;

        Destroy(Player.gameObject);
    }
}
