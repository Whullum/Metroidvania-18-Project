using UnityEngine;
using UnityEngine.UIElements;

public class PlayerUI : Singleton<PlayerUI>
{
    private VisualElement _root;
    private VisualElement _healthProgress;
    private VisualElement _gunMagazineProgress;
    private Label _healthText;
    private Label _resourceText;
    private Label _healthRestoreDrinkText;
    private Label _gunMagazineText;
    private Label _gunSettingText;

    protected override void Awake()
    {
        base.Awake();

        InitializeDocument();
    }

    private void InitializeDocument()
    {
        _root = GetComponent<UIDocument>().rootVisualElement;

        _healthProgress = _root.Q<VisualElement>("health-progress");
        _gunMagazineProgress = _root.Q<VisualElement>("magazine-progress");
        _healthText = _root.Q<Label>("health-text");
        _healthRestoreDrinkText = _root.Q<Label>("drink-text");
        _resourceText= _root.Q<Label>("resource-text");
        _gunMagazineText= _root.Q<Label>("magazine-ammo-text");
        _gunSettingText= _root.Q<Label>("gun-setting-text");
    }

    public void UpdateUIValues()
    {
        SetHealth();
        SetResources();
        SetGun();
    }

    public void DisablePlayerUI() => _root.style.display = DisplayStyle.None;
    public void EnablePlayerUI() => _root.style.display = DisplayStyle.Flex;

    private void SetHealth()
    {
        float healthProgress = (float)GameManager.Instance.Player.Health.CurrentHealth / (float)GameManager.Instance.Player.Health.MaxHealth * 100f;

        _healthProgress.style.width = Length.Percent(healthProgress);
        _healthText.text = GameManager.Instance.Player.Health.CurrentHealth.ToString();
    }

    private void SetResources()
    {
        int totalDrink = DrinkInventory.Instance.GetTotalDrink(DrinkType.Health_Restore);

        if (totalDrink == -1) totalDrink = 0;

        _healthRestoreDrinkText.text = totalDrink.ToString();
        _resourceText.text = ResourceManager.TotalResource.ToString();
    }

    private void SetGun()
    {
        float gunMagazine = (float)GameManager.Instance.Player.Gun.CurrentMagazine / (float)GameManager.Instance.Player.Gun.ActiveGunSetting.MagazineSize * 100f;

        _gunMagazineProgress.style.width = Length.Percent(gunMagazine);
        _gunMagazineText.text = GameManager.Instance.Player.Gun.CurrentMagazine.ToString("F2");
        _gunSettingText.text = GameManager.Instance.Player.Gun.ActiveGunSetting.name;
    }
}
