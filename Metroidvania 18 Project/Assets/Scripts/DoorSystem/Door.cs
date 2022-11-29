using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : MonoBehaviour
{
    private string _ID;
    private SceneTransition _sceneTransition;
    private static DoorConnection _activeConnection;
    private static PlayerController _player;
    private AsyncOperation _loadedScene;

    [SerializeField] private DoorConnection _doorConnection;
    [SerializeField] private string _levelToLoad;
    [SerializeField] private Transform _playerSpawnPoint;
    [SerializeField] private bool _faceRight;
    [SerializeField] private bool _isTraversable = true;

    private void Awake()
    {
        if (!_player) _player = FindObjectOfType<PlayerController>();

        _sceneTransition = FindObjectOfType<SceneTransition>();

        _ID = _doorConnection.name + _levelToLoad;
    }

    private void Start()
    {
        InitializeDoor();

        if (_activeConnection == _doorConnection)
        {
            _sceneTransition.FadeOut(_doorConnection.TransitionTime, _activeConnection.TransitionColor);

            SetPlayer();

            _isTraversable = true;

            DoorManager.UpdateDoor(_ID, _isTraversable);
        }
    }

    private void InitializeDoor()
    {
        DoorData door = DoorManager.GetDoor(_ID);

        if (door == null)
            DoorManager.AddDoor(_ID, _isTraversable);
        else
            _isTraversable = door.IsTraversable;
    }

    private void SetPlayer()
    {
        _player.transform.position = _playerSpawnPoint.position;

        _player.GetComponent<SpriteRenderer>().flipY = _faceRight;
    }

    private void LoadScene()
    {
        _loadedScene = SceneManager.LoadSceneAsync(_levelToLoad);

        _loadedScene.allowSceneActivation = false;
    }

    private void ActivateScene()
    {
        _loadedScene.allowSceneActivation = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!_isTraversable) return;

        if (collision.collider.CompareTag("Player"))
        {
            _activeConnection = _doorConnection;

            _sceneTransition.FadeIn(_activeConnection.TransitionTime, _activeConnection.TransitionColor);

            LoadScene();

            Invoke("ActivateScene", _activeConnection.TransitionTime);
        }
    }
}
