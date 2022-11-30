using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : MonoBehaviour
{
    // Unique identifier of this door, composed from the doorConnection name string and the level to load string.
    private string _ID;
    // The SceneTransition component of this scene.
    private SceneTransition _sceneTransition; 
    // Active link used at this time by the player to move between scenes.
    private static DoorConnection _activeConnection;
    // The scene that is being loaded.
    private AsyncOperation _loadedScene;

    [Tooltip("Link between two doors.")]
    [SerializeField] private DoorConnection _doorConnection;
    [Tooltip("Name of the level to load when reaching this door.")]
    [SerializeField] private string _levelToLoad;
    [Tooltip("Point where the player will be placed when enters the scene trough this door.")]
    [SerializeField] private Transform _playerSpawnPoint;
    [Tooltip("Set the player sprite renderer to face ledt of right.")]
    [SerializeField] private bool _faceRight;
    [Tooltip("Enables this door to be used to travel between scenes. Used for creating one way doors.")]
    [SerializeField] private bool _isTraversable = true;

    private void Awake()
    {
        _sceneTransition = FindObjectOfType<SceneTransition>();

        GenerateUniquePersistentID();
    }

    private void Start()
    {
        InitializeDoor();

        // If this is the active connection that the player is using to travel between levels, then this door is used as the initializer.
        if (_activeConnection == _doorConnection)
        {
            _sceneTransition.FadeOut(_doorConnection.TransitionTime, _activeConnection.TransitionColor); // Starts the fade out transition.

            SetPlayer();

            _isTraversable = true; // If this was an one way door, we set is Traversable property to true, because is now unlocked.

            DoorManager.UpdateDoor(_ID, _isTraversable); // Update the current door state.
            DoorManager.SaveDoorData(); // Persist the changues saving it to disk.
        }
    }

    /// <summary>
    /// Check if the DoorManager already has thsi door in its list, if so, update the value.
    /// </summary>
    private void InitializeDoor()
    {
        DoorData door = DoorManager.GetDoor(_ID);

        if (door == null)
            DoorManager.AddDoor(_ID, _isTraversable);
        else
            _isTraversable = door.IsTraversable;
    }

    /// <summary>
    /// Initializes the player in this scene, setting its position and facing direction.
    /// </summary>
    private void SetPlayer()
    {
        GameManager.Instance.Player.transform.position = _playerSpawnPoint.position;

        GameManager.Instance.Player.GetComponent<SpriteRenderer>().flipY = _faceRight;
    }

    /// <summary>
    /// Loads a scene by its name and prevents activating it automaticly when finishes loading.
    /// </summary>
    private void LoadScene()
    {
        _loadedScene = SceneManager.LoadSceneAsync(_levelToLoad);

        _loadedScene.allowSceneActivation = false;
    }

    /// <summary>
    /// Allows the loaded scene to be activated.
    /// </summary>
    private void ActivateScene()
    {
        _loadedScene.allowSceneActivation = true;
    }

    private void GenerateUniquePersistentID() => _ID = _doorConnection.name + _levelToLoad;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!_isTraversable) return;

        if (collision.collider.CompareTag("Player"))
        {
            _activeConnection = _doorConnection; // Set the link between the two doors to be this.

            _sceneTransition.FadeIn(_activeConnection.TransitionTime, _activeConnection.TransitionColor); // Starts the fade in transition.

            LoadScene(); // Load the desired scene.

            Invoke("ActivateScene", _activeConnection.TransitionTime); // When the transition time ends, the loaded scene is activated.
        }
    }
}
