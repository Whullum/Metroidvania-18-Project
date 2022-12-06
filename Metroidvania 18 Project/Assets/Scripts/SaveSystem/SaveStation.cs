using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class SaveStation : MonoBehaviour
{
    private VisualElement _root;
    private VisualElement _saveGamePopUp;
    private bool _canInteract = false;
    private bool _savingGame;
    private Canvas _interactionCanvas;
    private ParticleSystem _saveParticles;

    [SerializeField] private float _popUpDuration = 2.0f;
    [SerializeField] private KeyCode _interactionKey = KeyCode.E;

    private void Awake()
    {
        InitializeDocument();
        _interactionCanvas = GetComponentInChildren<Canvas>();
        _interactionCanvas.gameObject.SetActive(false);
        _saveParticles = GetComponentInChildren<ParticleSystem>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !_savingGame)
        {
            _canInteract = true;
            _interactionCanvas.gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            _canInteract = false;
            _interactionCanvas.gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        if (_canInteract && Input.GetKeyDown(_interactionKey))
            StartCoroutine(SaveGame());
    }

    private void InitializeDocument()
    {
        _root = GetComponent<UIDocument>().rootVisualElement;
        _saveGamePopUp = _root.Q<VisualElement>("game-saved-popup");
    }

    private IEnumerator SaveGame()
    {
        if (_savingGame) yield break;

        _saveParticles.Play();
        _savingGame = true;
        _canInteract = false;

        SaveSystem.SaveGameData();

        _interactionCanvas.gameObject.SetActive(false);
        _saveGamePopUp.RemoveFromClassList("game-save-out");
        _saveGamePopUp.AddToClassList("game-save-in");

        yield return new WaitForSeconds(_popUpDuration);

        _saveGamePopUp.RemoveFromClassList("game-save-in");
        _saveGamePopUp.AddToClassList("game-save-out");
        _savingGame = false;
    }
}
