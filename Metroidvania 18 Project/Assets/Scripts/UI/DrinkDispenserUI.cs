using UnityEngine;
using UnityEngine.UIElements;

public class DrinkDispenserUI : MonoBehaviour
{
    private VisualElement _root;
    private VisualElement _dispenserUI;
    private Label _resourceAmount;
    private Button _closeButton;
    private bool _isOpen;
    private bool _canOpen;
    private Drink[] _drinks;
    private Canvas _interactionCanvas;

    [SerializeField] private AK.Wwise.Event _buttonHoverSound;
    [SerializeField] private AK.Wwise.Event _buttonClickSound;

    private void Start()
    {
        InitializeDocument();

        _interactionCanvas = GetComponentInChildren<Canvas>();
        _interactionCanvas.gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            _canOpen = true;
            _interactionCanvas.gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            _canOpen = false;
            _interactionCanvas.gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && _canOpen)
            ToggleUI();
    }

    private void InitializeDocument()
    {
        _root = GetComponent<UIDocument>().rootVisualElement;
        _dispenserUI = _root.Q<VisualElement>("dispenser-ui");
        _resourceAmount = _root.Q<Label>("resource-amount");
        _closeButton = _root.Q<Button>("close-dispenser");

        _closeButton.clicked += ToggleUI;
        _closeButton.RegisterCallback<MouseOverEvent>(PlayHoverSound);

        CreateDrinkUI();
    }

    private void ToggleUI()
    {
        _isOpen = !_isOpen;

        if (_isOpen)
        {
            _dispenserUI.RemoveFromClassList("drink-dispenser-out");
            _dispenserUI.AddToClassList("drink-dispenser-in");
            UpdateUI();
            GameManager.Instance.SetPlayerInput(false);
            _closeButton.SetEnabled(true);
        }
        else
        {
            _buttonClickSound.Post(gameObject);

            _dispenserUI.RemoveFromClassList("drink-dispenser-in");
            _dispenserUI.AddToClassList("drink-dispenser-out");
            _canOpen = false;
            PlayerUI.Instance.UpdateUIValues();
            GameManager.Instance.SetPlayerInput(true);
            _closeButton.SetEnabled(false);
        }
    }

    private void CreateDrinkUI()
    {
        _drinks = Resources.LoadAll<Drink>("Drinks");

        if (_drinks.Length <= 0) { Debug.LogError("Drink Dispenser UI ERROR : No ScriptableObjects to load inside Drinks folder."); return; }

        _resourceAmount.text = ResourceManager.TotalResource.ToString();
        VisualElement drinksContainer = _dispenserUI.Q<VisualElement>("drinks-container");

        for (int i = 0; i < _drinks.Length; i++)
        {
            VisualElement newDrinkElement = new VisualElement();
            newDrinkElement.AddToClassList("drink-element");

            Image newDrinkImage = new Image();
            newDrinkImage.sprite = _drinks[i].DrinkImage;
            newDrinkImage.AddToClassList("drink-image");

            Label newDrinkName = new Label(_drinks[i].name);
            newDrinkName.AddToClassList("drink-text");

            Label newDrinkCost = new Label(_drinks[i].Cost.ToString());
            newDrinkCost.AddToClassList("buy-text");

            Button newDrinkBuyButton = new Button();
            newDrinkBuyButton.text = "Craft Drink";
            newDrinkBuyButton.AddToClassList("drink-buy-button");
            newDrinkBuyButton.name = _drinks[i].DrinkType.ToString();
            newDrinkBuyButton.RegisterCallback<ClickEvent>(BuyDrink);
            newDrinkBuyButton.RegisterCallback<MouseOverEvent>(PlayHoverSound);

            drinksContainer.Add(newDrinkElement);

            newDrinkElement.Add(newDrinkImage);
            newDrinkElement.Add(newDrinkName);
            newDrinkElement.Add(newDrinkCost);
            newDrinkElement.Add(newDrinkBuyButton);
        }
    }

    private void BuyDrink(ClickEvent evt)
    {
        Button clickedButton = evt.currentTarget as Button;
        DrinkType selectedDrink = (DrinkType)System.Enum.Parse(typeof(DrinkType), clickedButton.name);

        _buttonClickSound.Post(gameObject);

        for (int i = 0; i < _drinks.Length; i++)
        {
            if (_drinks[i].DrinkType == selectedDrink)
            {
                if (ResourceManager.SpendResource(_drinks[i].Cost))
                {
                    if (selectedDrink == DrinkType.Health_Upgrade)
                        _drinks[i].UseDrink();
                    else
                        DrinkInventory.Instance.AddDrink(_drinks[i], 1);
                }
                else
                {
                    // Not enough resource.
                    // Error sound.
                }
                UpdateUI();
            }
        }
    }

    private void UpdateBuyButtons()
    {
        _dispenserUI.Query(className: "drink-buy-button")
            .ForEach((element) =>
            {
                for (int i = 0; i < _drinks.Length; i++)
                {
                    if (element.name == _drinks[i].DrinkType.ToString())
                    {
                        if (_drinks[i].Cost > ResourceManager.TotalResource)
                            element.SetEnabled(false);
                        else
                            element.SetEnabled(true);
                    }
                }
            });
    }

    private void PlayHoverSound(MouseOverEvent evt) => _buttonHoverSound.Post(gameObject);

    private void UpdateUI()
    {
        UpdateBuyButtons();
        _resourceAmount.text = ResourceManager.TotalResource.ToString();
        PlayerUI.Instance.UpdateUIValues();
    }
}
