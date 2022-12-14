using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class PauseMenu : MonoBehaviour
{
    public bool IsActive { get; set; }

    private VisualElement _root;
    private Button _resumeGame;
    private Button _toMainMenu;
    private Button _quitGame;

    [Header("States for changing audio on pause")]
    [SerializeField] private AK.Wwise.State _normalAudioState;
    [SerializeField] private AK.Wwise.State _pausedAudioState;
    [SerializeField] private AK.Wwise.Event _buttonHoverSound;
    [SerializeField] private AK.Wwise.Event _buttonClickSound;

    private void Awake()
    {
        InitializeDocument();
    }

    private void InitializeDocument()
    {
        _root = GetComponent<UIDocument>().rootVisualElement;

        _resumeGame = _root.Q<Button>("resume");
        _toMainMenu = _root.Q<Button>("main-menu");
        _quitGame = _root.Q<Button>("quit");

        _resumeGame.clicked += ResumeGame;
        _resumeGame.clicked += PlayClickSound;
        _toMainMenu.clicked += MainMenu;
        _toMainMenu.clicked += PlayClickSound;
        _quitGame.clicked += QuitGame;
        _quitGame.clicked += PlayClickSound;

        _resumeGame.RegisterCallback<MouseOverEvent>(PlayHoverSound);
        _toMainMenu.RegisterCallback<MouseOverEvent>(PlayHoverSound);
        _quitGame.RegisterCallback<MouseOverEvent>(PlayHoverSound);

        _root.style.display = DisplayStyle.None;
    }

    public void ResumeGame()
    {
        GameManager.Instance.SetPlayerInput(true);
        Time.timeScale = 1;
        _root.style.display = DisplayStyle.None;
        IsActive = false;
        _normalAudioState.SetValue();

    }

    private void MainMenu()
    {
        GameManager.Instance.ExitGame();
        _root.style.display = DisplayStyle.None;
        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenu");
        IsActive = false;
        _normalAudioState.SetValue();
    }

    private void QuitGame() => Application.Quit();
    private void PlayHoverSound(MouseOverEvent evt) => _buttonHoverSound.Post(gameObject);
    private void PlayClickSound() => _buttonClickSound.Post(gameObject);

    public void EnablePauseMenu()
    {
        GameManager.Instance.SetPlayerInput(false);
        Time.timeScale = 0;
        _root.style.display = DisplayStyle.Flex;
        IsActive = true;
        _pausedAudioState.SetValue();
    }
}
