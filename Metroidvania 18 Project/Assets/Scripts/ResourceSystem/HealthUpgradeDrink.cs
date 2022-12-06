using UnityEngine;

[CreateAssetMenu(fileName ="New Health Upgrade Drink", menuName ="Drinks/Health Upgrade")]
public class HealthUpgradeDrink : Drink
{
    public int HealthUpgrade;

    public override void UseDrink()
    {
        GameManager.Instance.Player.Health.UpgradeHealth(HealthUpgrade);
        Debug.Log("Player Upgrade Health");
    }
}
