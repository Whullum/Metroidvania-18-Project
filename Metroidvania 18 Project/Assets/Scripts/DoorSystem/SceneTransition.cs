using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SceneTransition : MonoBehaviour
{
    private Image _transition;

    private void Awake()
    {
        _transition = GetComponentInChildren<Image>();
    }

    public void FadeIn(float duration, Color transitionColor)
    {
        StartCoroutine(ImageIn(duration, transitionColor));
    }

    public void FadeOut(float duration, Color transitionColor)
    {
        StartCoroutine(ImageOut(duration, transitionColor));
    }

    private IEnumerator ImageIn(float duration, Color transitionColor)
    {
        _transition.color = transitionColor;

        Color newColor = new Color(_transition.color.r, _transition.color.g, _transition.color.b, 0);

        _transition.color = newColor;

        float alpha = 0;
        float elapsedTime = 0;

        while (elapsedTime < duration)
        {
            Color color = new Color(_transition.color.r, _transition.color.g, _transition.color.b, alpha);

            _transition.color = color;

            elapsedTime += Time.deltaTime;

            alpha = elapsedTime / duration; //Normalize value between 0 and 1

            yield return null;
        }
    }

    private IEnumerator ImageOut(float duration, Color transitionColor)
    {
        _transition.color = transitionColor;

        Color newColor = new Color(_transition.color.r, _transition.color.g, _transition.color.b, 1);
        
        _transition.color = newColor;

        float alpha = 1;
        float elapsedTime = 0;

        while (elapsedTime < duration)
        {
            Color color = new Color(_transition.color.r, _transition.color.g, _transition.color.b, alpha);

            _transition.color = color;

            elapsedTime += Time.deltaTime;

            alpha -= Time.deltaTime / duration;

            yield return null;
        }
    }
}
