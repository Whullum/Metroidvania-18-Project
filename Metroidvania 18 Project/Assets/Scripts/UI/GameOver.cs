using UnityEngine;
using UnityEngine.UIElements;

public class GameOver : MonoBehaviour
{
    private void Start()
    {
        ShowGameOverScreen();
    }

    private void ShowGameOverScreen()
    {
        VisualElement root = GetComponent<UIDocument>().rootVisualElement;
        VisualElement gameOver = root.Q<VisualElement>("game-over");

        gameOver.ToggleInClassList("game-over-out");
        gameOver.ToggleInClassList("game-over-in");
    }
}
