using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    private static bool _spawned;

    [SerializeField] private GameObject _playerPrefab;

    private void Awake()
    {
        if (_spawned) return;

        SaveSystem.LoadGameData();

        Instantiate(_playerPrefab, transform.position, Quaternion.identity);

        _spawned = true;
    }
}
