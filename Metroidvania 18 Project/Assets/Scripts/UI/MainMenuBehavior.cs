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
    private bool _creditsActive;

    [SerializeField] private string _firstSceneToLoad = "4,11 - 4,12";

    private void Awake()
    {
        InitializeDocument();
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

        _newGameButton.clicked += NewGame;
        _loadGameButton.clicked += LoadGame;
        _creditsButton.clicked += ShowCredits;

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
}
