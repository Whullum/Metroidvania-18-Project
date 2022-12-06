using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DrinkInventory : Singleton<DrinkInventory>
{
    public DrinkData[] DrinkInventoryData
    {
        get
        {
            DrinkData[] data = new DrinkData[_inventory.Count];
            int i = 0;

            foreach (KeyValuePair<Drink, int> drink in _inventory)
            {
                DrinkData newData = new DrinkData(drink.Key.DrinkType.ToString(), drink.Value);
                data[i] = newData;

                i++;
            }

            return data;
        }
    }
    private Dictionary<Drink, int> _inventory = new Dictionary<Drink, int>();
    private TextMeshProUGUI _healthRestoreDrinkText;

    private void Start()
    {
        UpdateUI();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
            UseDrink(DrinkType.Health_Restore);
    }

    public void LoadDrinkData()
    {
        _inventory.Clear();

        Drink[] drinks = Resources.LoadAll<Drink>("Drinks");

        if (drinks.Length <= 0) { Debug.LogError("Drink Inventory ERROR : Cannot load drink resources."); return; }

        for (int i = 0; i < drinks.Length; i++)
        {
            AddDrink(drinks[i], 0);
        }

        for (int i = 0; i < SaveSystem.GameData.DrinkInventory.Length; i++)
        {
            DrinkType drinkType = (DrinkType)System.Enum.Parse(typeof(DrinkType), SaveSystem.GameData.DrinkInventory[i].DrinkType);

            for (int j = 0; j < drinks.Length; j++)
            {
                if (drinkType == drinks[j].DrinkType)
                    AddDrink(drinks[j], SaveSystem.GameData.DrinkInventory[i].Amount);
            }
        }
    }

    public void AddDrink(Drink drink, int amount)
    {
        if (_inventory.ContainsKey(drink))
            _inventory[drink] += amount;
        else
            _inventory.Add(drink, amount);

        UpdateUI();
    }

    private void UseDrink(Drink drink)
    {
        if (drink == null) return;

        if (_inventory.ContainsKey(drink))
            _inventory[drink] -= 1;
    }

    private void UseDrink(DrinkType type)
    {
        switch (type)
        {
            case DrinkType.Health_Restore:

                Drink usedDrink = null;

                foreach (KeyValuePair<Drink, int> drink in _inventory)
                {
                    if (drink.Key.DrinkType == type)
                    {
                        if (drink.Value > 0)
                        {
                            drink.Key.UseDrink();

                            usedDrink = drink.Key;

                            continue;
                        }
                    }
                }

                UseDrink(usedDrink);
                UpdateUI();

                break;

            default:

                break;
        }
    }

    private void UpdateUI()
    {
        if (_healthRestoreDrinkText == null) _healthRestoreDrinkText = GetComponentInChildren<TextMeshProUGUI>();

        foreach (KeyValuePair<Drink, int> drink in _inventory)
        {
            if (drink.Key.DrinkType == DrinkType.Health_Restore)
                _healthRestoreDrinkText.text = drink.Value.ToString();
        }
    }
}
