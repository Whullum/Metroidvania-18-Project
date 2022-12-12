using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    public static bool PlayerSpawned { get; set; }

    [SerializeField] private GameObject _playerPrefab;

    private void Awake()
    {
        if (PlayerSpawned) return;

        FindObjectOfType<SceneTransition>().FadeOut(2, Color.black);

        Instantiate(_playerPrefab, transform.position, Quaternion.identity);

        PlayerSpawned = true;

        PlayerUI.Instance.EnablePlayerUI();

        if (!GameManager.Instance.IsNewGame)
            SaveSystem.LoadGameData();
    }
}
