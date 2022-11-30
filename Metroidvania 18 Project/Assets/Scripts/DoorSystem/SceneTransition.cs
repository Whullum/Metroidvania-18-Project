using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SceneTransition : MonoBehaviour
{
    // UI image covering all the screen.
    private Image _transition;

    private void Awake()
    {
        _transition = GetComponentInChildren<Image>();
    }

    /// <summary>
    /// Starts a fade in animation.
    /// </summary>
    /// <param name="duration">Duration in seconds.</param>
    /// <param name="transitionColor">Transition color.</param>
    public void FadeIn(float duration, Color transitionColor)
    {
        StartCoroutine(ImageIn(duration, transitionColor));
    }

    /// <summary>
    /// Starts a fade out animation.
    /// </summary>
    /// <param name="duration">Duration in seconds.</param>
    /// <param name="transitionColor">Transition color.</param>
    public void FadeOut(float duration, Color transitionColor)
    {
        StartCoroutine(ImageOut(duration, transitionColor));
    }

    private IEnumerator ImageIn(float duration, Color transitionColor)
    {
        _transition.color = transitionColor; // Set the image color to the specified one.

        Color newColor = new Color(_transition.color.r, _transition.color.g, _transition.color.b, 0); 

        _transition.color = newColor; // Set image alpha to 0.

        float alpha = 0;
        float elapsedTime = 0;

        while (elapsedTime < duration)
        {
            Color color = new Color(_transition.color.r, _transition.color.g, _transition.color.b, alpha);

            _transition.color = color;

            elapsedTime += Time.deltaTime;
            alpha = elapsedTime / duration; //Normalize value between 0 and 1 and slowly increase alpha value.

            yield return null;
        }
    }

    private IEnumerator ImageOut(float duration, Color transitionColor)
    {
        _transition.color = transitionColor;

        Color newColor = new Color(_transition.color.r, _transition.color.g, _transition.color.b, 1);
        
        _transition.color = newColor; // Set image alpha to 1.

        float alpha = 1;
        float elapsedTime = 0;

        while (elapsedTime < duration)
        {
            Color color = new Color(_transition.color.r, _transition.color.g, _transition.color.b, alpha);

            _transition.color = color;

            elapsedTime += Time.deltaTime;
            alpha -= Time.deltaTime / duration; //Normalize value between 0 and 1 and slowly decrease alpha value.

            yield return null;
        }
    }
}
