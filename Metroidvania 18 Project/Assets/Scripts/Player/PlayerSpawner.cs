using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    public static bool PlayerSpawned { get; set; }

    [SerializeField] private GameObject _playerPrefab;

    private void Awake()
    {
        if (PlayerSpawned) return;

        Instantiate(_playerPrefab, transform.position, Quaternion.identity);

        PlayerSpawned = true;

        if (!GameManager.Instance.IsNewGame)
            SaveSystem.LoadGameData();
    }
}
