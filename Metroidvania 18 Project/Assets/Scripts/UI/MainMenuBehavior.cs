using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

/// <summary>
/// Manages main menu behaviors such as buton presses, settings, etc.
/// </summary>
public class MainMenuBehavior : MonoBehaviour
{
    private VisualElement _root;
    private VisualElement _mainMenu;
    private VisualElement _creditsContainer;
    private Button _newGameButton;
    private Button _loadGameButton;
    private Button _creditsButton;
    private Button _exitButton;
    private bool _creditsActive;

    [SerializeField] private string _firstSceneToLoad = "4,11 - 4,12";
    [SerializeField] private AK.Wwise.Event _buttonHoverSound;
    [SerializeField] private AK.Wwise.Event _buttonClickSound;

    private void Awake()
    {
        InitializeDocument();
    }

    private void Start()
    {
        PlayerUI.Instance.DisablePlayerUI();
    }

    private void Update()
    {
        if (_creditsActive && Input.anyKeyDown)
            HideCredits();

    }

    private void InitializeDocument()
    {
        _root = GetComponent<UIDocument>().rootVisualElement;
        _mainMenu = _root.Q<VisualElement>("main-menu");
        _creditsContainer = _root.Q<VisualElement>("credits-container");
        _newGameButton = _root.Q<Button>("new-game");
        _loadGameButton= _root.Q<Button>("load-game");
        _creditsButton= _root.Q<Button>("credits");
        _exitButton = _root.Q<Button>("exit");

        _newGameButton.clicked += NewGame;
        _newGameButton.clicked += PlayClickSound;
        _loadGameButton.clicked += LoadGame;
        _loadGameButton.clicked += PlayClickSound;
        _creditsButton.clicked += ShowCredits;
        _creditsButton.clicked += PlayClickSound;
        _exitButton.clicked += QuitGame;
        _exitButton.clicked += PlayClickSound;

        _newGameButton.RegisterCallback<MouseOverEvent>(PlayHoverSound);
        _loadGameButton.RegisterCallback<MouseOverEvent>(PlayHoverSound);
        _creditsButton.RegisterCallback<MouseOverEvent>(PlayHoverSound);
        _exitButton.RegisterCallback<MouseOverEvent>(PlayHoverSound);

        if (!File.Exists(SaveSystem.GameDataPath))
            _loadGameButton.SetEnabled(false);
    }

    private void NewGame()
    {
        GameManager.Instance.IsNewGame = true;

        SaveSystem.DeleteGameData();

        SceneManager.LoadScene(_firstSceneToLoad);
    }

    private void LoadGame()
    {
        GameManager.Instance.IsNewGame = false;

        SceneManager.LoadScene(_firstSceneToLoad);
    }

    private void ShowCredits()
    {
        _creditsActive = true;
        _mainMenu.style.display = DisplayStyle.None;
        _creditsContainer.style.display = DisplayStyle.Flex;
    }

    private void HideCredits()
    {
        _creditsActive = false;
        _mainMenu.style.display = DisplayStyle.Flex;
        _creditsContainer.style.display = DisplayStyle.None;
    }

    private void PlayHoverSound(MouseOverEvent evt) => _buttonHoverSound.Post(gameObject);
    private void PlayClickSound() => _buttonClickSound.Post(gameObject);

    private void QuitGame() => Application.Quit();
}
