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
        _toMainMenu.clicked += MainMenu;
        _quitGame.clicked += QuitGame;

        _root.style.display = DisplayStyle.None;
    }

    public void ResumeGame()
    {
        GameManager.Instance.SetPlayerInput(true);
        Time.timeScale = 1;
        _root.style.display = DisplayStyle.None;
        IsActive = false;
    }

    private void MainMenu()
    {
        GameManager.Instance.ExitGame();
        _root.style.display = DisplayStyle.None;
        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenu");
        IsActive = false;
    }

    private void QuitGame() => Application.Quit();

    public void EnablePauseMenu()
    {
        GameManager.Instance.SetPlayerInput(false);
        Time.timeScale = 0;
        _root.style.display = DisplayStyle.Flex;
        IsActive = true;
    }
}
