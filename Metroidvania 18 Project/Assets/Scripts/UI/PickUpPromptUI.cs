using System.Collections;
using TMPro;
using UnityEngine;

public class PickUpPromptUI : MonoBehaviour
{
    private TextMeshProUGUI _promptText;

    [SerializeField] private float _lifeTime = 2.0f;
    [SerializeField] private float _upSpeed = 0.01f;

    private void Awake()
    {
        _promptText = GetComponentInChildren<TextMeshProUGUI>();
    }

    private void Start()
    {
        StartCoroutine(FadeOut());
    }

    private void Update()
    {
        transform.Translate(Vector3.up * _upSpeed);
    }

    private IEnumerator FadeOut()
    {
        CanvasGroup canvasGroup = GetComponentInChildren<CanvasGroup>();

        while(canvasGroup.alpha > 0)
        {
            canvasGroup.alpha -= Time.deltaTime / _lifeTime;

            yield return null;
        }
        Destroy(gameObject);
    }

    public void SetPromptText(string text) => _promptText.text = text;
}
