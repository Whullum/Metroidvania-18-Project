using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    private static bool _spawned;

    [SerializeField] private GameObject _playerPrefab;
    [SerializeField] private Cinemachine.CinemachineVirtualCamera _camera;

    private void Awake()
    {
        if (_spawned) return;
        //if (FindObjectOfType<PlayerController>() != null) return;

        SaveSystem.LoadGameData();

        Instantiate(_playerPrefab, transform.position, Quaternion.identity);
        _camera.m_Follow = _playerPrefab.transform;

        _spawned = true;
    }
}
