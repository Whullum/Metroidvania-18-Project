using UnityEngine;

[CreateAssetMenu(fileName = "New Health Restore Drink", menuName = "Drinks/Health Restore")]
public class HealthRestoreDrink : Drink
{
    public int HealthRestore;

    public override void UseDrink()
    {
        GameManager.Instance.Player.Health.RestoreHealth(HealthRestore);
        Debug.Log("Player health restore");
    }
}
